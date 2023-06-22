using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMover : MonoBehaviour
{
    public float maxJump = 100, jumpMult = 2;
    public float maxPush = 100, pushMult = 2;
    public float DebugJumpForce = 10, DebugSlideForce = 10;
    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void UpdateHeadForce(Vector3 f)
    {
        float squatForce = Mathf.Clamp(jumpMult * f.y, 0, maxJump);
        body.AddForce(transform.up * squatForce);
    }

    public void DebugJump()
    {
        Vector3 f = new Vector3(0, DebugJumpForce, 0);
        body.AddForce(transform.rotation * f);
    }

    public void Kickback( Vector3 f)
    {
        body.AddForce(f / Time.deltaTime);
        // body.velocity = body.velocity + f;
        Debug.Log("Kickback");
    }

}