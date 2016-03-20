using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallColliderGenerator : MonoBehaviour {
    public static List<Vector3> WalledWalls = new List<Vector3>();
    public bool hasWalled = false;

    public void ColliderHorizontal()
    {
        BoxCollider2D CurrentCollider = null;

        if (WalledWalls.Contains(transform.position) || hasWalled == true)
        {
            return;
        }
        if (!WalledWalls.Contains(transform.position))
        {
            WalledWalls.Add(transform.position);
            CurrentCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        Vector3 rightWallPos = transform.position + Vector3.right;
        while (RoomSpawner.wallPosition.Contains(rightWallPos))
        {
            if (!WalledWalls.Contains(rightWallPos))
            {
                WalledWalls.Add(rightWallPos);
                CurrentCollider.size += new Vector2(1, 0);
                CurrentCollider.offset += new Vector2(0.5f, 0);
                rightWallPos += Vector3.right;
            }
           
        }


        Vector3 leftWallPos = transform.position + Vector3.left;
        while (RoomSpawner.wallPosition.Contains(leftWallPos))
        {
            if (!WalledWalls.Contains(leftWallPos))
            {
                WalledWalls.Add(leftWallPos);
                CurrentCollider.size += new Vector2(1, 0);
                CurrentCollider.offset += new Vector2(-0.5f, 0);
                leftWallPos += Vector3.left;
            }
                  
        }
      //  ColliderVertical();
    }

    public void ColliderVertical()
    {
        BoxCollider2D CurrentCollider = null;
        if (WalledWalls.Contains(transform.position) || hasWalled == true)
        {
            return;
        }
        if (!WalledWalls.Contains(transform.position))
        {
            WalledWalls.Add(transform.position);
            CurrentCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        Vector3 UpWallPos = transform.position + Vector3.up;
        while (RoomSpawner.wallPosition.Contains(UpWallPos))
        {
            if (!WalledWalls.Contains(UpWallPos))
            {
                WalledWalls.Add(UpWallPos);
                CurrentCollider.size += new Vector2(0, 1);
                CurrentCollider.offset += new Vector2(0, 0.5f);
                UpWallPos += Vector3.up;
            }
        }

        Vector3 downWallPos = transform.position + Vector3.down;
        while (RoomSpawner.wallPosition.Contains(downWallPos))
        {
            if (!WalledWalls.Contains(downWallPos))
            {
                WalledWalls.Add(downWallPos);
                CurrentCollider.size += new Vector2(0, 1);
                CurrentCollider.offset += new Vector2(0, -0.5f);
                downWallPos += Vector3.down;
            }          
        }   
    }

    void Update()
    {
        if (WalledWalls.Contains(transform.position))
        {
            hasWalled = true;
        }
    }



}
