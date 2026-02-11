using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component of the arrow
    public Vector2 direction = Vector2.right; // Direction in which the arrow will move
    //public float lifeSpawn = 2.0f; // Time in seconds after which the arrow will be destroyed (2 seconds)
    public float speed = 10f; // Speed at which the arrow will move

    public LayerMask enemyLayer; // Layer mask to identify enemies
    public LayerMask obstacleLayer; // Layer mask to identify obstacles
    public LayerMask chestLayer; // Layer mask to identify chests

    public SpriteRenderer sr; // Reference to the SpriteRenderer component of the arrow
    public Sprite buriedSprite; // Sprite to be used when the arrow is buried in an obstacle

    public int damage = 1; // Damage dealt by the arrow
    public float knockbackForce = 5f; // Force applied for knockback
    public float knockbackTime = 0.15f; // Duration of the knockback effect
    public float stunTime = 0.3f; // Duration of the stun effect after knockback

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = direction * speed; // Set the velocity of the arrow based on the direction and speed
        RotateArrow(); // Call the method to rotate the arrow based on its direction
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in degrees based on the direction vector
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotate the arrow to face the direction of movement
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_Health>()
                .ChangeHealth(-damage);

            collision.gameObject.GetComponent<Enemy_Knockback>()
                .Knockback(transform, knockbackForce, knockbackTime, stunTime);

            StickAndDisappear(collision.transform);
        }
        else if ((chestLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Chest>()?.TakeDamage(damage);
            StickAndDisappear(collision.transform);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            AttachToTarget(collision.transform);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Identifica se l'oggetto è nel Layer Enemy
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Prova a prendere la salute del nemico comune
            Enemy_Health enemy = collision.gameObject.GetComponent<Enemy_Health>();

            if (enemy != null)
            {
                // CASO NEMICO COMUNE
                enemy.ChangeHealth(-damage);

                // Il knockback lo cerchiamo SOLO qui dentro
                if (collision.gameObject.TryGetComponent<Enemy_Knockback>(out Enemy_Knockback kb))
                {
                    kb.Knockback(transform, knockbackForce, knockbackTime, stunTime);
                }
            }
            else if (collision.gameObject.TryGetComponent<BossHealth>(out BossHealth boss))
            {
                // CASO BOSS
                boss.TakeDamage(damage);
                // Il boss non ha knockback
            }

            StickAndDisappear(collision.transform);
        }
        // 2. Altri Layer (Casse, Ostacoli...)
        else if ((chestLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Chest>()?.TakeDamage(damage);
            StickAndDisappear(collision.transform);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            AttachToTarget(collision.transform);
        }
    }


    void StickAndDisappear(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        GetComponent<Collider2D>().enabled = false;

        transform.SetParent(target);

        StartCoroutine(FadeAndDestroy(0.5f, 0.4f));
    }

    private void AttachToTarget(Transform target)
    {
        if (sr != null && buriedSprite != null)
            sr.sprite = buriedSprite;

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        GetComponent<Collider2D>().enabled = false;

        transform.SetParent(target);

        // 1 secondo ferma, poi fade 3 secondi
        StartCoroutine(FadeAndDestroy(1f, 3f));
    }


    IEnumerator FadeAndDestroy(float delay, float fadeTime)
    {
        yield return new WaitForSeconds(delay);

        float t = 0f;
        Color c = sr.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            sr.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }

}
