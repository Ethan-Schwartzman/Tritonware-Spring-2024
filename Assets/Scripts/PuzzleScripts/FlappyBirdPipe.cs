using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdPipe : MonoBehaviour {
    public RectTransform PipeUpper;
    public RectTransform PipeLower;
    public Collider2D PlayerCollider;
    public BoxCollider2D UpperCollider;
    public BoxCollider2D LowerCollider;

    public bool CountedScoreYet = false;
    public bool Collided = false;

    void FixedUpdate() {
        if (Physics2D.IsTouching(PlayerCollider, UpperCollider)) {
            Collided = true;
        }
        if (Physics2D.IsTouching(PlayerCollider, LowerCollider)) {
            Collided = true;
        }
    }
}
