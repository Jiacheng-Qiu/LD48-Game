using UnityEngine;

public class RockBehind : MonoBehaviour
{
    public float maxDist = 15f;
    public float speed = 12f;
    private float angle;
    public GameObject player;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        angle = Mathf.Abs(transform.localRotation.eulerAngles.z - 360) * Mathf.Deg2Rad;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > maxDist)
        {
            transform.position = player.transform.position + new Vector3(-maxDist * Mathf.Cos(angle), 0.5f + maxDist * Mathf.Sin(angle), 0);
        }
        if (Time.time - startTime > 10)
        {
            speed++;
            startTime = Time.time;
        }
        transform.position += new Vector3(speed * Time.deltaTime * Mathf.Cos(angle), -speed * Time.deltaTime * Mathf.Sin(angle), 0);
    }
}
