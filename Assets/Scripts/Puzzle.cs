using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public bool isComplete = false;
    public abstract void OnPuzzle1();

    public abstract void OnPuzzle2();

    public abstract void CompletePuzzle();
}
