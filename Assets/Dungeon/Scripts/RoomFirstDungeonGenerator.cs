using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    private Transform player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int minEnemiesPerRoom = 1;
    [SerializeField] private int maxEnemiesPerRoom = 3;



    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {

        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        Debug.Log("Rooms list count: " + roomsList.Count);


        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(new List<Vector2Int>(roomCenters));
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        SpawnEnemies(roomsList);


        // ---- MOVE PLAYER TO FIRST ROOM ----
        if (player != null && roomCenters.Count > 0)
        {
            Vector2Int spawnPosition = roomCenters[0];
            Vector3 spawnWorldPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

            StartCoroutine(MovePlayerNextFrame(spawnWorldPosition));
        }
        Debug.Log("Room centers count: " + roomCenters.Count);
        Debug.Log("Player reference: " + player);


    }

    private void SpawnEnemies(List<BoundsInt> roomsList)
    {
        bool firstRoom = true;

        foreach (var room in roomsList)
        {
            if (firstRoom)
            {
                firstRoom = false;
                continue;
            }
            int enemiesToSpawn = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);

            // Raccogli solo le posizioni valide della stanza
            HashSet<Vector2Int> validTiles = new HashSet<Vector2Int>();

            for (int x = offset; x < room.size.x - offset; x++)
            {
                for (int y = offset; y < room.size.y - offset; y++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(x, y);
                    if (tilemapVisualizer.FloorTileExists(pos)) // <- metodo che controlla se c'è tile dipinta
                    {
                        validTiles.Add(pos);
                    }
                }
            }

            List<Vector2Int> validList = new List<Vector2Int>(validTiles);

            for (int i = 0; i < enemiesToSpawn && validList.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, validList.Count);
                Vector2Int spawnPos = validList[randomIndex];
                validList.RemoveAt(randomIndex);

                Vector3 worldPos = new Vector3(spawnPos.x, spawnPos.y, 0);
                Instantiate(enemyPrefab, worldPos, Quaternion.identity);
            }
        }
    }


    private Vector2Int GetRandomPositionInRoom(BoundsInt room)
    {
        int x = Random.Range(room.xMin + offset, room.xMax - offset);
        int y = Random.Range(room.yMin + offset, room.yMax - offset);

        return new Vector2Int(x, y);
    }



    private IEnumerator MovePlayerNextFrame(Vector3 position)
    {
        yield return null; // aspetta un frame
        player.position = position;
    }


    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
