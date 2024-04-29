using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Loading;
using UnityEditor.Compilation;

public class CaptchaPuzzle : Puzzle
{
    public Transform PreCaptcha;
    public Transform Captcha;
    public Transform Select;
    public Sprite[] CaptchaSprites;
    private int selected = 0;
    private int correct = 0;
    private bool loading = false;
    private bool[] toggle = {false, false, false, false, false, false};
    private int[] appointedSprite = {0, 1, 2, 3, 4, 5};
    private HashSet<int> fishIndex = new HashSet<int> {0, 1, 2};
    private Transform[] Images = new Transform[6];

    public override int GetDifficulty()
    {
        return 3;
    }

    public override void InitPuzzle(float difficulty)
    {
        PreCaptcha.gameObject.SetActive(true);
        Captcha.gameObject.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            Images[i] = Captcha.GetChild(4+i);
            Images[i].GetChild(0).gameObject.SetActive(false);
        }
        SetSelected(0);
    }

    public override void OnPuzzle1()
    {
        if (!loading && PreCaptcha.gameObject.activeSelf)
        {
            PreCaptcha.GetChild(1).gameObject.SetActive(false);
            PreCaptcha.GetChild(2).gameObject.SetActive(true);
            StartCoroutine(Loading());
            loading = true;
        } else if (Captcha.gameObject.activeSelf){
            if (fishIndex.Contains(appointedSprite[selected])){
                if (!toggle[selected]){
                    toggle[selected] = true;
                    correct++;
                } else{
                    toggle[selected] = false;
                    correct--;
                }
            } else{
                if (!toggle[selected]){
                    toggle[selected] = true;
                    correct--;
                } else{
                    toggle[selected] = false;
                    correct++;
                }
            }
            Images[selected].GetChild(0).gameObject.SetActive(toggle[selected]);
            //Debug.Log("# of Correct: " + correct);
            if (correct == 3){
                PuzzleManager.Instance.CompletePuzzle(index);
            }
        }
        
    }

    void Update(){
        if (PreCaptcha.gameObject.activeSelf && PreCaptcha.GetChild(2).gameObject.activeSelf){
            PreCaptcha.GetChild(2).Rotate(0, 0, Time.deltaTime * 200);
        }
    }

    public override void OnPuzzle2()
    {
        if (Captcha.gameObject.activeSelf)
        {
            SetSelected((selected + 1) % 6);
        }
    }

    public void SetSelected(int index){
        selected = index;
        Select.position = Images[selected].position;
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        PreCaptcha.gameObject.SetActive(false);
        Captcha.gameObject.SetActive(true);
        RandomizeSprites();
    }

    public void RandomizeSprites(){
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(i, 6);
            int temp = appointedSprite[i];
            appointedSprite[i] = appointedSprite[randomIndex];
            appointedSprite[randomIndex] = temp;
        }
        for (int i = 0; i < 6; i++)
        {
            Images[i].GetComponent<Image>().sprite = CaptchaSprites[appointedSprite[i]];
        }
    }
}
