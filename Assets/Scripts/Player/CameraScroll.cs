using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Transform Player;
    public float timeOffset;

    public Vector3 offset;
    public Vector3 boundsMin;
    public Vector3 boundsMax;
    private void LateUpdate()
    {
        if (Player != null)
        {
            Vector3 start = transform.position;
            Vector3 target = Player.position;

            target.x += offset.x;
            target.y += offset.y;
            target.z = transform.position.z;

            target.x = Mathf.Clamp(target.x, boundsMin.x, boundsMax.x); //keeps camera within boundaries of boundsMin and boundsMax's x value
            target.y = Mathf.Clamp(target.y, boundsMin.y, boundsMax.y); //same as above but for y

            float t = 1f - Mathf.Pow(1f - timeOffset, Time.deltaTime * 60); //easing camera at 60fps
            transform.position = Vector3.Lerp(start, target, t);
        }
    }
}
