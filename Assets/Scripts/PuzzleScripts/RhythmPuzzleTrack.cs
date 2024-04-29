using UnityEngine;

[CreateAssetMenu(fileName = "RhythmPuzzleTrack", menuName = "ScriptableObjects/RhythmPuzzleTrack", order = 1)]
public class RhythmPuzzleTrack : ScriptableObject
{
    public bool[] trackL;
    public bool[] trackR;
}