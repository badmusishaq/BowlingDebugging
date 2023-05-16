using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce;
    public float playerMoveSpeed = 1;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform throwDirection;
    [SerializeField]
    private List<Rigidbody> bowlingBallPrefabs;
    [SerializeField]
    private FollowTarget followTarget;
    [SerializeField]
    private GameManager gameManager;

    public bool wasBallThrown;

    public void StartAiming()
    {
        animator.SetBool("Aiming", true);
        wasBallThrown = false;
        followTarget.targetTransform = transform;
    }

    private void Update()
    {
        var input = GetAxisInput();
        transform.position += transform.right * (input * playerMoveSpeed * Time.deltaTime);

        TryThrowBall();
    }

    private void TryThrowBall()
    {
        if (wasBallThrown || !Input.GetButtonDown("Fire1")) return;

        wasBallThrown = true;
        var selectedPrefab = bowlingBallPrefabs[Random.Range(0, bowlingBallPrefabs.Count)];

        var newBallRigidbody = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        newBallRigidbody.AddForce(throwDirection.forward * throwForce, ForceMode.Impulse);
        // newBallRigidbody.velocity = throwDirection.forward * throwForce;

        followTarget.targetTransform = newBallRigidbody.transform;
        gameManager.BallThrown(newBallRigidbody.GetComponent<BowlingBall>());

        animator.SetBool("Aiming", false);
    }

    private float GetButtonInput()
    {
        Debug.Log("Moving Using Buttons");

        if (Input.GetButtonDown("Left"))
        {
            return -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            return 1;
        }

        return 0;
    }

    private float GetAxisInput()
    {
        return Input.GetAxis("Horizontal");
    }
}


