using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlappyBirdPuzzle : Puzzle
{
    public FlappyBirdPipe TemplatePipe;
    public RectTransform Mask;
    public RectTransform DespawnPos;
    public RectTransform ScoreCountPos;
    public RectTransform Player; 
    public TextMeshProUGUI ScoreText;

    private float lastSpawnTime;
    private float spawnCooldown;
    private float fallSpeed;
    private float score;

    private List<FlappyBirdPipe> pipes;
    private const float SPEED = 125f;
    private const float FASTEST_SPAWN_TIME = 1.5f;
    private const float SLOWEST_SPAWN_TIME = 3f;
    private const float GRAVITY = 1000f;

    public override int GetDifficulty()
    {
        return 3;
    }

    public override void InitPuzzle(float difficulty)
    {
        pipes = new List<FlappyBirdPipe>();
        spawnCooldown = 0;
        fallSpeed = 0;
        score = Random.Range(3, 7);
        UpdateScore();
    }

    public override void OnPuzzle1()
    {
        fallSpeed = 350f;
    }

    public override void OnPuzzle2()
    {
        OnPuzzle1();
    }

    // Update is called once per frame
    void Update()
    {
        fallSpeed -= GRAVITY*Time.deltaTime; //?
        float playerY = Player.transform.localPosition.y + (fallSpeed*Time.deltaTime);
        if(playerY < -230) playerY = -230;
        if(playerY > 230) playerY = 230;

        Player.transform.localPosition = new Vector3(
            Player.transform.localPosition.x,
            playerY,
            Player.transform.localPosition.z
        );

        if(Time.time - lastSpawnTime > spawnCooldown) {
            SpawnPipe();
            lastSpawnTime = Time.time;
            spawnCooldown = Random.Range(FASTEST_SPAWN_TIME, SLOWEST_SPAWN_TIME);
        }

        for(int i = 0; i < pipes.Count; i++) {
            FlappyBirdPipe pipe = pipes[i];
            pipe.transform.localPosition -= new Vector3(SPEED, 0, 0) * Time.deltaTime;
            if(pipe.transform.localPosition.x < DespawnPos.localPosition.x) {
                pipes.Remove(pipe);
                i--;
                Destroy(pipe.gameObject);
            }

            if(pipe.transform.localPosition.x < ScoreCountPos.localPosition.x
                && !pipe.Collided && ! pipe.CountedScoreYet) {

                pipe.CountedScoreYet = true;
                score--;
                UpdateScore();
            }
        }
    }

    private void SpawnPipe() {
        FlappyBirdPipe pipe = Instantiate(TemplatePipe);

        pipe.transform.SetParent(Mask);
        pipe.transform.localScale = TemplatePipe.transform.localScale;
        pipe.transform.localPosition = TemplatePipe.transform.localPosition;

        float pipeDistance = 2.0f;
        float upperHeight = Random.Range(0.5f, 5.4f-2.0f-0.5f);
        float lowerHeight = 5.4f-upperHeight-pipeDistance;

        pipe.PipeUpper.localScale = new Vector3(
            TemplatePipe.PipeUpper.localScale.x,
            upperHeight,
            TemplatePipe.PipeLower.localScale.z
        );
        pipe.PipeLower.localScale = new Vector3(
            TemplatePipe.PipeLower.localScale.x,
            lowerHeight,
            TemplatePipe.PipeLower.localScale.z
        );

        pipe.UpperCollider.size = pipe.PipeUpper.sizeDelta;
        pipe.LowerCollider.size = pipe.PipeLower.sizeDelta;

        pipe.UpperCollider.offset = new Vector2(0, -50);
        pipe.LowerCollider.offset = new Vector2(0, 50);

        pipes.Add(pipe);
    }

    private void UpdateScore() {
        if(score <= 0) PuzzleManager.Instance.CompletePuzzle(index);
        ScoreText.text = score.ToString();
    }
}