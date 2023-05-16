using UnityEngine;

public class ChangeCamTarget : MonoBehaviour
{
    public FollowTarget followTarget;
    public Transform newTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
            followTarget.targetTransform = newTarget;
    }
}
