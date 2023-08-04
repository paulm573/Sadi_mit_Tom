using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public static class ImageReader
{
    // ********************** Fine Tuning *************************

  

    // Light Sensitivity (in percent)
    private const float lightSensitivity = 1f;
    private const float saturationStrengh = 1f;

    // *************************************************************

    public static Stack<WorldObject> MatchNippleToWorldObjects(Color[,] nippleMap,Dictionary<Color,string> colorMap)
    {
        Stack<WorldObject> worldObjects = new Stack<WorldObject>();
        Color[] encodedColors = colorMap.Keys.ToArray();

        for (int i = 0; i < nippleMap.GetLength(0); i++)
        {
            for (int j = 0; j < nippleMap.GetLength(1); j++)
            {
                Color bestMatch = FindClosestMatch(nippleMap[i, j], encodedColors);
                WorldObject next = new WorldObject((i, j), colorMap[bestMatch]);
                worldObjects.Push(next);
            }
        }
        return worldObjects;
    }

    public static Color[,] ConvertImageToNippelArray(Texture2D image, int nippleCountW, int nippleCountH, float percentageOfNippleUsed)
    {
          // Analysed area per Nipple (from the center towards the outside) (in percent)
    float percentageOffset = (100f - percentageOfNippleUsed) / 2f;


    Color[,] result = new Color[nippleCountW, nippleCountH];
        
        int pixelPerNippel_I = image.width / nippleCountW ;
        int pixelPerNippel_J = image.height / nippleCountH ;
        int analysedArea_I = (int)(pixelPerNippel_I * (percentageOfNippleUsed / 100f));
        int analysedArea_J = (int)(pixelPerNippel_J * (percentageOfNippleUsed / 100f));
        int nippleOffset_I = (int)(pixelPerNippel_I * (percentageOffset / 100f));
        int nippleOffset_J = (int)(pixelPerNippel_J * (percentageOffset / 100f));


        int currentPixel_I = 0;
        int currentPixel_J = 0;

        for (int i = 0; i < nippleCountW; i++)
        {
            for (int j = 0; j < nippleCountH; j++)
            {
                result[i, j] = GetNippelAverageColor(image, currentPixel_I, currentPixel_J, analysedArea_I, nippleOffset_I, analysedArea_J, nippleOffset_J);
                currentPixel_J += pixelPerNippel_J;
            }
            currentPixel_I += pixelPerNippel_I;
            currentPixel_J = 0;
        }

        return result;
    }

    private static Color GetNippelAverageColor(Texture2D image, int start_I, int start_J, int area_I, int offset_I, int area_J, int offset_J)
    {
        float r_buffer = 0;
        float g_buffer = 0;
        float b_buffer = 0;
        float alpha_buffer = 0;

        int pixelCount = 0;

        for (int i = start_I + offset_I; i < start_I + offset_I + area_I; i++)
        {
            for (int j = start_J + offset_J; j < start_J + offset_J + area_J; j++)
            {
                Color currentPixel = image.GetPixel(i, j);

                r_buffer += currentPixel.r;
                g_buffer += currentPixel.g;
                b_buffer += currentPixel.b;
                alpha_buffer += currentPixel.a;

                pixelCount++;
            }
        }

        return new Color(r_buffer / pixelCount, g_buffer / pixelCount, b_buffer / pixelCount, alpha_buffer/ pixelCount);
    }

    private static float DistanceBetweenColors(Color a, Color b)
    {
        float aH, aS, aV, bH, bS, bV;
        Color.RGBToHSV(a, out aH, out aS, out aV);
        Color.RGBToHSV(b, out bH, out bS, out bV);

        float x = Mathf.Pow(bH - aH, 2);
        float y = Mathf.Pow(bS - aS, 2);
        float z = Mathf.Pow(bV - aV, 2);  
        return Mathf.Sqrt(x + y + z);
    }

    private static Color FindClosestMatch(Color color, Color[] colors)
    { 
        float bestMatch = float.MaxValue;
        Color closestColor = new Color();

        foreach (Color next in colors)
        {
            float dist = DistanceBetweenColors(next, color);
            if (dist < bestMatch)
            { 
                bestMatch = dist;
                closestColor = next;
            }
        }

        return closestColor;
    }
}

