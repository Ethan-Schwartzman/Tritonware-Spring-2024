using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdPuzzle : Puzzle
{
    public RectTransform PipeUpper;
    public RectTransform PipeLower;
    public RectTransform Mask;
    public RectTransform DespawnPos;

    private float lastSpawnTime;
    private float spawnCooldown;

    private List<RectTransform> pipes;
    private const float SPEED = 75f;
    private const float FASTEST_SPAWN_TIME = 1.5f;
    private const float SLOWEST_SPAWN_TIME = 3f;

    public override int GetDifficulty()
    {
        return 3;
    }

    public override void InitPuzzle(float difficulty)
    {
        pipes = new List<RectTransform>();
        spawnCooldown = 0;
    }

    public override void OnPuzzle1()
    {
        SpawnPipe();
    }

    public override void OnPuzzle2()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime > spawnCooldown) {
            SpawnPipe();
            lastSpawnTime = Time.time;
            spawnCooldown = Random.Range(FASTEST_SPAWN_TIME, SLOWEST_SPAWN_TIME);
        }

        for(int i = 0; i < pipes.Count; i++) {
            RectTransform pipe = pipes[i];
            pipe.transform.localPosition -= new Vector3(SPEED, 0, 0) * Time.deltaTime;
            if(pipe.transform.localPosition.x < DespawnPos.localPosition.x) {
                pipes.Remove(pipe);
                i--;
                Destroy(pipe.gameObject);
            }
        }
    }

    private void SpawnPipe() {
        RectTransform upper = Instantiate(PipeUpper);
        RectTransform lower = Instantiate(PipeLower);

        upper.SetParent(Mask);
        lower.SetParent(Mask);

        upper.position = PipeUpper.position;
        lower.position = PipeLower.position;

        float pipeDistance = 2.0f;
        float upperHeight = Random.Range(0.5f, 5.4f-2.0f-0.5f);
        float lowerHeight = 5.4f-upperHeight-pipeDistance;
        upper.localScale = new Vector3(
            PipeUpper.localScale.x,
            upperHeight,
            PipeUpper.localScale.z
        );
        lower.localScale = new Vector3(
            PipeLower.localScale.x,
            lowerHeight,
            PipeLower.localScale.z
        );

        pipes.Add(upper);
        pipes.Add(lower);
    }
}
