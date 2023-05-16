using System;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private int framesWithoutMoving;

    public bool DidBallMove()
    {
        var didBallMove = (transform.position - lastPosition).magnitude > 0.0001f ||
               Quaternion.Angle(transform.rotation, lastRotation) > 0.01f;

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        framesWithoutMoving = didBallMove ? 0 : framesWithoutMoving + 1;

        return framesWithoutMoving <= 10;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BowlingTrack"))
            Debug.Log("Bowling ball just hit the track!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pit")) // Ball fell into the pit
        {
            var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.BallKnockedDown();
            Destroy(gameObject);
            return;
        }
    }
}
