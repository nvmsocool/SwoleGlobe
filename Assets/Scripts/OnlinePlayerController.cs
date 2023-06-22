using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayerController : MonoBehaviour
{
    public Transform head, handL, handR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReceiveState(ServerInterface.PlayerState ps)
    {
        // set position, set pose, spawn projectiles, etc
        transform.localPosition = ps.pose.position;
        handL.position = ps.leftHand.position;
        handR.position = ps.rightHand.position;
    }
}
