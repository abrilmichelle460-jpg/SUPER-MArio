using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Vector2 lower;
    public Vector2 upper;
    public float speed = 1.6f;
    private Rigidbody2D body;
    private bool ascending = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        Vector2 target = ascending ? upper : lower;
        body.MovePosition(Vector2.MoveTowards(body.position, target, speed * Time.fixedDeltaTime));
        if (Vector2.Distance(body.position, target) < .05f) ascending = !ascending;
    }
}
