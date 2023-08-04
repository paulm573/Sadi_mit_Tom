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
    public int NIPPLESIZE; //in cm
    public int NIPPLECOUNTWIDTH;
    public int NIPPLECOUNTHEIGHT;


    [Header("Map from Color to Objectname"), SerializeField]
    public Color[] COLORATINDEX = { Color.white };
    public string[] OBJECTNAMEATINDEX = { "wall" };

    public WorldBuilder worldBuilder;
    public Renderer textureRenderer;

    public string generateMazePath;


    [SerializeField]
     float hueStrength = 0;
    [SerializeField]
     float saturationStrength = 0;
    [SerializeField]
     float valueStrength = 0;
    [SerializeField]
    float percentageOfNippleUsed = 25f;

    private void Start() {

    string nameOfTestImage = "testColors-" + hueStrength + "h-" + saturationStrength + "s- " + valueStrength + "v";

        Texture2D testImage = (Texture2D)Resources.Load(generateMazePath);
       // testImage = testColors(testImage);

        /*
                int count = 0;
                Debug.Log("start");
                for (float i = 0; i < 1; i += 0.1f) { // grid search -> finding the sweetspot of hue, value, saturation for color matching of lego nipples
                    hueStrength += i;
                    for (float j = 0; j < 1; j += 0.1f) {
                        saturationStrength += j;
                        for (float z = 0; z < 1; z += 0.1f) {
                            valueStrength += z;
                            nameOfTestImage = "testColors-" + hueStrength + "h-" + saturationStrength + "s- " + valueStrength + "v";
                            testColors(testImage);
                            count++;
                        }
                        valueStrength = 0;
                        Debug.Log("count: " + count);
                    }
                    saturationStrength = 0;
                }
        */
        Debug.Log("finished!");

        //Debug.Log(testImage + ">>" +  testImage.width + ">>" +  testImage.height);

        Color[,] testResult = ImageReader.ConvertImageToNippelArray(testImage, NIPPLECOUNTWIDTH, NIPPLECOUNTHEIGHT, percentageOfNippleUsed);

        // Debug.Log(testResult.Length);

        DrawComputerVision(testResult, 4);


        Dictionary<Color, string> colorObjectMap = CreateColorObjectMap(COLORATINDEX, OBJECTNAMEATINDEX);

        Stack<WorldObject> worldObjects = ImageReader.MatchNippleToWorldObjects(testResult, colorObjectMap);

        //while (worldObjects.Count > 0)
        //{
        //    WorldObject w = worldObjects.Pop();
        //    Debug.Log(w.position.Item1 + "|" + w.position.Item2 + " >>> " + w.type);
        //}


        worldBuilder.CreateWorld(NIPPLESIZE, NIPPLECOUNTWIDTH, NIPPLECOUNTHEIGHT, worldObjects);

    }

    private Dictionary<Color, string> CreateColorObjectMap(Color[] colors, string[] types) {
        return colors.Zip(types, (col, ty) => new { col, ty }).ToDictionary(e => e.col, e => e.ty);
    }

    private void DrawComputerVision(Color[,] colourMap, int scaleFactor) {
        int size = colourMap.GetLength(0);


        //___
        Texture2D texture = new Texture2D(colourMap.GetLength(0) * scaleFactor, colourMap.GetLength(1) * scaleFactor);

        Color[,] colorMapUpscaled = new Color[colourMap.GetLength(0) * scaleFactor, colourMap.GetLength(1) * scaleFactor];
        for (int i = 0; i < colourMap.GetLength(0); i++) {
            for (int j = 0; j < colourMap.GetLength(1); j++) {
                for (int b = 0; b < scaleFactor; b++) {
                    for (int c = 0; c < scaleFactor; c++) {
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
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    static Color[] To1DArray(Color[,] input) {

        int size = input.Length;
        Color[] result = new Color[size];


        int write = 0;
        for (int z = 0; z <= input.GetUpperBound(1); z++) {
            for (int i = 0; i <= input.GetUpperBound(0); i++) {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }




    public Texture2D testColors(Texture2D tex) {
        Texture2D result = new Texture2D(tex.width, tex.height);

        for (int x = 0; x < result.width; x++) {
            for (int y = 0; y < result.height; y++) {
                float aH, aS, aV;
                Color.RGBToHSV(tex.GetPixel(x, y), out aH, out aS, out aV);
                Color newPixelColor = Color.HSVToRGB(aH * hueStrength, aS * saturationStrength, aV * valueStrength);
                result.SetPixel(x, y, newPixelColor);

            }
        }
        return result;
        // savePictureToFile(result);
    }
    /*
    public void savePictureToFile(Texture2D takenImage) {   //save each file to file explorer for later comparison
        if (takenImage != null) {
            byte[] byteArray = takenImage.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/TestImages/" + nameOfTestImage + ".png", byteArray);
            Debug.Log("written the image to " + Application.dataPath + "/TestImages/");
            //AssetDatabase.Refresh();
            System.GC.Collect();
        }
    }
    */
}