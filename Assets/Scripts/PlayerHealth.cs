using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 6; // Deve essere un multiplo di 2
    public float currentHealth = 6; // Deve essere un multiplo di 2
    public float invulnerableTime = 0.3f; // Tempo di invulnerabilità dopo essere stati colpiti

    private bool isInvulnerable = false; // Flag per controllare lo stato di invulnerabilità
    private bool isDead = false; // Flag per controllare se il giocatore è morto


    // Trascina l'oggetto con lo script HealthHeartBar qui nell'Inspector
    public HealthHeartBar heartBar;

    // Metodo per cambiare la salute del giocatore
    public void ChangeHealth(int amount)
    {
        if (isInvulnerable) return;

        currentHealth += amount; // Modifica la salute attuale
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limita la salute tra 0 e il massimo

        // AGGIORNAMENTO: Chiama il ridisegno dei cuori
        if (heartBar != null)
        {
            heartBar.DrawHearts();
        }

        // Controlla se la salute è scesa a 0 o meno
        if (currentHealth <= 0)
        {
            Die(); // Chiama il metodo per gestire la morte del giocatore
            return;
            //gameObject.SetActive(false); // Disattiva il giocatore
            //GetComponent<PlayerMovement>().enabled = false; // Ferma il movimento
            //GetComponent<Collider2D>().enabled = false;    // Evita altri colpi
            //GetComponent<SpriteRenderer>().color = Color.gray; // Feedback visivo
        }

        // Avvia il feedback di danno solo se l'ammontare è negativo (danno subito)
        if (amount < 0)
        {
            StartCoroutine(DamageFeedback()); // Avvia la coroutine per il feedback di danno
        }
    }

    // Coroutine per gestire il feedback di danno e l'invulnerabilità temporanea
    IEnumerator DamageFeedback()
    {
        isInvulnerable = true;

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Damage");

        yield return new WaitForSeconds(invulnerableTime); // Attendi per il tempo di invulnerabilità

        isInvulnerable = false;
    }

    // Metodo per gestire la morte del giocatore
    void Die()
    {

        if (isDead) return; // Evita di chiamare più volte la morte
        isDead = true;

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        GetComponent<PlayerMovement>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        GetComponent<Collider2D>().enabled = false;

        /*
        // 1. Attiva l'animazione
        GetComponent<Animator>().SetTrigger("Die");

        // 2. Disabilita il movimento per non far scivolare il cadavere
        GetComponent<PlayerMovement>().enabled = false;

        // 3. Opzionale: ferma il corpo fisicamente
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Così non cade e non sposta altri oggetti

        // 4. Disabilita il collider così i nemici lo ignorano
        GetComponent<Collider2D>().enabled = false;
        */
    }
}