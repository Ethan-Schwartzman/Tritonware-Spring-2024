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

    private float countdown;

    public override int GetDifficulty()
    {
        return 2;
    }

    public override void InitPuzzle(float difficulty)
    {
        countdown = 2f;
        ScoreText.text = countdown.ToString();
        StartCoroutine(Countdown());
    }

    public override void OnPuzzle1()
    {
        countdown += 1f;
        StartCoroutine(Flash());
    }

    public override void OnPuzzle2()
    {
        countdown += 1f;
        StartCoroutine(Flash());
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
