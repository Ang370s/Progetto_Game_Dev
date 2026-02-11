using UnityEngine;

public enum BossState
{
    Idle,
    Walking,
    Taunting,
    Attacking,
    Damaged,
    Dead
}

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.8f;
    public float playerDetectRange = 10f;
    public LayerMask playerLayer;
    public Transform detectionPoint;

    [HideInInspector] public BossState currentState;
    [HideInInspector] public Transform player;
    [HideInInspector] public bool isBusy = false;

    private Rigidbody2D rb;
    private Animator anim;
    private int facingDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(BossState.Idle);
    }

    void Update()
    {
        if (currentState == BossState.Dead) return;

        // Se è impegnato (attacco/danno), forziamo la velocità a zero per non farlo scivolare
        if (isBusy)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        DetectPlayer();

        if (player == null)
        {
            StopBoss();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopBoss();
        }
    }

    void MoveTowardsPlayer()
    {
        ChangeState(BossState.Walking);
        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0 && facingDirection == -1 || direction.x < 0 && facingDirection == 1)
            Flip();

        rb.linearVelocity = direction * moveSpeed;
    }

    void StopBoss()
    {
        rb.linearVelocity = Vector2.zero;
        ChangeState(BossState.Idle);
    }

    public void ChangeState(BossState newState)
    {
        // Se è già in questo stato, non fare nulla (evita che le animazioni resettino)
        if (currentState == newState) return;

        currentState = newState;

        // Reset di tutti i trigger di movimento per evitare conflitti
        anim.ResetTrigger("isIdle");
        anim.ResetTrigger("isWalking");
        anim.ResetTrigger("isAttacking");
        anim.ResetTrigger("isDamaged");

        switch (newState)
        {
            case BossState.Idle:
                anim.SetTrigger("isIdle");
                break;
            case BossState.Walking:
                anim.SetTrigger("isWalking");
                break;
            case BossState.Attacking:
                anim.SetTrigger("isAttacking");
                break;
            case BossState.Damaged:
                anim.SetTrigger("isDamaged");
                break;
            case BossState.Dead:
                anim.SetTrigger("isDead");
                break;
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(detectionPoint.position, playerDetectRange, playerLayer);
        player = hit != null ? hit.transform : null;
    }

    void Flip()
    {
        facingDirection *= -1;

        // Creiamo un nuovo vettore partendo dalla scala attuale
        Vector3 newScale = transform.localScale;

        // Cambiamo SOLO la X moltiplicandola per -1
        newScale.x *= -1;

        // Riapplichiamo la scala senza toccare Y e Z
        transform.localScale = newScale;
    }
}
