using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMover : MonoBehaviour
{
    public float maxJump = 100, jumpMult = 2;
    public float maxPush = 100, pushMult = 2;
    public float rotSpeed = 1;
    public Transform myHandL, myHandR, rotRoot;
    public Rigidbody body;

    private UnityEngine.XR.InputDevice leftDevice, rightDevice, headDevice;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.InputDevices.deviceConnected += TryGetDevices;
    }

    private void TryGetDevices(UnityEngine.XR.InputDevice device)
    {
        if (device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand) &&
            device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.Controller))
        {
            if (device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.Left))
                leftDevice = device;

            if (device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.Right))
                rightDevice = device;
        }
        if (device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.HeadMounted))
        {
            headDevice = device;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!headDevice.isValid)
            return;

        // set avatar to match player
        Vector3 headPos;
        if (headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out headPos))
        {
            UpdateAvatarHand(leftDevice, myHandL, headPos);
            UpdateAvatarHand(rightDevice, myHandR, headPos);
        }

        // move
        body.AddForce(GetHeadForce() - GetHandForce());

        //turn
        if (leftDevice.isValid)
        {
            Vector2 thumb;
            if (leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out thumb))
            {
                rotRoot.Rotate(new Vector3(0, rotSpeed * thumb.x, 0));
            }
        }
    }

    private Vector3 GetHeadForce()
    {
        Vector3 vel = Vector3.zero;
        if (headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out vel))
        {
            vel.x = 0;
            vel.y = Mathf.Clamp(jumpMult * vel.y, 0, maxJump);
            vel.z = 0;
        }
        return vel;
    }

    private Vector3 GetHandForce()
    {
        Vector3 pushForce = GetHandForce(leftDevice) + GetHandForce(rightDevice);
        float pushMag = pushForce.magnitude;
        pushForce = pushMag == 0 ? pushForce : (pushForce / pushMag) * Mathf.Min(maxPush, pushMag * pushMult);
        return pushForce;
    }

    private void UpdateAvatarHand(UnityEngine.XR.InputDevice device, Transform t, Vector3 headPos)
    {
        Vector3 pos;
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out pos))
        {
            t.localPosition = pos - headPos;
        }
    }

    private Vector3 GetHandForce(UnityEngine.XR.InputDevice device)
    {
        Vector3 ret = Vector3.zero;
        if (!device.isValid)
            return ret;

        bool triggerValue;
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out triggerValue) && triggerValue)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out ret))
            {
                ret.y = 0;
            }
        }
        return ret;
    }
}