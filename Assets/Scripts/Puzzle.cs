using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    private static int SCREEN_WIDTH = 1920;
    private static int SCREEN_HEIGHT = 1080;
    protected int index;



    public void SetPosition(int index)
    {
        this.index = index;
        int x, y;
        x = index % 3;
        y = index / 3;
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(SCREEN_WIDTH / 3 * x, -SCREEN_HEIGHT / 2 * y, 0);
    }
    public abstract void InitPuzzle(float difficulty);
    public abstract void OnPuzzle1();

    public abstract void OnPuzzle2();

    public abstract int GetDifficulty();

    //public abstract void CompletePuzzle();
    //public abstract bool IsComplete() {};
}
