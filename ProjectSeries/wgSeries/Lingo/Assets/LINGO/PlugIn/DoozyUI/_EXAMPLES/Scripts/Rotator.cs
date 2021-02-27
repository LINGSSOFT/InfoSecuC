using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rotator : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    public float speed = 10f;
    public Axis axis = Axis.Y;

    private Vector3 direction;

    void Start()
    {
        switch (axis)
        {
            case Axis.X: direction = new Vector3(1, 0, 0); break;
            case Axis.Y: direction = new Vector3(0, 1, 0); break;
            case Axis.Z: direction = new Vector3(0, 0, 1); break;
        }

        transform.DORotate(direction, speed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetSpeedBased(true)
            .Play();
    }

}
