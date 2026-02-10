using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [Header("Settings")]
    public float attackRange = 2.0f;
    public float attackCooldown = 2.5f;
    private float lastAttackTime;

    [Header("Hitbox")]
    public Transform attackPoint;    // Trascina qui l'oggetto vuoto "AttackPoint"
    public float hitboxRadius = 1.5f; // Raggio del cerchio di attacco
    public int damageDealt = 2;     // Danno inflitto al Player
    public LayerMask playerLayer;    // Seleziona il Layer "Player" nell'Inspector

    [Header("Knockback Settings")]
    public float bossKnockbackForce = 7f;
    public float bossStunTime = 0.3f;

    [Header("Timing")]
    public float windUpTime = 0.5f; // Tempo di "caricamento" prima di far partire l'animazione

    private BossController controller;

    void Start() => controller = GetComponent<BossController>();

    void Update()
    {
        if (controller.currentState == BossState.Dead || controller.isBusy) return;

        if (controller.player != null)
        {
            float distance = Vector2.Distance(transform.position, controller.player.position);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        controller.isBusy = true; // Blocca il movimento
        controller.ChangeState(BossState.Attacking);
    }

    // 1° ANIMATION EVENT: Da chiamare nel frame dell'impatto visivo
    public void DamagePlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, hitboxRadius, playerLayer);

        if (hit != null)
        {
            // 1. Applica il Danno
            if (hit.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.ChangeHealth(-damageDealt);
            }

            // 2. Applica il Knockback (AGGIUNTO QUI)
            if (hit.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
            {
                // Passiamo transform (del Boss), la forza e il tempo di stordimento
                movement.Knockback(transform, bossKnockbackForce, bossStunTime);
            }

            Debug.Log("Il Boss ha colpito e sbalzato il Player!");
        }
    }

    // 2° ANIMATION EVENT: Da chiamare alla fine dell'animazione
    public void EndAttack()
    {
        controller.isBusy = false;
        // Non forziamo lo stato qui, ci pensa l'Update del Controller
    }

    // Disegna il raggio dell'attacco nell'Editor (cerchio blu)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, hitboxRadius);
        }
    }
}