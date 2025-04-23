using UnityEngine;

public class BouncingObject : MonoBehaviour
{
    public float amplitude = 1f;  // Height of the bounce
    public float frequency = 1f;  // Speed of the bounce

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
