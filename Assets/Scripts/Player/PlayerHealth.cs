using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * PlayerHealth.cs
 * 
 * Questo script gestisce la salute del giocatore, il feedback di danno e la morte.
 * Assicurati che il GameObject del giocatore abbia un componente Animator con i trigger "Damage" e "Die" configurati.
 * Inoltre, assicurati che il GameObject del giocatore abbia un componente Collider2D e Rigidbody2D per gestire le collisioni e la fisica.
 * 
 * Il metodo ChangeHealth(int amount) viene chiamato per modificare la salute del giocatore. Se l'amount è negativo, si infligge danno; se è positivo, si cura.
 * Dopo ogni modifica della salute, viene aggiornato il display dei cuori tramite il componente HealthHeartBar.
 * Se la salute scende a 0 o meno, viene chiamato il metodo Die() per gestire la morte del giocatore.
 */
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10; // Deve essere un multiplo di 2
    public float currentHealth = 10; // Deve essere un multiplo di 2
    public float invulnerableTime = 0.5f; // Tempo di invulnerabilità dopo essere stati colpiti

    private bool isInvulnerable = false; // Flag per controllare lo stato di invulnerabilit�
    private bool isDead = false; // Flag per controllare se il giocatore � morto


    // Trascina l'oggetto con lo script HealthHeartBar qui nell'Inspector
    public HealthHeartBar heartBar;

    void Start()
    {
        if (heartBar == null)
            heartBar = FindObjectOfType<HealthHeartBar>();

        // Se esiste una vita salvata → ripristina
        if (PlayerStats.Instance != null && PlayerStats.Instance.savedHealth > 0)
        {
            currentHealth = PlayerStats.Instance.savedHealth;
        }

        if (heartBar != null)
            heartBar.DrawHearts();
    }

    // permette di aggiornare i cuori ogni volta che il giocatore viene abilitato (ad esempio dopo un respawn)
    void OnEnable()
    {
        if (heartBar == null)
            heartBar = FindObjectOfType<HealthHeartBar>();

        if (heartBar != null)
            heartBar.DrawHearts();
    }

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
            SFXManager.Instance.PlaySFX(SFXManager.Instance.damage);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.heal);
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

        if (isDead) return; // Evita di chiamare più volte la morte
        isDead = true;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.playerDied = true;
            PlayerStats.Instance.SaveTimeFromTimer();
        }

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        GetComponent<PlayerMovement>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        GetComponent<Collider2D>().enabled = false;

        Invoke(nameof(LoadGameOver), 1.5f);
    }

    void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

}