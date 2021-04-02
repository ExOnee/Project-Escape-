using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        transform.position = Target.position;
    }
}

