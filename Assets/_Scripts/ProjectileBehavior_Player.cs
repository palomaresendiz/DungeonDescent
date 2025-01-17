using UnityEngine;

public class ProjectileBehavior_Player : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public LayerMask whatStopsMovement;
    public Transform movePoint;

    public float maxDistance = 5f; 
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile in the direction it's facing
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);
        // Check if the distance traveled exceeds the threshold
        if (distanceTraveled >= maxDistance)
        {
            // If distance exceeds threshold, destroy the GameObject
            Destroy(gameObject);
        }
    }

    // Function to set the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        // Determine the angle between the current direction and the new direction
        float angle = Vector2.SignedAngle(Vector2.down, newDirection);

        // Rotate the sprite to face the new direction
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Function to handle collisions with other objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Log the layer of the collided object for debugging
        //Debug.Log("Collided object layer: " + LayerMask.LayerToName(collision.gameObject.layer));

        if (LayerMask.LayerToName(collision.gameObject.layer) == "WALL")
        {
            //Debug.Log("COLLISIONS");
            Destroy(gameObject);
        }     
    }
}
