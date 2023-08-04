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
    GameObject whiteWall, coin, blueWall, tree, floor;
    List<(int, int)> legalSpawns;
    [SerializeField]Transform playerTransform;

    private void Awake()
    {
        this.transform.localScale= Vector3.one;
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {   
        whiteWall = (GameObject)Resources.Load(path + "/" + "whiteWall");
        coin = (GameObject)Resources.Load(path + "/" + "coin");
        tree = (GameObject)Resources.Load(path + "/" + "tree");
        blueWall = (GameObject)Resources.Load(path + "/" + "blueWall");
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
            
            Vector3 position = new Vector3(i * nipScale, 40f, j * nipScale);

            InstaniateObject(next.type, position, rotation);
        }
        // create Floor
        float floorX = (nipScale * nipCount_I) / 2 - nipScale / 2;
        float floorY = (nipScale / 2 * -1) - 0.5f + 40f;
        float floorZ = (nipScale * nipCount_J) / 2 - nipScale / 2;
        Instantiate(floor, new Vector3(floorX, floorY ,floorZ ), Quaternion.Euler(rotation)).transform.SetParent(this.transform);
        // place player
        (int x, int z) = legalSpawns[UnityEngine.Random.Range(0, legalSpawns.Count)];
        playerTransform.position = new Vector3(x * nipScale, 65f, z * nipScale);
    }

    private void SetScale(int nipScale, int nipCount_I, int nipCount_J)
    {
        Vector3 scale = new Vector3(nipScale,nipScale,nipScale);
        whiteWall.transform.localScale = scale;
        blueWall.transform.localScale = scale;
        coin.transform.localScale = scale;
        tree.transform.localScale = scale;
        floor.transform.localScale = new Vector3(nipScale*nipCount_I,1f,nipScale*nipCount_J);
    }

    private void InstaniateObject(string type, Vector3 pos, Vector3 rot) {
        switch (type)
        {
            case "whiteWall": Instantiate(whiteWall, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "blueWall": Instantiate(blueWall, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "coin": Instantiate(coin, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
            case "tree": Instantiate(tree, pos, Quaternion.Euler(rot)).transform.SetParent(this.transform); break;
        } 
    }
}
