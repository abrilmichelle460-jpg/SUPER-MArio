
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;

    private bool shelled;
    private bool pushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player == null)
                return;

            if (player.starpower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(
                    transform.position.x - other.transform.position.x,
                    0f
                );

                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();

                if (player == null)
                    return;

                if (player.starpower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }
        else if (!shelled &&
                 other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        shelled = true;

        GetComponent<SpriteRenderer>().sprite = shellSprite;

        AnimatedSprite animatedSprite = GetComponent<AnimatedSprite>();
        if (animatedSprite != null)
            animatedSprite.enabled = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        if (movement != null)
            movement.enabled = false;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        EntityMovement movement = GetComponent<EntityMovement>();

        if (movement != null)
        {
            movement.direction = direction.normalized;
            movement.speed = shellSpeed;
            movement.enabled = true;
        }

        int shellLayer = LayerMask.NameToLayer("Shell");

        if (shellLayer >= 0)
            gameObject.layer = shellLayer;
    }

    private void Hit()
    {
        AnimatedSprite animatedSprite = GetComponent<AnimatedSprite>();

        if (animatedSprite != null)
            animatedSprite.enabled = false;

        DeathAnimation deathAnimation = GetComponent<DeathAnimation>();

        if (deathAnimation != null)
            deathAnimation.enabled = true;

        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if (pushed)
            Destroy(gameObject);
    }
}
