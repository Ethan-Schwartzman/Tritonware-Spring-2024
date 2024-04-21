using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPuzzle : Puzzle
{
    public Collider2D SelectorCollider;
    public Collider2D IndicatorCollider;
    public RectTransform SelectorTransform;
    public RectTransform Center;

    private float speed = 300f;
    private float range = 200f;
    private bool movingRight = true;


    public override void InitPuzzle(float difficulty)
    {
        SelectorTransform.position = Center.position + Random.Range(-range, range) * Vector3.right;
        speed *= difficulty;
    }



    public override void OnPuzzle1()
    {
        if(Physics2D.IsTouching(SelectorCollider, IndicatorCollider)) {
            PuzzleManager.Instance.CompletePuzzle(index);
        }
    }

    public override void OnPuzzle2()
    {
        return;
    }

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
