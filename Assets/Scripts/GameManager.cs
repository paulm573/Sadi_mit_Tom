using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //-----------------------------//
    //---------GlobalFields--------//
    //-----------------------------//
    [Header("Nipples Data")]
    public int NIPPLESIZE ; //in cm
    public int NIPPLECOUNTWIDTH;
    public int NIPPLECOUNTHEIGHT;

    [Header("Field Dimensions")]
    public int PLAYGROUNDWIDTH ; //in cm
    public int PLAYGROUNDHEIGHT; //in cm
    [Header("Map from Color to Objectname"), SerializeField]
    public Color[] COLORATINDEX = { Color.white };
    public string[] OBJECTNAMEATINDEX = { "wall" };

    public WorldBuilder worldBuilder;
    public Renderer textureRenderer;
    
    [SerializeField]
    string imageNameForWorldGeneration;


    /// <summary>
    /// Builds the World onClick with desired image
    /// -> ich habe nur den Code aus der Start in eine diese Methode kopiert, damit das erst auf Knopdruck passiert.
    /// </summary>
    public void Start() {

        //-> die Images, die geladen werden, sind "non readable" das muss in den Importersettings eingestellt, werden: solution laut Stackoverflow:
        // https://stackoverflow.com/questions/25175864/making-a-texture2d-readable-in-unity-via-code
        // wenn man die Datei schon hat und die read/write settings behält, bleibts erhältlich
        Texture2D testImage = (Texture2D)Resources.Load(imageNameForWorldGeneration);
        if (testImage == null) {
            throw new System.Exception("no image with " + imageNameForWorldGeneration + "found ->check generation and folderstruture");
        }
        Debug.Log(testImage + ">>" + testImage.width + ">>" + testImage.height);

        Color[,] testResult = ImageReader.ConvertImageToNippelArray(testImage, NIPPLECOUNTWIDTH, NIPPLECOUNTHEIGHT);

        // Debug.Log(testResult.Length);

        DrawComputerVision(testResult, 4);


        Dictionary<Color, string> colorObjectMap = CreateColorObjectMap(COLORATINDEX, OBJECTNAMEATINDEX);

        Stack<WorldObject> worldObjects = ImageReader.MatchNippleToWorldObjects(testResult, colorObjectMap);

        //while (worldObjects.Count > 0)
        //{
        //    WorldObject w = worldObjects.Pop();
        //    Debug.Log(w.position.Item1 + "|" + w.position.Item2 + " >>> " + w.type);
        //}

        worldBuilder.CreateWorld(40, NIPPLECOUNTWIDTH, NIPPLECOUNTHEIGHT, worldObjects);
    }


    private Dictionary<Color, string> CreateColorObjectMap(Color[] colors, string[] types)
    { 
          return colors.Zip(types, (col,ty) => new { col, ty}).ToDictionary(e => e.col, e => e.ty);
    }

    private void DrawComputerVision(Color[,] colourMap, int scaleFactor)
    {
        int size = colourMap.GetLength(0);
        

        //___
        Texture2D texture = new Texture2D(colourMap.GetLength(0) * scaleFactor, colourMap.GetLength(1) * scaleFactor);

        Color[,] colorMapUpscaled = new Color[colourMap.GetLength(0) * scaleFactor, colourMap.GetLength(1) * scaleFactor];
        for (int i = 0; i < colourMap.GetLength(0); i++)
        {
            for (int j = 0; j < colourMap.GetLength(1); j++)
            {
                for (int b = 0; b < scaleFactor; b++)
                {
                    for (int c = 0; c < scaleFactor; c++)
                    {
                        colorMapUpscaled[i * scaleFactor + b, j * scaleFactor + c] = colourMap[i, j];
                    }
                }
            }
        }


        texture.SetPixels(To1DArray(colorMapUpscaled));
        //____

        //********* 1 to 1
        //Texture2D texture = new Texture2D(colourMap.GetLength(0), colourMap.GetLength(1));
        //texture.SetPixels(To1DArray(colourMap));
        // **********

        texture.Apply();


        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width,1, texture.height);
    }

    static Color[] To1DArray(Color[,] input)
    {
        
        int size = input.Length;
        Color[] result = new Color[size];
 
        
        int write = 0;
        for (int z = 0; z <= input.GetUpperBound(1); z++)
        {
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }

  

}