using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float elapsedTime = 0f;
    public bool isRunning = true;

    void Awake()
    {
        if (FindObjectsOfType<GameTimer>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}
