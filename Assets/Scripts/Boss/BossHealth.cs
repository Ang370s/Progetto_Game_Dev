using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
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

            // Se non usi Animation Events per il danno, questo Invoke va bene
            CancelInvoke("EndDamage"); // Resetta eventuali invoke precedenti
            Invoke("EndDamage", 0.3f);
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void EndDamage()
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
        GetComponent<Collider2D>().enabled = false;

        // Comunica la kill alle statistiche (opzionale)
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddKill(transform.position);
        }

        Debug.Log("Il Boss è stato sconfitto!");
    }
}
