using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public AspectRatioManager AspectRatio;
    public List<ParallaxLayer> Layers;
    public Camera MainCamera;
    private float width;
    private  float height;
    private float scaleFactor;

    void Start() {
        AspectRatio.UpdateAspectRatio();
        InitParallax();
    }
    
    void InitParallax() {
        Canvas canvas = Layers[0].GetComponent<Canvas>();
        //scaleFactor = canvas.transform.localScale.x;
        scaleFactor = canvas.transform.localScale.x;
        height = canvas.pixelRect.height;
        width = canvas.pixelRect.width;
        //height = MainCamera.rect.height * MainCamera.pixelRect.height;
        //height = MainCamera.rect.width * MainCamera.pixelRect.width;
        //width = MainCamera.rect.width;
        //height = MainCamera.pixelHeight;
        //width = MainCamera.pixelWidth;

        //height = MainCamera.rect.height * MainCamera.pixelRect.height;
        //width = MainCamera.rect.width * MainCamera.pixelRect.width;

        //height = MainCamera.rect.height;
        //width = MainCamera.rect.width;


        Debug.Log("height: " + height);
        Debug.Log("width: " + width);
        Debug.Log("scalefactor:" + scaleFactor);

        foreach(ParallaxLayer layer in Layers) {
            RectTransform rt = (RectTransform)layer.BottomLeft.transform;
            rt.sizeDelta = new Vector2(width, height);
            rt.position = new Vector2(0, 0)*scaleFactor;

            rt = (RectTransform)layer.BottomRight.transform;
            rt.sizeDelta = new Vector2(width, height);
            rt.position = new Vector2(width, 0)*scaleFactor;

            rt = (RectTransform)layer.UpperLeft.transform;
            rt.sizeDelta = new Vector2(width, height);
            rt.position = new Vector2(0, height)*scaleFactor;

            rt = (RectTransform)layer.UpperRight.transform;
            rt.sizeDelta = new Vector2(width, height);
            rt.position = new Vector2(width, height)*scaleFactor;
        }
    }

    public void UpdateParallax(Vector3 deltaPos, Vector3 cameraPos) {

        Canvas canvas = Layers[0].GetComponent<Canvas>();
        scaleFactor = canvas.transform.localScale.x;
        Debug.Log("scalefactor:" + scaleFactor);
        foreach (ParallaxLayer layer in Layers) {
            foreach(RectTransform tile in layer.Tiles) {
                tile.transform.position -= deltaPos*layer.ParallaxIntensity;

                // x
                if((cameraPos - tile.transform.position).x/scaleFactor > width) {
                    tile.transform.position = new Vector3(
                        tile.transform.position.x + 2*width*scaleFactor,
                        tile.transform.position.y,
                        0
                    );
                }
                else if((cameraPos - tile.transform.position).x/scaleFactor < -width) {
                    tile.transform.position = new Vector3(
                        tile.transform.position.x - 2*width*scaleFactor,
                        tile.transform.position.y,
                        0
                    );
                }

                // y
                if((cameraPos - tile.transform.position).y/scaleFactor > height) {
                    tile.transform.position = new Vector3(
                        tile.transform.position.x,
                        tile.transform.position.y + 2*height*scaleFactor,
                        0
                    );
                }
                else if((cameraPos - tile.transform.position).y/scaleFactor < -height) {
                    tile.transform.position = new Vector3(
                        tile.transform.position.x,
                        tile.transform.position.y - 2*height*scaleFactor,
                        0
                    );
                }
            }
        }
    }
}
