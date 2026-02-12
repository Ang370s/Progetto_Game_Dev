using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    public void SpawnPlayer(Vector3 position)
    {
        Instantiate(playerPrefab, position, Quaternion.identity);
    }
}
