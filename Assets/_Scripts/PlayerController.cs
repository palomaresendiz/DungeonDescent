using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public float smoothnessFactor = 100f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public Vector3 directionFacing = Vector3.zero;
    public bool isJumping = false;

    Animator animator; 
    public VisualEffect vfx;

    public Collider2D leftHitbox;
    public Collider2D rightHitbox;
    public Collider2D upHitbox;
    public Collider2D downHitbox;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;

        animator = GetComponent<Animator>();

        DisableAllHitboxes();
    }

    // Update is called once per frame
    void Update()
    {
//        vfx.SetVector3("CollisionPos", transform.position + new Vector3(13f, 0f, 0f));
        //vfx.SetVector3("CollisionPos", Vector3.Lerp(vfx.GetVector3("CollisionPos"), transform.position, Time.deltaTime * smoothnessFactor));
        bool isInverted = PlayerStatus.isInverted;

        if (!isInverted)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
                }

                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    Move(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    Move(new Vector3(-Input.GetAxisRaw("Horizontal"), 0f, 0f));
                }

                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    Move(new Vector3(0f, -Input.GetAxisRaw("Vertical"), 0f));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
        }

    if (Input.GetMouseButtonDown(0)) // Left-click for attack
    {
        TriggerAttack();
    }
    if (Input.GetMouseButtonDown(1)) // Right-click for attack
    {
        ShootArrow();
    }
    }

    private void Move(Vector3 direction)
    {
        if (!Physics2D.OverlapCircle(movePoint.position + direction, .2f, whatStopsMovement))
        {
            movePoint.position += direction;
            directionFacing = direction;

            
            if (animator != null)
            {
                animator.SetFloat("DirectionX", direction.x);
                animator.SetFloat("DirectionY", direction.y);
            }
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;

        Vector3 jumpDestination = movePoint.position + 2 * directionFacing;

        RaycastHit2D hit = Physics2D.Raycast(movePoint.position, directionFacing, 1f, whatStopsMovement);
        if (hit.collider == null)
        {
            float scaleDuration = 0.1f;
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = originalScale * 1.3f;

            float elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
            movePoint.position = jumpDestination;

            elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = originalScale;
        }

        isJumping = false;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public Direction GetFacingDirection(float directionX, float directionY)
    {
        if (directionX < 0) return Direction.Left;
        if (directionX > 0) return Direction.Right;
        if (directionY < 0) return Direction.Down;
        if (directionY > 0) return Direction.Up;

        return Direction.Down; // Default or fallback direction
    }

    public void DisableAllHitboxes()
    {
        leftHitbox.enabled = false;
        rightHitbox.enabled = false;
        upHitbox.enabled = false;
        downHitbox.enabled = false;
    }

    public void EnableHitbox(Direction direction)
    {
        DisableAllHitboxes(); // Ensure only one hitbox is enabled at a time

        switch (direction)
        {
            case Direction.Left:
                leftHitbox.enabled = true;
                break;
            case Direction.Right:
                rightHitbox.enabled = true;
                break;
            case Direction.Up:
                upHitbox.enabled = true;
                break;
            case Direction.Down:
                downHitbox.enabled = true;
                break;
        }
    }

    public float hitboxDuration = 0.2f; // Duration to keep hitbox enabled (adjust as needed)

    public void TriggerAttack()
    {
        if (animator != null)
        {
            // Determine the direction the player is facing
            float directionX = animator.GetFloat("DirectionX");
            float directionY = animator.GetFloat("DirectionY");

            Direction facingDirection = GetFacingDirection(directionX, directionY);
            audioManager.PlaySFX(audioManager.Swing);
            // Set the attack direction in the Animator
            int attackDirectionValue = 0;

            switch (facingDirection)
            {
                case Direction.Left:
                    attackDirectionValue = 0;
                    break;
                case Direction.Right:
                    attackDirectionValue = 1;
                    break;
                case Direction.Up:
                    attackDirectionValue = 2;
                    break;
                case Direction.Down:
                    attackDirectionValue = 3;
                    break;
            }

            animator.SetInteger("AttackDirection", attackDirectionValue); // Set direction
            animator.SetTrigger("Attack"); // Trigger the attack animation

            // Enable hitbox and start coroutine to disable after duration
            EnableHitbox(facingDirection);
            StartCoroutine(DisableHitboxAfterDelay(0.2f)); // Adjust duration as needed
        }
    }

    public IEnumerator DisableHitboxAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        DisableAllHitboxes(); // Disable all hitboxes
    }

public GameObject arrowPrefab; // Reference to the arrow prefab
public Transform arrowSpawnPoint; // Point from where the arrow will be spawned
public float arrowCooldown = 1.0f; // Set cooldown period in seconds
private float lastArrowTime = 0f;  // Time when the last arrow was shot

public void ShootArrow()
{
        // Check if cooldown period has passed
    if (Time.time - lastArrowTime >= arrowCooldown)
    {
        audioManager.PlaySFX(audioManager.Arrow);

    // Instantiate the arrow prefab at the spawn point
    GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

    // Get the direction the player is facing
    float directionX = animator.GetFloat("DirectionX");
    float directionY = animator.GetFloat("DirectionY");
    Direction facingDirection = GetFacingDirection(directionX, directionY);

    // Adjust direction based on correct facing orientation
    switch (facingDirection)
    {
        case Direction.Left:
            arrow.GetComponent<ProjectileBehavior_Player>().SetDirection(Vector2.right); // Corrected direction
            break;
        case Direction.Right:
            arrow.GetComponent<ProjectileBehavior_Player>().SetDirection(Vector2.left); // Corrected direction
            break;
        case Direction.Up:
            arrow.GetComponent<ProjectileBehavior_Player>().SetDirection(Vector2.down); // Corrected direction
            break;
        case Direction.Down:
            arrow.GetComponent<ProjectileBehavior_Player>().SetDirection(Vector2.up); // Corrected direction
            break;
    }
            // Update the last arrow time to the current time
        lastArrowTime = Time.time;
    }
    else
    {
        // Feedback or log message indicating cooldown is active
        Debug.Log("Cannot shoot yet, cooldown active.");
    }
}
    void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
}
