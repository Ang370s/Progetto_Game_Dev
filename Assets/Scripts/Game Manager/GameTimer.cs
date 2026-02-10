using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float elapsedTime = 0f;
    public bool isRunning = true;

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
