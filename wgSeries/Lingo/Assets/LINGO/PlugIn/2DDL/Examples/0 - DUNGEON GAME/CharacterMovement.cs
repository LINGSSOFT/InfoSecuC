using UnityEngine;
using System.Collections;
using Prime31;


public class CharacterMovement : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private float normalizedVerticalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	//private  UnityEngine.SceneManagement.Scene _scene;

	private static int currentLevel;

	private bool firingTrigger = false;
	private bool firingDead = false;

	private AudioSource jumpSound;

	bool wantJump;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		//_scene  = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		currentLevel = 0;//_scene.buildIndex;

		jumpSound = GetComponent<AudioSource>();

	}

	void Start(){
		StartCoroutine(CharacterUpdate());
	}

	void Update(){
		if(Input.GetKeyDown( KeyCode.UpArrow))
			wantJump = true;
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		string tag = col.gameObject.tag;
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
		if(tag == "deadTrigger"){
			firingDead = true;
		}else if(tag == "onlyTrigger"){
			firingTrigger = true;
		}
	}


	void onTriggerExitEvent( Collider2D col )
	{
		//firingTrigger = false;
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	IEnumerator CharacterUpdate()
	{
		while(true){
			if(gravity !=0){
				if( _controller.isGrounded )
					_velocity.y = 0;
				
				
				if( Input.GetKey( KeyCode.RightArrow ) )
				{
					normalizedHorizontalSpeed = 1;
					if( transform.localScale.x < 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
					
					if( _controller.isGrounded )
						_animator.Play( Animator.StringToHash( "Run" ) );
				}
				else if( Input.GetKey( KeyCode.LeftArrow ) )
				{
					normalizedHorizontalSpeed = -1;
					if( transform.localScale.x > 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
					
					if( _controller.isGrounded )
						_animator.Play( Animator.StringToHash( "Run" ) );
				}
				else
				{
					normalizedHorizontalSpeed = 0;
					
					if( _controller.isGrounded )
						_animator.Play( Animator.StringToHash( "Idle" ) );
				}
				
				// we can only jump whilst grounded
				if( _controller.isGrounded && wantJump)
				{
					wantJump = false;
					if(!firingTrigger){
						_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
						_animator.Play( Animator.StringToHash( "Jump" ) );
						if(jumpSound)
							jumpSound.Play();

						yield return new WaitForEndOfFrame();
					
					} else{
						// Switch level 
						Debug.Log("Load next Level");
						loadNextLevel();
					}
					
				}

				if(firingDead){
					reloadLevel();
				}
				
				var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
				_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
				
				// apply gravity before moving
				_velocity.y += gravity * Time.deltaTime;
				
				// if holding down bump up our movement amount and turn off one way platform detection for a frame.
				// this lets uf jump down through one way platforms
				if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
				{
					_velocity.y *= 3f;
					_controller.ignoreOneWayPlatformsThisFrame = true;
				}

				yield return new WaitForEndOfFrame();
				_controller.move( _velocity * Time.deltaTime );
				
				// grab our current _velocity to use as a base for all calculations
				_velocity = _controller.velocity;
				


			}
			
			

			
			if(gravity == 0){

				normalizedVerticalSpeed = 0;
				normalizedHorizontalSpeed = 0;
				



				if(Input.GetKey( KeyCode.UpArrow )){
					normalizedVerticalSpeed = 1;
					//normalizedHorizontalSpeed = 0;
					_animator.Play( Animator.StringToHash( "Run" ) );

					if(firingTrigger == true)
						loadNextLevel();
					
				}
				if(Input.GetKey( KeyCode.DownArrow )){
					normalizedVerticalSpeed = -1;
					//normalizedHorizontalSpeed = 0;
					_animator.Play( Animator.StringToHash( "Run" ) );
				}
				if(Input.GetKey( KeyCode.LeftArrow )){
					normalizedHorizontalSpeed = -1;
					//normalizedVerticalSpeed = 0;
					if( transform.localScale.x > 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
					
					_animator.Play( Animator.StringToHash( "Run" ) );
				}
				if(Input.GetKey( KeyCode.RightArrow )){
					normalizedHorizontalSpeed = 1;
					//normalizedVerticalSpeed = 0;
					if( transform.localScale.x < 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
					
					_animator.Play( Animator.StringToHash( "Run" ) );
				}
					
				if(!Input.anyKey)
					_animator.Play( Animator.StringToHash( "Idle" ) );
				
				//yield return null;
				//var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
				_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * groundDamping);
				_velocity.y = Mathf.Lerp( _velocity.y, normalizedVerticalSpeed * runSpeed, Time.deltaTime * groundDamping);
				


				_controller.move( _velocity * Time.deltaTime );
				
				// grab our current _velocity to use as a base for all calculations
				_velocity = _controller.velocity;
				//sda
				yield return new WaitForEndOfFrame();
				//yield return new WaitForEndOfFrame ();
				
			}
		}

	}

	internal void reloadLevel(){
		firingDead = false;
		Debug.Log (currentLevel);
		#if UNITY_5_3_OR_NEWER
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentLevel);
		#else
		Application.LoadLevelAsync(Application.loadedLevel);
		#endif
		
		//
		
	}
	
	internal void loadNextLevel(){
		firingTrigger = false;
		currentLevel ++;
		
		#if UNITY_5_3_OR_NEWER
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
		#else
		Application.LoadLevelAsync(Application.loadedLevel + 1);
		#endif
		
	}


}
