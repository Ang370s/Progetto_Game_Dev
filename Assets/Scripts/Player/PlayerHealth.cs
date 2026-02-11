using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10; // Deve essere un multiplo di 2
    public float currentHealth = 10; // Deve essere un multiplo di 2
    public float invulnerableTime = 0.5f; // Tempo di invulnerabilità dopo essere stati colpiti

    private bool isInvulnerable = false; // Flag per controllare lo stato di invulnerabilit�
    private bool isDead = false; // Flag per controllare se il giocatore � morto


    // Trascina l'oggetto con lo script HealthHeartBar qui nell'Inspector
    public HealthHeartBar heartBar;

    // Metodo per cambiare la salute del giocatore
    public void ChangeHealth(int amount)
    {
        if (isInvulnerable && amount < 0) return; // Se il giocatore è invulnerabile e si sta cercando di infliggere danno, non fare nulla

        currentHealth += amount; // Modifica la salute attuale
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limita la salute tra 0 e il massimo

        // AGGIORNAMENTO: Chiama il ridisegno dei cuori
        if (heartBar != null)
        {
            heartBar.DrawHearts();
        }

        // Controlla se la salute � scesa a 0 o meno
        if (currentHealth <= 0)
        {
            Die(); // Chiama il metodo per gestire la morte del giocatore
            return;
        }

        // Avvia il feedback di danno solo se l'ammontare � negativo (danno subito)
        if (amount < 0)
        {
            StartCoroutine(DamageFeedback()); // Avvia la coroutine per il feedback di danno
        }
    }

    // Coroutine per gestire il feedback di danno e l'invulnerabilit� temporanea
    IEnumerator DamageFeedback()
    {
        isInvulnerable = true;

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Damage");

        yield return new WaitForSeconds(invulnerableTime); // Attendi per il tempo di invulnerabilit�

        isInvulnerable = false;
    }

    // Metodo per gestire la morte del giocatore
    void Die()
    {

        if (isDead) return; // Evita di chiamare pi� volte la morte
        isDead = true;

        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.StopTimer();

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        GetComponent<PlayerMovement>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        GetComponent<Collider2D>().enabled = false;
    }
}