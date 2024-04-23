using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public Puzzle TopLeftPuzzle;
    public Puzzle TopMiddlePuzzle;
    public Puzzle TopRightPuzzle;
    public Puzzle BottomLeftPuzzle;
    public Puzzle BottomMiddlePuzzle;
    public Puzzle BottomRightPuzzle;

    private Puzzle[] puzzles;
    int puzzleCount = 0;

    

    public Puzzle[] puzzleTemplates;

    [SerializeField] private float puzzleChancePerDamage = 0.1f;
    private int maxDifficulty;
    [SerializeField]  private int baseMaxDifficulty = 12;

    float puzzleActiveTime = 0;
    float activePuzzleDifficultyTickTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        maxDifficulty = baseMaxDifficulty;

        if(Instance == null) {
            Instance = this;

        }
        else {
            Debug.LogWarning("Tried to create more than one instance of PuzzleManager");
            Destroy(this);
        }

        puzzles = new Puzzle[] {
            TopLeftPuzzle, TopMiddlePuzzle, TopRightPuzzle,
            BottomLeftPuzzle, BottomMiddlePuzzle, BottomRightPuzzle
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivePuzzleCount() > 0)
        {
            puzzleActiveTime += Time.deltaTime;
        }
        else
        {
            puzzleActiveTime = 0;
            maxDifficulty = baseMaxDifficulty;
        }
        if (puzzleActiveTime >= activePuzzleDifficultyTickTime)
        {
            puzzleActiveTime = 0;
            maxDifficulty += 2;
            Debug.Log($"max difficulty ticked, now {maxDifficulty}");
        }
    }

    public void TriggerPuzzle1()
    {
        foreach (Puzzle puzzle in puzzles)
        {
            if (puzzle != null)
            {
                puzzle.OnPuzzle1();
            }
        }
    }

    public void TriggerPuzzle2()
    {
        foreach (Puzzle puzzle in puzzles)
        {
            if (puzzle != null)
            {
                puzzle.OnPuzzle2();
            }
        }
    }

    public void RollForPuzzle(int damage)
    {
        if (GetTotalDifficulty() >= maxDifficulty - 1) return;
        float roll = Random.value;
        if (puzzleCount >= 3) roll *= 2;
        if (roll < damage * puzzleChancePerDamage)
        {
            SpawnPuzzle();
        }
    }

    public void SpawnPuzzle()
    {
        List<int> availableSpaces = new List<int>();
       


        for (int i = 0; i < puzzles.Length; i++)
        {
            if (puzzles[i] == null)
            {
                availableSpaces.Add(i);
            }
        }
        if (availableSpaces.Count == 0) return;
        int puzzleIndex = availableSpaces[Random.Range(0,availableSpaces.Count)];

        bool puzzleCreated = false;
        Puzzle selectedPuzzle = null;
        while (!puzzleCreated)
        {
            selectedPuzzle = puzzleTemplates[Random.Range(0, puzzleTemplates.Length)];
            puzzleCreated = selectedPuzzle.GetDifficulty() + GetTotalDifficulty() <= maxDifficulty;
            if (!puzzleCreated) Debug.Log($"Tried to create puzzle of difficulty {selectedPuzzle.GetDifficulty()} but would exceed max difficulty of {maxDifficulty} (current: {GetTotalDifficulty()})");
        }
        
        Puzzle newPuzzle = Instantiate(selectedPuzzle);
        newPuzzle.transform.SetParent(transform, false);
        // todo: choose puzzle based on difficulty
        newPuzzle.SetPosition(puzzleIndex);
        newPuzzle.InitPuzzle(1f);
        puzzles[puzzleIndex] = newPuzzle;
        puzzleCount++;
    }

    public void CompletePuzzle(int index)
    {
        Destroy(puzzles[index].gameObject);
        puzzles[index] = null;
        puzzleCount--;
    }

    int GetTotalDifficulty()
    {
        int total = 0;
        foreach (Puzzle puzzle in puzzles)
        {
            if (puzzle != null) total += puzzle.GetDifficulty();
        }
        return total;
    }

    public int ActivePuzzleCount()
    {
        int count = 0;
        foreach (Puzzle puzzle in puzzles)
        {
            if (puzzle != null) count++;
        }
        return count;
    }

}
