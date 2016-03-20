using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSpawner : MonoBehaviour
{
    public int minRoomLength, maxRoomLength, minRoomHeight, maxRoomHeight, roomLength, roomHeight;
    public GameObject prefab;
    public GameObject wall;
    public static List<Vector3> allTilePos = new List<Vector3>();
    public static List<Vector3> outSideTiles = new List<Vector3>();
    public static List<Vector3> wallPosition = new List<Vector3>();

    public List<int> pathed = new List<int>();

    public static List<Vector3> neighbourVectors = new List<Vector3>();
    public static bool GeneratedWalls = false;
    public static int currentTile = 0;
    public static int wallNumber = 0;

    void Start()
    {
        for (int x = 0;x < RoomSpawnerSpawner.maxRooms; x++)
        {
            pathed.Add(0);
        }
        pathed[int.Parse(gameObject.name)] = 1;
        CreateRoom();
        Invoke("CreatePathway",0.1f);
        if (GeneratedWalls == false)
        {
            for (int n = 0; n < 8; n ++)
            {
                neighbourVectors.Add(Vector3.zero);
            }
            Invoke("CreateWalls", 0.2f);
            GeneratedWalls = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            CreatePathway();
        }

    }
   
    void CreateRoom()
    {
        roomLength = Random.Range(minRoomLength, maxRoomLength);
        roomHeight = Random.Range(minRoomHeight, maxRoomHeight);
        Vector3 tilePosition = transform.position;

        for (int x = 0;x < roomLength; x++)
        {
            if (!allTilePos.Contains(tilePosition))
            {
                (Instantiate(prefab, tilePosition, Quaternion.identity) as GameObject).transform.parent = transform;
                allTilePos.Add(tilePosition);
            } 

            for(int y = 0;y< roomHeight; y++)
            {
                if (!allTilePos.Contains(tilePosition))
                {
                    (Instantiate(prefab, tilePosition, Quaternion.identity) as GameObject).transform.parent = transform;
                    allTilePos.Add(tilePosition);
                }
                tilePosition += new Vector3(0, 1, 0);
            }
            tilePosition.y -= roomHeight;
            tilePosition += new Vector3(1, 0, 0);
        }
      
        for (int x = 0; x < roomLength; x += 2)
        {
            Vector3 bottomX = transform.position + new Vector3(x, 0, 0);
            outSideTiles.Add(bottomX);
            Vector3 topX = transform.position + new Vector3(x, 0, 0) + new Vector3(0, roomHeight - 1 , 0);
            outSideTiles.Add(topX);
        }

        for (int y = 0; y < roomHeight - 1; y += 2)
        {
            Vector3 leftY = transform.position + new Vector3(0, y, 0);
            outSideTiles.Add(leftY);
            Vector3 rightY = transform.position + new Vector3(0, y, 0) + new Vector3(roomLength - 1, 0, 0);
            outSideTiles.Add(rightY);
        }
        outSideTiles.Add(transform.position + new Vector3(roomLength - 1, roomHeight - 1, 0));

    }

    void CreatePathway()
    {
        int destinationID = Random.Range(0, RoomSpawnerSpawner.maxRooms);
        if (pathed[destinationID] == 1)
        {
            for (int x = 0; x < RoomSpawnerSpawner.maxRooms; x++)
            {
                if (pathed[x] == 0)
                {
                    destinationID = x;
                }
            }
        }
        
        if (pathed[destinationID] == 0 )
        {
            pathed[destinationID] = 1;
            Debug.Log("room " + gameObject.name + " Chose  room " + destinationID + " To CreatePath ");

            GameObject DestinationObj = GameObject.Find("" + destinationID);
            DestinationObj.GetComponent<RoomSpawner>().pathed[int.Parse(gameObject.name)] = 1;

            Vector3 tilePosition = transform.GetChild(Random.Range(0, transform.childCount - 1)).position;
            Vector3 targetPosition = DestinationObj.transform.GetChild(Random.Range(0, DestinationObj.transform.childCount - 1)).position;
            Vector3 distance = targetPosition - tilePosition;

            Debug.Log(distance);

            for (int x = 0; x < Mathf.Abs(distance.x); x++)
            {
                if (!allTilePos.Contains(tilePosition))
                {
                    Instantiate(prefab, tilePosition, Quaternion.identity);
                    allTilePos.Add(tilePosition);
                    if (x % 2 == 0)
                    {
                        outSideTiles.Add(tilePosition);
                    }
                }
                if (distance.x > 0)
                {
                    tilePosition += new Vector3(1, 0, 0);
                }
                if (distance.x < 0)
                {
                    tilePosition += new Vector3(-1, 0, 0);
                }
            }

            for (int y = 0; y < Mathf.Abs(distance.y); y++)
            {
                if (!allTilePos.Contains(tilePosition))
                {
                    Instantiate(prefab,tilePosition,Quaternion.identity);
                    allTilePos.Add(tilePosition);
                    if (y % 2 == 0)
                    {
                        outSideTiles.Add(tilePosition);
                    }
                }
                if (distance.y > 0)
                {
                    tilePosition += new Vector3(0, 1, 0);
                }
                if (distance.y < 0)
                {
                    tilePosition += new Vector3(0, -1, 0);
                }
            }
        }
    }

    void CreateWalls()
    {

        neighbourVectors[0] = outSideTiles[currentTile] + new Vector3(1, 0, 0);
        neighbourVectors[1] = outSideTiles[currentTile] + new Vector3(-1, 0, 0);
        neighbourVectors[2] = outSideTiles[currentTile] + new Vector3(0, 1, 0);
        neighbourVectors[3] = outSideTiles[currentTile] + new Vector3(0, -1, 0);
        neighbourVectors[4] = outSideTiles[currentTile] + new Vector3(1, 1, 0);
        neighbourVectors[5] = outSideTiles[currentTile] + new Vector3(-1, 1, 0);
        neighbourVectors[6] = outSideTiles[currentTile] + new Vector3(1, -1, 0);
        neighbourVectors[7] = outSideTiles[currentTile] + new Vector3(-1, -1, 0);

        for (int n = 0; n < neighbourVectors.Count; n++)
        {
            if (!wallPosition.Contains(neighbourVectors[n]) && !RoomSpawner.allTilePos.Contains(neighbourVectors[n]))
            {
                GameObject wallTemp = Instantiate(wall, neighbourVectors[n], transform.rotation) as GameObject;
                wallTemp.name = "Wall " + wallNumber;
                wallNumber++;
                wallPosition.Add(neighbourVectors[n]);
            }
        }

        currentTile++;
        if (currentTile < outSideTiles.Count)
        {
            Invoke("CreateWalls",0f);
        }
        if (currentTile >= outSideTiles.Count)
        {
            print(Time.realtimeSinceStartup);
            AstarPath.active.Scan();
            for (int x = 0; x < wallNumber; x++) 
            {
                WallColliderGenerator currentWall = GameObject.Find("Wall " + x).GetComponent<WallColliderGenerator>();
                if (currentWall.hasWalled == false)
                {
                    GameObject.Find("Wall " + x).GetComponent<WallColliderGenerator>().Invoke("ColliderHorizontal", 1f);
                }

            }
            GameObject.Find("Wall 0").GetComponent<WallColliderGenerator>().Invoke("ColliderHorizontal", 0);
        }
    }
}
