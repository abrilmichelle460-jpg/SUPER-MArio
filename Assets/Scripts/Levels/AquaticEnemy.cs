using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class AquaticEnemy : MonoBehaviour
{
    public bool squid;
    public Sprite[] frames;
    public float range = 3f;
    public float speed = 1.2f;
    private Rigidbody2D body;
    private SpriteRenderer view;
    private Vector2 origin;
    private float phase;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        view = GetComponent<SpriteRenderer>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0;
        origin = body.position;
        phase = origin.x * .71f;
    }

    private void FixedUpdate()
    {
        float t = Time.time * speed + phase;
        Vector2 target = origin + new Vector2(Mathf.Sin(t * .55f) * range, Mathf.Sin(t) * (squid ? 1.25f : .45f));
        view.flipX = target.x > body.position.x;
        body.MovePosition(target);
        if (frames != null && frames.Length > 0) view.sprite = frames[(int)(Time.time * 3) % frames.Length];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Player player = other.GetComponent<Player>();
        if (player.starpower) Destroy(gameObject);
        else player.Hit();
    }
}
