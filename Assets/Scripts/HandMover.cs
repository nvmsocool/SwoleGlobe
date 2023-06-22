using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMover : MonoBehaviour
{
    public Object orbPrefab;
    private GameObject orb;
    private Rigidbody orbBody;

    private int histIndex;
    private Vector3[] velHist;
    private Vector3 throwVel;
    private static int samples = 10;

    private Vector3 headPos;

    private bool isTryingToGrab = false, isSpawning = false, themAnchored = false;
    private List<GameObject> holdTargets, anchorTargets;

    private GameObject heldObject = null, anchoredObject = null;

    public EventVector3 kickbackEvent;
    public EventGameObject anchorEvent;

    private void Start()
    {
        //spawn orb
        orb = Instantiate(orbPrefab) as GameObject;
        orbBody = orb.GetComponent<Rigidbody>();
        orb.SetActive(false);
        velHist = new Vector3[samples];
    }

    public void UpdateVelocity(Vector3 v)
    {
        throwVel += (v - velHist[histIndex]) / samples;
        velHist[histIndex] = v;
        histIndex++;
    }

    public void UpdatePosition(Vector3 pos)
    {
        transform.localPosition = headPos - pos;
    }

    public void UpdateHeadPosition(Vector3 pos)
    {
        headPos = pos;
    }

    public void SpawnOrbInput(bool isDown)
    {
        isSpawning = isDown;
        orbBody.isKinematic = isDown;
        if (isDown)
        {
            Release();
            orb.transform.SetParent(transform);
            orb.transform.localPosition = Vector3.zero;
            orbBody.velocity = Vector3.zero;
            if (!orb.activeInHierarchy)
                orb.SetActive(true);
        }
        else
        {
            orb.transform.SetParent(null);

            if (themAnchored)
            {
                //full throw, no kickback
                orbBody.velocity = throwVel;
            }
            else
            {
                // split throw, kickback
                orbBody.velocity = throwVel / 2.0f;
                kickbackEvent.Raise(-orbBody.velocity);
            }
        }
    }

    public void GrabInput(bool isDown)
    {
        isTryingToGrab = isDown;
        if (isTryingToGrab)
        {
            TryToGrab();
        }
        else
        {
            Release();
        }
    }

    private void TryToGrab()
    {
        if (isSpawning)
            return;

        if (holdTargets.Count > 0)
        {
            //grab that grabbable
            Hold();
        }
        else if (anchorTargets.Count > 0)
        {
            Anchor();
        }
    }

    private void Release()
    {
        if (heldObject != null)
        {
            //split throw, kickback
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = throwVel / 2.0f;
            kickbackEvent.Raise(-rb.velocity);
            heldObject = null;
        }

        if (anchoredObject != null)
        {
            // full kickback, no throw
            kickbackEvent.Raise(-throwVel);
            anchorEvent.Raise(null);
            anchoredObject = null;
        }
    }

    private void Anchor()
    {
        //anchor self to top of anchorable list
        anchoredObject = anchorTargets[0];

        // disable self physics??
        anchorEvent.Raise(gameObject);

    }

    public void SetThemAnchor(GameObject g)
    {
        themAnchored = g != null;
        if (anchoredObject != null)
        {
            anchoredObject = null;
        }
    }

    private void Hold()
    {
        // attach top of holdable list to hand
        heldObject = holdTargets[0];
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        heldObject.transform.SetParent(transform);
        heldObject.transform.localPosition = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickupable"))
        {
            holdTargets.Add(collision.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Holdable"))
        {
            anchorTargets.Add(collision.gameObject);
        }

        // pre-grab
        if (isTryingToGrab)
        {
            TryToGrab();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickupable"))
        {
            holdTargets.Remove(collision.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Holdable"))
        {
            anchorTargets.Remove(collision.gameObject);
        }
    }

    public void DebugToss(Vector2 f)
    {
        if (f.SqrMagnitude() > 0.0f)
        {
            // spawn orb, toss in direction
            SpawnOrbInput(true);
            throwVel = new Vector3(f.x, 0, f.y) / 4.0f;
            throwVel = transform.parent.rotation * throwVel;
            SpawnOrbInput(false);
        }

    }
}
