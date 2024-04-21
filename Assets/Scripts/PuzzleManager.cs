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

    public Puzzle[] puzzleTemplates;

    // Start is called before the first frame update
    void Start()
    {
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
        Puzzle newPuzzle = Instantiate(puzzleTemplates[Random.Range(0, puzzleTemplates.Length)]);
        newPuzzle.transform.SetParent(transform, false);
        // todo: choose puzzle based on difficulty
        newPuzzle.SetPosition(puzzleIndex);
        newPuzzle.InitPuzzle(1f);
        puzzles[puzzleIndex] = newPuzzle;

    }

    public void CompletePuzzle(int index)
    {
        Destroy(puzzles[index].gameObject, 0.01f);
        puzzles[index] = null;
    }

}
