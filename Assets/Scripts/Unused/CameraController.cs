using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TO-DO: MAKE THE CAMERA BETTER
    // currently it just looks at the player but it doesn't actually follow the player and kinda sucks

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public Vector3 cameraOffset;
    public float maxDistance;
    public Vector3 distance;
    public float distanceSqrd;
    public float speed;
    // Update is called once per frame
    void LateUpdate()
    {
        // Vector3 nextPos = player.position + cameraOffset;
        // transform.position = Vector3.Lerp(transform.position, nextPos, 0.25f);
        distance = player.transform.position - transform.position;
        distanceSqrd = distance.sqrMagnitude;

        if (distance.sqrMagnitude > maxDistance)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position + cameraOffset, step);
        }

        transform.LookAt(player);
    }
}
