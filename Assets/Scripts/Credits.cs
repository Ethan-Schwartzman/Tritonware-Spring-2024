using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{
    public VideoPlayer Video;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreditsCoroutine());
    }

    private IEnumerator CreditsCoroutine() {
        float startTime = Time.deltaTime;
        float duration = 18.5f;
        while(Time.time - startTime < duration) {
            if(Input.GetButtonDown("Menu")) break;
            yield return null;
        }
        SceneManager.LoadScene("IntroScene");
    }
}
