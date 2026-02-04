using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 4;
    public int facingDirection = 1; // 1: right, -1: left

    public Rigidbody2D rb;
    public Animator anim;

    // Facing Update is called 50x frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 1. Crea un vettore con i due input
        Vector2 moveInput = new Vector2(horizontal, vertical);

        // 2. Normalizza il vettore se la sua lunghezza supera 1
        // ClampMagnitude è ideale perché mantiene la sensibilità del joystick (se lo usi)
        // ma impedisce di superare la velocità massima in diagonale.
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        // 3. Applica la velocità al vettore normalizzato/clamped
        rb.linearVelocity = moveInput * speed;

        //rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}