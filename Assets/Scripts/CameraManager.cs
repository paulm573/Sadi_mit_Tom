using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{

    private WebCamDevice[] webcamDevices;
    private WebCamTexture WebCamTexture;
    [SerializeField]
    private RawImage cameraPreview;

    private Texture snapShotPreview;

    private void Start() {
        webcamDevices = WebCamTexture.devices;
        if (webcamDevices.Length == 0) {
            throw new System.Exception("no camera devices found");
        }

        WebCamTexture = new WebCamTexture(webcamDevices[0].name);
        WebCamTexture.Play();


    }

    private void Update() {
        cameraPreview.texture = WebCamTexture;
    }

    public void TakePicture() {
     //   WebCamTexture.Stop();
        Debug.Log("took picture");
        GameObject ssp = GameObject.Find("PicturePreview");
        if (ssp == null) {
            throw new System.Exception("no such Gameobject found: PicturePreview");
        }
        Texture2D takenImage = new Texture2D(WebCamTexture.height, WebCamTexture.height);
        takenImage.SetPixels(WebCamTexture.GetPixels());
        takenImage = createImage(takenImage);
        takenImage.Apply();
        ssp.GetComponent<RawImage>().texture = takenImage;
        //TODO -> fixen, pixel werden angezeigt, aber nicht in der richtigen Reihenfolge!


    }

    private Texture2D createImage(Texture2D tex) {
     
        Texture2D result = new Texture2D(WebCamTexture.height, WebCamTexture.height); // create squared Array for squared Image
      
        int spaceToRemove = WebCamTexture.width / 4;
        Debug.Log("leftBorder: " + spaceToRemove + "rightBorder: " + (WebCamTexture.width - spaceToRemove) + "width: " + (WebCamTexture.width - spaceToRemove));
        for (int x = spaceToRemove; x < WebCamTexture.width - spaceToRemove; x++) { // create the squared array
            for (int y = 0; y < WebCamTexture.height; y++) {
                int loc = x + (y * WebCamTexture.width); // get location in 1d array
                result.SetPixel(x, y, tex.GetPixel(x,y));
            }
        }
      
        return result;

    }

    public void RemovePicture() {
        WebCamTexture.Play();
        Debug.Log("remove picture");
    }

}
