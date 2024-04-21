using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public abstract bool IsComplete { get; }
    public abstract void OnPuzzle1();

    public abstract void OnPuzzle2();

    //public abstract void CompletePuzzle();
    //public abstract bool IsComplete() {};
}
