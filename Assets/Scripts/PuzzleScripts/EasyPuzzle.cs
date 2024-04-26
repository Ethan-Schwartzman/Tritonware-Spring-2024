using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Puzzle Example: Sliding selector
 * To add new puzzle: Create the nececssary UI elements, and have one puzzle script 
 * that is a subclass of Puzzle and follows this format.
 * Then, create a prefab of the gameObject that the puzzle is attached to.
 * Finally, add the prefab to the puzzleTemplates array under the Puzzle Canvas gameObject.
 * 
 * Press F to spawn a random puzzle from the array to the screen.
 */

public class EasyPuzzle : Puzzle
{
    public Collider2D SelectorCollider;
    public Collider2D IndicatorCollider;
    public RectTransform SelectorTransform;
    public RectTransform Center;

    private float speed = 300f/30f;
    private float range = 200f/30f;
    private bool movingRight = true;

    // sets initial state of puzzle: randomizes selector position and sets difficulty
    public override void InitPuzzle(float difficulty)
    {
        SelectorTransform.position = Center.position + Random.Range(-range, range) * Vector3.right;
        speed *= difficulty;
    }


    // input: left arrow
    public override void OnPuzzle1()
    {
        // puzzle completion condition: check if selector (red) is touching indicator (green)
        if (Physics2D.IsTouching(SelectorCollider, IndicatorCollider)) {
            
            PuzzleManager.Instance.CompletePuzzle(index); // complete puzzle when completion condition is met
        }
    }

    // input: right arrow
    public override void OnPuzzle2()
    {
        return;
    }

    public override int GetDifficulty()
    {
        return 1;
    }

    // puzzle mechanics here: move selector left and right linearly
    void Update() {
        if(movingRight) {
            SelectorTransform.position = new Vector3(
                SelectorTransform.position.x + speed * Time.deltaTime,
                SelectorTransform.position.y,
                SelectorTransform.position.z
            );

            if(SelectorTransform.position.x - Center.position.x > range) movingRight = false;
        }
        else {
            SelectorTransform.position = new Vector3(
                SelectorTransform.position.x - speed * Time.deltaTime,
                SelectorTransform.position.y,
                SelectorTransform.position.z
            );

            if(Center.position.x - SelectorTransform.position.x > range) movingRight = true;
        }
    }
}
