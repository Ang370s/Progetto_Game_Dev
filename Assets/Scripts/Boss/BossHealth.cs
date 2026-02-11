using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    private BossController controller;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<BossController>();
    }

    public void TakeDamage(int damage)
    {
        if (controller.currentState == BossState.Dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Blocca il movimento fisico durante il danno
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            controller.isBusy = true;
            controller.ChangeState(BossState.Damaged);

            // Feedback visivo immediato
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Color.red;
            Invoke("ResetColor", 0.1f);
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Questa funzione verrà chiamata dall'Animation Event
    public void OnDamageAnimationEnd()
    {
        if (controller.currentState != BossState.Dead)
        {
            controller.isBusy = false;
            controller.ChangeState(BossState.Idle);
        }
    }

    void Die()
    {
        if (controller.currentState == BossState.Dead) return;

        controller.isBusy = true;
        controller.ChangeState(BossState.Dead);

        // 1. Disabilita il collider così le nuove frecce passano attraverso
        GetComponent<Collider2D>().enabled = false;

        // 2. FERMA LA FISICA: Impedisce che venga trascinato o spostato
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static; // Diventa un oggetto fisso, la freccia non lo sposta più
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddKill(transform.position);
        }

        Debug.Log("Il Boss è stato sconfitto!");
    }
}
