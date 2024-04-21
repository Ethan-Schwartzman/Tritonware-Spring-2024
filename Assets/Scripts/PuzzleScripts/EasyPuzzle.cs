using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPuzzle : Puzzle
{
    public BoxCollider2D SelectorCollider;
    public BoxCollider2D IndicatorCollider;
    public Transform SelectorTransform;
    public Transform Center;

    private float speed = 10f;
    private float range = 10f;
    private bool movingRight = true;

    private bool isComplete = false;

    public override bool IsComplete { get{ return isComplete;} }

    public override void OnPuzzle1()
    {
        if(Physics2D.IsTouching(SelectorCollider, IndicatorCollider)) {
            isComplete = true;
            Debug.Log("complete");
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
