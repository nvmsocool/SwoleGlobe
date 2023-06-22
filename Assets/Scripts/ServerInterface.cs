using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInterface : MonoBehaviour
{

    bool alone = true;
    public Transform myPose;
    public OnlinePlayerController[] onlinePlayerControllers;
    public Transform myTransform, boardTransform;
    
    public struct PlayerState
    {
        public Pose pose;
        public bool anchored;
        public bool fired;
        public Pose leftHand, rightHand;
    }

    public enum HandMode
    {
        OPEN,
        FIST,
        CASTING,
        THUMBSUP,
        POINTING,
        GRABBING,
        NUM_MODES
    }

    public struct HandState
    {
        public Pose position;
        public HandMode mode;
    }

    public struct ProjectileState
    {
        public Pose position;
        public float speed;
        public int strength;
    }

    private PlayerState[] playerStates;

    // Start is called before the first frame update
    void Start()
    {
        playerStates = new PlayerState[onlinePlayerControllers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (alone)
        {
            // fake online info
            // TODO: transform into local space??
            Pose myPose = new Pose(myTransform.position - boardTransform.position, myTransform.rotation);

            for(int j = 0; j < playerStates.Length; j++)
            {
                playerStates[j].pose = myPose;
            }

            // fake 7 other players
            int i = -1;
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.x *= -1;
            }
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.y *= -1;
            }
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.x *= -1;
                playerStates[i].pose.position.y *= -1;
            }

            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.z *= -1;
            }
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.z *= -1;
                playerStates[i].pose.position.x *= -1;
            }
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.z *= -1;
                playerStates[i].pose.position.y *= -1;
            }
            if (onlinePlayerControllers.Length >= ++i)
            {
                playerStates[i].pose.position.z *= -1;
                playerStates[i].pose.position.x *= -1;
                playerStates[i].pose.position.y *= -1;
            }
        }
        else
        {
            // send online info
            // receive
        }

        // players
        for (int i = 0; i < playerStates.Length; i++)
        {
            onlinePlayerControllers[i].ReceiveState(playerStates[i]);
        }

        // projectiles
    }
}
