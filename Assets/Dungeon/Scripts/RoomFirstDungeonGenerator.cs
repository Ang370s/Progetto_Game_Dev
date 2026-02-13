using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int minRoomWidth = 10, minRoomHeight = 10;
    [SerializeField] private int dungeonWidth = 100, dungeonHeight = 100;
    [SerializeField][Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;
    [SerializeField] private Transform player;

    [Header("Enemies")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int minEnemiesPerRoom = 1;
    [SerializeField] private int maxEnemiesPerRoom = 3;

    [Header("Chest")]
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private int minChestPerRoom = 1;
    [SerializeField] private int maxChestPerRoom = 2;

    [Header("Door")]
    [SerializeField] private GameObject doorPrefab;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }



    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),
            minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = randomWalkRooms
            ? CreateRoomsRandomly(roomsList)
            : CreateSimpleRooms(roomsList);

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));

        HashSet<Vector2Int> corridors = ConnectRooms(new List<Vector2Int>(roomCenters));
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        SpawnEnemies(roomsList);
        SpawnChests(roomsList);
        SpawnDoor(roomCenters);

        if (player != null && roomCenters.Count > 0)
            StartCoroutine(MovePlayerNextFrame(tilemapVisualizer.GetCellCenterWorld(roomCenters[0])));
    }

    // =========================
    // ENEMY SPAWN
    // =========================
    private void SpawnEnemies(List<BoundsInt> roomsList)
    {
        bool firstRoom = true;

        foreach (var room in roomsList)
        {
            if (firstRoom) { firstRoom = false; continue; }

            int amount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
            List<Vector2Int> validTiles = GetValidSpawnTiles(room);

            for (int i = 0; i < amount && validTiles.Count > 0; i++)
            {
                int index = Random.Range(0, validTiles.Count);
                Vector2Int pos = validTiles[index];
                validTiles.RemoveAt(index);

                Vector3 worldPos = tilemapVisualizer.GetCellCenterWorld(pos);

                if (IsAreaFree(worldPos, 0.7f))
                    Instantiate(enemyPrefab, worldPos, Quaternion.identity);
            }
        }
    }

    // =========================
    // CHEST SPAWN
    // =========================
    private void SpawnChests(List<BoundsInt> roomsList)
    {
        foreach (var room in roomsList)
        {
            int amount = Random.Range(minChestPerRoom, maxChestPerRoom + 1);
            List<Vector2Int> validTiles = GetValidSpawnTiles(room);

            for (int i = 0; i < amount && validTiles.Count > 0; i++)
            {
                int index = Random.Range(0, validTiles.Count);
                Vector2Int pos = validTiles[index];
                validTiles.RemoveAt(index);

                Vector3 worldPos = tilemapVisualizer.GetCellCenterWorld(pos);

                if (IsAreaFree(worldPos, 0.7f))
                    Instantiate(chestPrefab, worldPos, Quaternion.identity);
            }
        }
    }

    // =========================
    // DOOR SPAWN
    // =========================

    private void SpawnDoor(List<Vector2Int> roomCenters)
    {
        if (doorPrefab == null || player == null) return;

        Vector2Int playerGrid = Vector2Int.RoundToInt(player.position);

        // trova stanza più lontana
        Vector2Int farthestRoom = roomCenters[0];
        float maxDist = 0;

        foreach (var room in roomCenters)
        {
            float dist = Vector2.Distance(room, playerGrid);
            if (dist > maxDist)
            {
                maxDist = dist;
                farthestRoom = room;
            }
        }

        // trova posizione muro frontale
        Vector2Int doorPos = FindFrontWallNear(farthestRoom);

        if (doorPos != Vector2Int.zero)
        {
            // Prendi il centro della prima cella
            Vector3 worldPos = tilemapVisualizer.GetCellCenterWorld(doorPos);

            // SPOSTA il prefab di mezza unità a destra per coprire perfettamente 2 celle
            worldPos += new Vector3(0.5f, 0, 0);

            Instantiate(doorPrefab, worldPos, Quaternion.identity);
        }
    }

    private Vector2Int FindFrontWallNear(Vector2Int center)
    {
        int searchRadius = 100;

        for (int r = 1; r < searchRadius; r++)
        {
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    Vector2Int floorPos = center + new Vector2Int(x, y);

                    // serve pavimento sotto
                    if (!tilemapVisualizer.FloorTileExists(floorPos))
                        continue;

                    // posizione muro frontale
                    Vector2Int wallStart = floorPos + Vector2Int.up;

                    // controlliamo 4 muri consecutivi
                    bool validSequence = true;

                    for (int i = 0; i < 2; i++)
                    {
                        Vector2Int currentWall = wallStart + Vector2Int.right * i;
                        Vector2Int floorBelow = floorPos + Vector2Int.right * i;

                        // deve esserci pavimento sotto
                        if (!tilemapVisualizer.FloorTileExists(floorBelow))
                        {
                            validSequence = false;
                            break;
                        }

                        // sopra NON deve esserci pavimento
                        if (tilemapVisualizer.FloorTileExists(currentWall))
                        {
                            validSequence = false;
                            break;
                        }

                        // lati NON devono avere pavimento
                        if (tilemapVisualizer.FloorTileExists(currentWall + Vector2Int.left) ||
                            tilemapVisualizer.FloorTileExists(currentWall + Vector2Int.right))
                        {
                            validSequence = false;
                            break;
                        }
                    }
                    if (validSequence)
                    {
                        // ritorniamo i 2 centrali per centrare la porta
                        return wallStart;
                    }
                }
            }
        }

        return Vector2Int.zero;
    }





    // =========================
    // VALID TILE LIST
    // =========================
    private List<Vector2Int> GetValidSpawnTiles(BoundsInt room)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int x = offset; x < room.size.x - offset; x++)
        {
            for (int y = offset; y < room.size.y - offset; y++)
            {
                Vector2Int pos = (Vector2Int)room.min + new Vector2Int(x, y);

                if (tilemapVisualizer.FloorTileExists(pos))
                    list.Add(pos);
            }
        }
        return list;
    }

    // =========================
    // COLLISION CHECK
    // =========================
    private bool IsAreaFree(Vector2 worldPos, float size)
    {
        return Physics2D.OverlapBox(worldPos, new Vector2(size, size), 0) == null;
    }

    private IEnumerator MovePlayerNextFrame(Vector3 pos)
    {
        yield return null;
        player.position = pos;
    }

    // =========================
    // GENERATION HELPERS
    // =========================

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in rooms)
            for (int x = offset; x < room.size.x - offset; x++)
                for (int y = offset; y < room.size.y - offset; y++)
                    floor.Add((Vector2Int)room.min + new Vector2Int(x, y));
        return floor;
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in rooms)
        {
            var center = (Vector2Int)Vector3Int.RoundToInt(room.center);
            var walk = RunRandomWalk(randomWalkParameters, center);
            foreach (var pos in walk)
                if (pos.x >= room.xMin + offset && pos.x <= room.xMax - offset &&
                    pos.y >= room.yMin + offset && pos.y <= room.yMax - offset)
                    floor.Add(pos);
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> centers)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var current = centers[Random.Range(0, centers.Count)];
        centers.Remove(current);

        while (centers.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(current, centers);
            centers.Remove(closest);
            corridors.UnionWith(CreateCorridor(current, closest));
            current = closest;
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int a, Vector2Int b)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = a;
        corridor.Add(pos);

        while (pos.y != b.y)
        {
            pos += (b.y > pos.y) ? Vector2Int.up : Vector2Int.down;
            corridor.Add(pos);
        }

        while (pos.x != b.x)
        {
            pos += (b.x > pos.x) ? Vector2Int.right : Vector2Int.left;
            corridor.Add(pos);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int current, List<Vector2Int> list)
    {
        Vector2Int closest = Vector2Int.zero;
        float dist = float.MaxValue;

        foreach (var p in list)
        {
            float d = Vector2.Distance(p, current);
            if (d < dist)
            {
                dist = d;
                closest = p;
            }
        }
        return closest;
    }
}
