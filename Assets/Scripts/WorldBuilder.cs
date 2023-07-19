using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldBuilder : MonoBehaviour
{
    public string path = "";
    Plane floor;
    GameObject wall, red, orange, blue;

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

    }

    public void CreateWorld(int nipScale,int nipCount_I, int nipCount_J, Stack<WorldObject> worldObjects) 
    {
        SetScale(nipScale);

        while(worldObjects.Count > 0)
        {
            WorldObject next = worldObjects.Pop();
            if (next.type == "floor") continue;

            
            (int i, int j) = next.position;
            Vector3 rotation = Vector3.zero;
            Vector3 position = new Vector3(i * nipScale, 0f, j * nipScale);

            InstaniateObject(next.type, position, rotation);
        }
    }

    private void SetScale(int nipScale)
    {
        Vector3 scale = new Vector3(nipScale,nipScale,nipScale);
        red.transform.localScale = scale;
        orange.transform.localScale = scale;    
        blue.transform.localScale = scale;
        wall.transform.localScale = scale;
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
