using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 cameraPosition;

    private void FixedUpdate()
    {
        if(player.activeSelf)
        {
            transform.position = player.transform.position + cameraPosition;
        }
    }
}
