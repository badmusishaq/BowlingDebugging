using System;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    public bool DidPinFall { get; private set; }

    private int framesWithoutMoving;
    private Rigidbody rb;

    public bool DidPinMove()
    {
        var didPinMove = (transform.position - lastPosition).magnitude > 0.001f ||
                         Quaternion.Angle(transform.rotation,lastRotation) > 0.01f;

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        framesWithoutMoving = didPinMove ? 0 : framesWithoutMoving + 1;

        return framesWithoutMoving <= 10;
    }

    public void ResetPosition()
    {
        rb.position = startingPosition;
        rb.rotation = startingRotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        lastPosition = startingPosition;
        lastRotation = startingRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pit")) // Pin fell into the pit
        {
            var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.PinKnockedDown();
            Destroy(gameObject);
            return;
        }

        else if (other.CompareTag("BowlingTrack")) // Pin Head trigger hit the floor
        {
            DidPinFall = true;
            return;
        }
    }

    private void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
    }
}