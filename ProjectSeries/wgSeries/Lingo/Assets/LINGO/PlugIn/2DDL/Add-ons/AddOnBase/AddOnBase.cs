namespace DynamicLight2D
{
	using UnityEngine;
	using System.Collections;
	using DynamicLight2D;
	
	public class AddOnBase : MonoBehaviour {
		
		public DynamicLight dynamicLightInstance
		{
			get {
				if(_cLight == null){
					_cLight = GetComponent<DynamicLight>(); 
				} 
				return _cLight;
			}
		}
		
		private DynamicLight _cLight;
		
		
		public virtual void Start () {
			// override
		}
		
		
		public virtual void Update () {
			// override
		}
	}
}


