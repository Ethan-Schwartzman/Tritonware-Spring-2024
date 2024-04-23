
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class CodePuzzle : Puzzle
{
    public TMP_Text[] codeKeys;
    public Image[] codeLights;

    public TMP_Text inputText, answerText, completeText, failureText;

    public Color OnColor = Color.green, OffColor = Color.red;

    List<char> codeChars;
    int selectedKey = 0;
    int currentInputLength = 0;

    const int CODE_LENGTH = 3;
    char[] correctCode = new char[CODE_LENGTH];
    char[] currentGuess = new char[CODE_LENGTH];

    bool isComplete = false;
    

    public override void InitPuzzle(float difficulty)
    {
        codeChars = new List<char>();
        for (int i = 0; i < 6; i++)
        {
            char c = (char)Random.Range(33,127);
            while (codeChars.Contains(c))
            {
                c = (char)Random.Range(33, 127);
            }
            codeChars.Add(c);
            codeKeys[i].text = c.ToString();
        }

        for (int i = 0; i < CODE_LENGTH; i++)
        {
            correctCode[i] = codeChars[Random.Range(0, 6)];
        }
        answerText.text = new string(correctCode);
        inputText.text = "";

        SetSelectedKey(Random.Range(0,6));

    }

    public override void OnPuzzle1()
    {
        if (isComplete) return;
        currentGuess[currentInputLength] = codeChars[selectedKey];
        if (currentGuess[currentInputLength] != correctCode[currentInputLength])
        {
            Clear();
            StartCoroutine(Failure());
            return;
        }
        currentInputLength++;
        if (currentInputLength == CODE_LENGTH)
        {
            isComplete = true;
            StartCoroutine(Complete());
        }
        inputText.text = new string(currentGuess);
        
    }

    public override void OnPuzzle2()
    {
        if (isComplete) return;
        SetSelectedKey((selectedKey + 1) % 6);
    }

    public override int GetDifficulty()
    {
        return 5;
    }

    IEnumerator Complete()
    {
        completeText.enabled = true;
        yield return new WaitForSeconds(0.8f);
        PuzzleManager.Instance.CompletePuzzle(index);
    }

    IEnumerator Failure()
    {
        failureText.enabled = true;
        yield return new WaitForSeconds(0.8f);
        failureText.enabled = false;
    }

    void SetSelectedKey(int key)
    {
        for (int i =0; i < 6; i++) 
        { 
            if (i == key)
            {
                codeLights[i].color = OnColor;
            }
            else
            {
                codeLights[i].color = OffColor;
            }
        }
        selectedKey = key;
    }

    void Clear()
    {
        for (int i = 0;i < CODE_LENGTH;i++)
        {
            currentGuess[i] = '\0';
        }
        currentInputLength = 0;
        inputText.text = "";
    }
}
