
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    public float jumpForce =>
        (2f * maxJumpHeight) / (maxJumpTime / 2f);

    public float gravity =>
        (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }

    // Activado por LevelRuntime cuando Mario entra en el agua.
    public bool swimming { get; set; }

    public bool running =>
        Mathf.Abs(velocity.x) > 0.25f ||
        Mathf.Abs(inputAxis) > 0.25f;

    public bool sliding =>
        (inputAxis > 0f && velocity.x < 0f) ||
        (inputAxis < 0f && velocity.x > 0f);

    public bool falling =>
        velocity.y < 0f && !grounded;

    [Header("Movimiento bajo el agua")]
    public float swimUpSpeed = 4f;
    public float sinkSpeed = 3f;

    private void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        if (collider != null)
            collider.enabled = true;

        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        if (rigidbody != null)
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

        if (collider != null)
            collider.enabled = false;

        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rigidbody.Raycast(Vector2.down);

        if (swimming)
        {
            SwimmingMovement();
        }
        else
        {
            if (grounded)
            {
                GroundedMovement();
            }

            ApplyGravity();
        }
    }

    private void FixedUpdate()
    {
        if (camera == null)
            camera = Camera.main;

        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // Limitar el movimiento a la pantalla si existe una cámara.
        if (camera != null)
        {
            Vector2 leftEdge =
                camera.ScreenToWorldPoint(Vector2.zero);

            Vector2 rightEdge =
                camera.ScreenToWorldPoint(
                    new Vector2(Screen.width, Screen.height)
                );

            position.x = Mathf.Clamp(
                position.x,
                leftEdge.x + 0.5f,
                rightEdge.x - 0.5f
            );
        }

        rigidbody.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");

        velocity.x = Mathf.MoveTowards(
            velocity.x,
            inputAxis * moveSpeed,
            moveSpeed * Time.deltaTime
        );

        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // Evitar que la velocidad de caída siga acumulándose.
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void SwimmingMovement()
    {
        grounded = false;

        if (Input.GetButton("Jump"))
        {
            velocity.y = Mathf.MoveTowards(
                velocity.y,
                swimUpSpeed,
                swimUpSpeed * Time.deltaTime
            );
        }
        else
        {
            velocity.y = Mathf.MoveTowards(
                velocity.y,
                -sinkSpeed,
                sinkSpeed * Time.deltaTime
            );
        }

        velocity.y = Mathf.Clamp(
            velocity.y,
            -sinkSpeed,
            swimUpSpeed
        );

        jumping = velocity.y > 0.1f;
    }

    private void ApplyGravity()
    {
        bool isFalling =
            velocity.y < 0f ||
            !Input.GetButton("Jump");

        float multiplier = isFalling ? 2f : 1f;

        velocity.y +=
            gravity * multiplier * Time.deltaTime;

        velocity.y = Mathf.Max(
            velocity.y,
            gravity / 2f
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer ==
            LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(
                collision.transform,
                Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer !=
                 LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(
                collision.transform,
                Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}
