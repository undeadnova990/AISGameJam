using UnityEngine;

public class ElevatorBehavior : MonoBehaviour
{
    public Vector3 initialPosition;
    public float speed = 5.0f;
    public float distance = 3.0f;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float movementY = initialPosition.y + Mathf.PingPong(Time.deltaTime * speed, distance);

        transform.position = new Vector3(initialPosition.x, movementY, initialPosition.z);
    }
}
