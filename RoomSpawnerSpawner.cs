using UnityEngine;
using System.Collections.Generic;

public class RoomSpawnerSpawner : MonoBehaviour {


    public GameObject roomSpawner;
    public int radius;
    public static int maxRooms = 50;
    public int maxRoomz;
    void Start ()
    {
        maxRooms = maxRoomz;
        for (int x = 0; x < maxRooms; x++)
        {
            Vector3 Pos = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0);
            GameObject roomSpawnerTemp = Instantiate(roomSpawner, Pos, Quaternion.identity) as GameObject;
            roomSpawnerTemp.name = "" + x;
        }
    }


}
