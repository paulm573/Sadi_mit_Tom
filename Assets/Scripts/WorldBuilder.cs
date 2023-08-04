using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class WorldBuilder : MonoBehaviour
{
    
    public string path_prefabs = "";
    GameObject wall, red, orange, blue, floor, player;
    List<(int, int)> legalSpawns;
    private GameObject world;
    private Vector3 playerSpawn;

    private void Awake()
    {
        playerSpawn = Vector3.zero;
        world = new GameObject();
        world.transform.localScale= Vector3.one;
        world.transform.position= Vector3.zero;
        world.transform.rotation= Quaternion.identity;
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {   
        red = (GameObject)Resources.Load(path_prefabs + "/"+ "red");
        orange = (GameObject)Resources.Load(path_prefabs + "/" + "orange");
        blue = (GameObject)Resources.Load(path_prefabs + "/" + "blue");
        wall = (GameObject)Resources.Load( path_prefabs + "/" + "wall");
        floor = (GameObject)Resources.Load(path_prefabs + "/" + "floor");
        player = (GameObject)Resources.Load(path_prefabs + "/" + "playerOne");
        legalSpawns = new List<(int, int)>();
       
    }

    public Vector3 GetPlayerSpawn() { return playerSpawn; }

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
        Instantiate(floor, new Vector3(floorX, floorY ,floorZ ), Quaternion.Euler(rotation)).transform.SetParent(world.transform);
        // place player
        (int x, int z) = legalSpawns[UnityEngine.Random.Range(0, legalSpawns.Count)];
        playerSpawn = new Vector3(x * nipScale, 3f, z * nipScale);
        // Instantiate(player, position_player, Quaternion.identity).transform.SetParent(world.transform);
        SaveWorldAsPrefab();
        Instantiate((GameObject)Resources.Load("generatedWorld"),  Vector3.zero, Quaternion.identity);
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
            case "wall": Instantiate(wall, pos, Quaternion.Euler(rot)).transform.SetParent(world.transform); break;
            case "orange": Instantiate(orange, pos, Quaternion.Euler(rot)).transform.SetParent(world.transform); break;
            case "blue": Instantiate(blue, pos, Quaternion.Euler(rot)).transform.SetParent(world.transform); break;
            case "red": Instantiate(red, pos, Quaternion.Euler(rot)).transform.SetParent(world.transform); break;
        } 
    }

    private void SaveWorldAsPrefab() 
    {

        string localPath = "Assets/Resources/" + "generatedWorld" + ".prefab";



        PrefabUtility.SaveAsPrefabAsset(world, localPath);
       
        Destroy(world);
        AssetDatabase.Refresh();
    }
}
