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
     public void PlayGame()
     {
          StartCoroutine(LoadGame());
     }
     IEnumerator LoadGame()
     {
          //for (int i = 0; i < 4; i++)
          //{
          //     MenuCanvas.GetChild(i).gameObject.SetActive(false);
          //}
          Video.gameObject.SetActive(true);
          BackgroundImage.SetActive(false);
          GL.Clear(true, true, Color.black);
          float startTime = Time.deltaTime;
          float duration = (float) Video.clip.length + 1.5f;
          while(Time.time - startTime < duration) {
               if(Input.GetButtonDown("Menu")) break;
               yield return null;
          }
          SceneManager.LoadSceneAsync("GameSetupScene");
     }
     public void QuitGame()
     {
          Application.Quit();
     }
     public void testing()
     {
          Debug.Log("You Click Me!");
     }
}
