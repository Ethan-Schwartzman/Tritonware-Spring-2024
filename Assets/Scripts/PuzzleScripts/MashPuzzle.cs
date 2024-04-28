using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class MashPuzzle : Puzzle
{
    public Transform Step;
    public Transform Center;
    private int stepCount;
    private Stack<Transform> steps;

    const int MIN_STEPS = 15;
    const int MAX_STEPS = 30;

    public override int GetDifficulty()
    {
        return 2;
    }

    public override void InitPuzzle(float difficulty)
    {
        steps = new Stack<Transform>();
        stepCount = Random.Range(MIN_STEPS, MAX_STEPS+1);
        float height = 300f/30f;
        for(int i = 0; i < stepCount; i++) {
            Transform s = Instantiate(Step);
            s.parent = Center;
            float offset = (float)i/stepCount * height - (height/2);
            s.position = new Vector3(
                Center.position.x,
                Center.position.y + offset,
                Center.position.z
            );
            s.localScale = new Vector3(1, 0.2f*(1-(float)stepCount/(MAX_STEPS*1.2f)), 1);
            s.gameObject.SetActive(true);
            steps.Push(s);
        }
    }

    public override void OnPuzzle1()
    {
        stepCount--;
        Destroy(steps.Pop().gameObject);

        if(stepCount <= 0) PuzzleManager.Instance.CompletePuzzle(index);
    }

    public override void OnPuzzle2()
    {
        OnPuzzle1();
    }
}
