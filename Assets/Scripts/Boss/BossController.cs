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

/*public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.8f;

    [Header("Detection")]
    public float playerDetectRange = 10f;
    public LayerMask playerLayer;
    public Transform detectionPoint;

    [Header("Combat")]
    public float attackCooldown = 2f;
    private float lastAttackTime;

    private bool isBusy = false;

    private BossState currentState;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private int facingDirection = 1; // 1 = right, -1 = left

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ChangeState(BossState.Idle);
    }

    void Update()
    {*/

/*if (Input.GetKeyDown(KeyCode.Alpha1))
    ChangeState(BossState.Idle);

if (Input.GetKeyDown(KeyCode.Alpha2))
    ChangeState(BossState.Walking);

if (Input.GetKeyDown(KeyCode.Alpha3))
    ChangeState(BossState.Taunting);

if (Input.GetKeyDown(KeyCode.Alpha4))
    ChangeState(BossState.Attacking);

if (Input.GetKeyDown(KeyCode.Alpha5))
    ChangeState(BossState.Damaged);

if (Input.GetKeyDown(KeyCode.Alpha6))
    ChangeState(BossState.Dead);

if (currentState == BossState.Dead)
    return;*/

/*if (currentState == BossState.Dead)
    return;

if (isBusy)
    return;

DetectPlayer();

if (player == null)
{
    rb.linearVelocity = Vector2.zero;
    ChangeState(BossState.Idle);
    return;
}

float distance = Vector2.Distance(transform.position, player.position);

if (distance > stopDistance)
{
    ChangeState(BossState.Walking);
    MoveTowardsPlayer();
}
else
{
    rb.linearVelocity = Vector2.zero;
    if (Time.time >= lastAttackTime + attackCooldown)
    {
        StartAttack();
    }
    else
    {
        ChangeState(BossState.Idle);
    }
}
}

void StartAttack()
{
lastAttackTime = Time.time;
PlayOneShot(BossState.Attacking); // Usa il tuo metodo OneShot per bloccare il movimento
}

public void PlayOneShot(BossState state)
{
isBusy = true;
ChangeState(state);
}

public void EndOneShot()
{
isBusy = false;
ChangeState(BossState.Idle);
}


void DetectPlayer()
{
Collider2D hit = Physics2D.OverlapCircle(detectionPoint.position, playerDetectRange, playerLayer);
if (hit != null)
    player = hit.transform;
else
    player = null;
}

void MoveTowardsPlayer()
{
Vector2 direction = (player.position - transform.position).normalized;

// Flip
if (direction.x > 0 && facingDirection == -1 ||
    direction.x < 0 && facingDirection == 1)
{
    Flip();
}

rb.linearVelocity = direction * moveSpeed;
}

void Flip()
{
facingDirection *= -1;
transform.localScale = new Vector3(
    transform.localScale.x * -1,
    transform.localScale.y,
    transform.localScale.z
);
}

public void ChangeState(BossState newState)
{
if (currentState == newState)
    return;

// se stiamo facendo una one-shot, non permettere override
if (isBusy && newState == BossState.Idle)
    return;

currentState = newState;
anim.SetInteger("state", (int)currentState);
}

private void OnDrawGizmosSelected()
{
if (detectionPoint != null)
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
}
}
}*/

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
        if (currentState == BossState.Dead || isBusy) return;

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
