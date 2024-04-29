using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoTouchPuzzle : Puzzle
{
    public TextMeshProUGUI ScoreText;
    public GameObject flash;
    public bool AllControlsTrigger = false;
    public float failIncrease = 0.5f;
    public float countdownStart = 2f;
    private float countdown;


    public override int GetDifficulty()
    {
        return 3;
    }

    public override void InitPuzzle(float difficulty)
    {
        countdown = countdownStart;
        ScoreText.text = countdown.ToString();
        if(isActiveAndEnabled) StartCoroutine(Countdown());
    }

    public override void OnPuzzle1()
    {
        countdown += failIncrease;
        if(isActiveAndEnabled) StartCoroutine(Flash());
    }

    public override void OnPuzzle2()
    {
        OnPuzzle1();
    }

    void Update(){
        if (AllControlsTrigger && Input.GetButtonDown("Left") || Input.GetButtonDown("Right")){
            OnPuzzle1();
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.01f);
        countdown -= 0.01f;
        ScoreText.text = countdown.ToString("n2");
        if(countdown > 0) StartCoroutine(Countdown());
        else PuzzleManager.Instance.CompletePuzzle(index);
    }

    IEnumerator Flash()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flash.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        flash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
    }
}
