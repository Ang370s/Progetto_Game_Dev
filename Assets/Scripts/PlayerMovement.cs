using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 4; // Speed of the player
    public int facingDirection = 1; // 1: right, -1: left

    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    public Animator anim; // Reference to the Animator component

    private bool isKnockedBack; // Flag to check if the player is currently being knocked back

    public Player_Combat player_Combat; // Reference to the Player_Combat script

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is attacking
        if (Input.GetMouseButtonDown(0))
        {
            player_Combat.Attack(); // Call the Attack method in the Player_Combat script
        }
    }

    // Facing Update is called 50x frame
    void FixedUpdate()
    {
        // Check if the player is not currently being knocked back
        if (isKnockedBack == false)
        {
            float horizontal = Input.GetAxis("Horizontal"); // Get horizontal input
            float vertical = Input.GetAxis("Vertical"); // Get vertical input

            // 1. Crea un vettore con i due input
            Vector2 moveInput = new Vector2(horizontal, vertical);

            // 2. Normalizza il vettore se la sua lunghezza supera 1
            // ClampMagnitude � ideale perch� mantiene la sensibilit� del joystick (se lo usi)
            // ma impedisce di superare la velocit� massima in diagonale.
            moveInput = Vector2.ClampMagnitude(moveInput, 1f);

            // Gestione del flip del personaggio
            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            // 3. Applica la velocit� al vettore normalizzato/clamped
            rb.linearVelocity = moveInput * speed;

            //rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);

        }
    }

    // Metodo per capovolgere la direzione del personaggio
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // Metodo per applicare il knockback al giocatore
    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true; // Imposta il flag per indicare che il giocatore è in knockback
        Vector2 direction = (transform.position - enemy.position).normalized; // Calcola la direzione del knockback
        rb.linearVelocity = direction * force; // Applica la forza del knockback al giocatore
        StartCoroutine(KnockbackCounter(stunTime)); // Avvia la coroutine per gestire la durata del knockback
    }

    // Coroutine per gestire la durata del knockback
    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime); // Attendi per 1 secondi
        rb.linearVelocity = Vector2.zero; // Ferma il movimento del giocatore
        isKnockedBack = false; // Resetta il flag per indicare che il giocatore non è più in knockback
    }
}