using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationManager : MonoBehaviour
{

    public float rotSpeed = 1;

    //allow for re-configuration of rotational axes?

    public void SetLeftThumb(Vector2 thumb)
    {
        transform.Rotate(new Vector3(rotSpeed * thumb.y, rotSpeed * thumb.x, 0));
    }

    public void SetRightThumb(Vector2 thumb)
    {
        transform.Rotate(new Vector3(0, 0, rotSpeed * thumb.x));
    }
}
