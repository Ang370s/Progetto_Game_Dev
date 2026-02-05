using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab del cuore
    public PlayerHealth playerHealth; // Riferimento allo script della salute del giocatore
    List<HealthHeart> hearts = new List<HealthHeart>(); // Lista dei cuori attuali

    // Start is called before the first frame update
    private void Start()
    {
        // Calcoliamo quanti cuori servono (es. 6 hp / 2 = 3 cuori)
        int totalHearts = Mathf.CeilToInt(playerHealth.maxHealth / 2f);

        // Chiamiamo il metodo passando il numero calcolato
        CreateEmptyHearts(totalHearts);

        // Ora che i cuori esistono, li coloriamo in base alla vita attuale
        DrawHearts();
    }

    // Update is called once per frame
    public void DrawHearts()
    {
        // Se non ci sono cuori, non facciamo nulla
        if (hearts.Count == 0) return;

        float healthPerHeart = playerHealth.maxHealth / hearts.Count; // Es. 6 hp / 3 cuori = 2 hp per cuore
        float currentHealth = playerHealth.currentHealth; // Salute attuale del giocatore

        // Cicliamo tutti i cuori e li coloriamo in base alla salute attuale
        foreach (HealthHeart heart in hearts)
        {
            if(currentHealth >= healthPerHeart)
            {
                heart.SetHeartImage(HeartStaus.Full);
            }
            else if(currentHealth >= healthPerHeart / 2)
            {
                heart.SetHeartImage(HeartStaus.Half);
            }
            else
            {
                heart.SetHeartImage(HeartStaus.Empty);
            }
            currentHealth -= healthPerHeart;
        }
    }

    public void CreateEmptyHearts(int numberOfHearts)
    {
        // Prima puliamo i cuori vecchi se ce ne sono
        ClearHearts();

        // Creiamo i nuovi cuori vuoti
        for (int i = 0; i < numberOfHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(transform);

            // Reset della scala (a volte Unity li crea giganti)
            newHeart.transform.localScale = Vector3.one;

            HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
            heartComponent.SetHeartImage(HeartStaus.Empty);

            // Li aggiungiamo alla lista così DrawHearts() può trovarli
            hearts.Add(heartComponent);
        }
    }

    // Rimuove tutti i cuori esistenti
    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
}
