using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;


public class WorldBuilder : MonoBehaviour
{
    public string path = "";
    GameObject wall, red, orange, blue, floor;
    List<(int, int)> legalSpawns;
    [SerializeField]Transform playerTransform;

    private void Awake()
    {
        this.transform.localScale= Vector3.one;
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {   
        red = (GameObject)Resources.Load(path + "/" + "red");
        orange = (GameObject)Resources.Load(path + "/" + "orange");
        blue = (GameObject)Resources.Load(path + "/" + "blue");
        wall = (GameObject)Resources.Load(path + "/" + "wall");
        floor = (GameObject)Resources.Load(path + "/" + "floor");
        legalSpawns = new List<(int, int)>();
       
    }

    public void CreateWorld(int nipScale,int nipCount_I, int nipCount_J, Stack<WorldObject> worldObjects) 
    {
        SetScale(nipScale,nipCount_I,nipCount_J);
        Vector3 rotation = Vector3.zero;
        // create Objects
        while (worldObjects.Count > 0)
        {
            WorldObject next = worldObjects.Pop();
            if (next.type == "floor")
            {
                legalSpawns.Add(next.position);
                continue;
            } 

            
            (int i, int j) = next.position;
            
            Vector3 position = new Vector3(i * nipScale, 0f, j * nipScale);

            InstaniateObject(next.type, position, rotation);
        }
        // create Floor
        float floorX = (nipScale * nipCount_I) / 2 - nipScale / 2;
        float floorY = (nipScale / 2 * -1) - 0.5f;
        float floorZ = (nipScale * nipCount_J) / 2 - nipScale / 2;
        Instantiate(floor, new Vector3(floorX, floorY ,floorZ ), Quaternion.Euler(rotation)).transform.SetParent(this.transform);
        // place player
        (int x, int z) = legalSpawns[UnityEngine.Random.Range(0, legalSpawns.Count)];
        playerTransform.position = new Vector3(x * nipScale, 10f, z * nipScale);
    }

    private void SetScale(int nipScale, int nipCount_I, int nipCount_J)
    {
        Vector3 scale = new Vector3(nipScale,nipScale,nipScale);
        red.transform.localScale = scale;
        orange.transform.localScale = scale;    
        blue.transform.localScale = scale;
        wall.transform.localScale = scale;
        floor.transform.localScale = new Vector3(nipScale*nipCount_I,1f,nipScale*nipCount_J);
    }

    private void InstaniateObject(string type, Vector3 pos, Vector3 rot) {
        switch (type)
        {
            case "wall": Instantiate(wall, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "orange": Instantiate(orange, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "blue": Instantiate(blue, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "red": Instantiate(red, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
        } 
    }
}
