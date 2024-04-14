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
}
