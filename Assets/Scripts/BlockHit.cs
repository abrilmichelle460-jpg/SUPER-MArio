using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1;
    private bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (HitFromBelow(collision)) {
                Hit(collision.gameObject.GetComponent<Player>());
            }
        }
    }

    private bool HitFromBelow(Collision2D collision)
    {
        Collider2D block = GetComponent<Collider2D>();
        if (collision.collider.bounds.center.y >= block.bounds.center.y) return false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);
            if (Mathf.Abs(contact.normal.y) > .5f && contact.point.y <= block.bounds.min.y + .15f)
                return true;
        }
        return false;
    }

    private void Hit(Player player)
    {
        // Empty, unlimited-hit bricks break for big Mario. Item blocks retain their existing behavior.
        if (maxHits < 0 && item == null && player != null && player.big)
        {
            Break();
            return;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        if (maxHits > 0) maxHits--;

        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
        }

        if (item != null) {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private void Break()
    {
        animating = true;
        SpriteRenderer view = GetComponent<SpriteRenderer>();
        Sprite source = view.sprite;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                Rect rect = source.rect;
                rect = new Rect(rect.x + x * rect.width / 2, rect.y + y * rect.height / 2, rect.width / 2, rect.height / 2);
                Sprite fragment = Sprite.Create(source.texture, rect, new Vector2(.5f, .5f), source.pixelsPerUnit);
                GameObject debris = new GameObject("Brick fragment");
                debris.transform.position = transform.position + new Vector3((x - .5f) * .5f, (y - .5f) * .5f);
                SpriteRenderer renderer = debris.AddComponent<SpriteRenderer>();
                renderer.sprite = fragment;
                renderer.sortingLayerID = view.sortingLayerID;
                renderer.sortingOrder = view.sortingOrder + 1;
                Rigidbody2D body = debris.AddComponent<Rigidbody2D>();
                body.gravityScale = 1.5f;
                body.linearVelocity = new Vector2(x == 0 ? -3f : 3f, y == 0 ? 4f : 6f);
                body.angularVelocity = x == 0 ? 120f : -120f;
                Destroy(debris, 2f);
                Destroy(fragment, 2f);
            }
        }
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

}
