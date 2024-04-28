using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour{
     public Transform MenuCanvas;
     public void PlayGame()
     {
          StartCoroutine(LoadGame());
     }
     IEnumerator LoadGame()
     {
          for (int i = 0; i < 4; i++)
          {
               MenuCanvas.GetChild(i).gameObject.SetActive(false);
          }
          MenuCanvas.GetChild(4).gameObject.SetActive(true);
          yield return new WaitForSeconds((float) MenuCanvas.GetChild(4).GetComponent<VideoPlayer>().clip.length);
          SceneManager.LoadSceneAsync("SampleScene");
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
