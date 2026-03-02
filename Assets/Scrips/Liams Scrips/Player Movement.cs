using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] KeyCode left = KeyCode.LeftArrow;
    [SerializeField] KeyCode right = KeyCode.RightArrow;
    [SerializeField] KeyCode up = KeyCode.UpArrow;
    [SerializeField] KeyCode down = KeyCode.DownArrow;
    [SerializeField] KeyCode dash = KeyCode.Space;

    [SerializeField] float speed = 6f;
    [SerializeField] float dashSpeed = 18f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] float dashCooldownTime = 1f;

    Rigidbody2D rb;
    Vector2 moveInput;
    Vector2 dashDirection;
    bool isDashing = false;
    bool canDash = true;
    float dashTimer = 0.2f;
    float dashCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        moveInput = Vector2.zero;
        if (Input.GetKey(left)) moveInput.x -= 1;
        if (Input.GetKey(right)) moveInput.x += 1;
        if (Input.GetKey(up)) moveInput.y += 1;
        if (Input.GetKey(down)) moveInput.y -= 1;
        moveInput = moveInput.normalized;


        /*
        // Dash input
        if (Input.GetKeyDown(dash) && canDash)
        {
            isDashing = true;
            canDash = false;
            dashTimer = 0f;
            dashDirection = moveInput == Vector2.zero ? transform.up : moveInput;
        }

        // Cooldown timer
        if (!canDash)
        {
            dashCooldown += Time.deltaTime;
            if (dashCooldown >= dashCooldownTime)
            {
                canDash = true;
                dashCooldown = 0f;
            }
        }

        // Rotate to face mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = (mousePos - transform.position).normalized;
        transform.up = aimDir;
        */
    }
    /*
    void FixedUpdate()
    {
        if (isDashing)//dash
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
        }
    }
    */
}
