using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopTheLockPuzzle : Puzzle
{
    public TextMeshProUGUI CountText;
    public Transform Center;
    public Transform Indicator;
    public Transform Selector;
    public Transform InnerCircle1;
    public Transform InnerCircle2;
    public Collider2D SelectorCollider;
    public Collider2D IndicatorCollider;

    private float speed = 100f;
    private float circle1Speed = 25;
    private float circle2Speed = -8f;
    private float count = 5;

    public override void InitPuzzle(float difficulty)
    {
        count = Random.Range(2, 5);
        speed = Random.Range(100f, 200f);
        CountText.text = count.ToString();
        Indicator.RotateAround(Center.position, Vector3.forward, Random.Range(0.0f, 9999f));
    }

    public override void OnPuzzle1()
    {
        // puzzle completion condition: check if selector (red) is touching indicator (green)
        if (Physics2D.IsTouching(SelectorCollider, IndicatorCollider)) {
            count--;
            if(count <= 0) PuzzleManager.Instance.CompletePuzzle(index);
            Indicator.RotateAround(Center.position, Vector3.forward, Random.Range(0.0f, 9999f));
            if(speed > 0) speed += 20;
            if(speed < 0) speed -= 20;
            speed *= -1;
            CountText.text = count.ToString();
        }
    }

    public override void OnPuzzle2()
    {
        return;
    }

    public override int GetDifficulty()
    {
        return 3;
    }

    void Update() {
        Selector.RotateAround(Center.position, Vector3.forward, Time.deltaTime * speed);
        InnerCircle1.Rotate(new Vector3(0, 0, circle1Speed * Time.deltaTime), Space.Self);
        InnerCircle2.Rotate(new Vector3(0, 0, circle2Speed * Time.deltaTime), Space.Self);
    }
}
