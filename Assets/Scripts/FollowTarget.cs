using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;

    private void Update()
    {
        if (targetTransform == null)
            return;

        transform.position = targetTransform.position + offset;
        transform.LookAt(targetTransform);
    }
}
