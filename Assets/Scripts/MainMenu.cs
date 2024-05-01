using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour{
     public Transform MenuCanvas;
     public GameObject BackgroundImage;
     public VideoPlayer Video;

     public GameObject music;
     public void PlayGame()
     {
          music.SetActive(false);
          StartCoroutine(LoadGame());
     }
     IEnumerator LoadGame()
     {
          //for (int i = 0; i < 4; i++)
          //{
          //     MenuCanvas.GetChild(i).gameObject.SetActive(false);
          //}
          Video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "intro-cutscene.mov"); 
          Video.gameObject.SetActive(true);
          BackgroundImage.SetActive(false);
          GL.Clear(true, true, Color.black);
          float startTime = Time.time;
          //float duration = (float) Video.length;
          float duration = 56f;
          while(Time.time - startTime < duration) {
               if(Input.GetButtonDown("Menu")) break;
               yield return null;
          }
          SceneManager.LoadScene("GameSetupScene");
     }
     public void QuitGame()
     {
          Application.Quit();
     }
     public void testing()
     {
          Debug.Log("You Click Me!");
     }

     public void LoadCredits() {
          SceneManager.LoadScene("Credits");
     }
}
