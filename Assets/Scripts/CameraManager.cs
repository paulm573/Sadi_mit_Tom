using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{

    private WebCamDevice[] webcamDevices;
    private WebCamTexture WebCamTexture;
    [SerializeField]
    private RawImage cameraPreview;
    Texture2D takenImage;
    [SerializeField,Tooltip("name of the file where the image as map for WorldGeneration is stored")]
    string fileName;

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
        Debug.Log("took picture");
        GameObject ssp = GameObject.Find("PicturePreview");
        if (ssp == null) {
            throw new System.Exception("no such Gameobject found: PicturePreview");
        }
        takenImage = new Texture2D(WebCamTexture.width, WebCamTexture.height);
        takenImage.SetPixels(WebCamTexture.GetPixels());
        takenImage = createImage(takenImage);
        takenImage.Apply();
        ssp.GetComponent<RawImage>().texture = takenImage;
      
        WebCamTexture.Stop();
    }

    private Texture2D createImage(Texture2D tex) {
     
        Texture2D result = new Texture2D(WebCamTexture.height, WebCamTexture.height); // create squared Array for squared Image
      
        int spaceToRemove =(WebCamTexture.width - WebCamTexture.height) / 2;
        Debug.Log("leftBorder: " + spaceToRemove + " rightBorder: " + (WebCamTexture.width - spaceToRemove));
        Debug.Log("new width: " + (WebCamTexture.width - spaceToRemove - spaceToRemove));
        Debug.Log("width: " + WebCamTexture.width + "height: " + WebCamTexture.height);
       // Color[] tmp = new Color[WebCamTexture.height];
        for (int x = spaceToRemove; x < WebCamTexture.width - spaceToRemove; x++) { // create the squared array
            for (int y = 0; y < WebCamTexture.height; y++) {
                int loc = x + (y * WebCamTexture.width);
                result.SetPixel(x-spaceToRemove, y, tex.GetPixel(x, y));
            }
        }
       // result.SetPixels(tmp); 

        return result;

    }

    public void RemovePicture() {
        WebCamTexture.Play();
        Debug.Log("remove picture");
    }

     public void ContinueWithThisPicture() {
        if (takenImage != null) {
            byte[] byteArray = takenImage.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/" + fileName + ".png", byteArray);
            Debug.Log("written the image to " + Application.dataPath + "/Resources/"); 
        }
    }

}
