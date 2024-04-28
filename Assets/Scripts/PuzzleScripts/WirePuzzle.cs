using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WirePuzzle : Puzzle
{
    public Transform[] Wires;
    public Transform LeftSelect;
    public Transform RightSelect;
    public TextMeshProUGUI ScoreText;

    private int wiresConnected = 0;
    private HashSet<int> finishedColors = new HashSet<int>();
    private int[] selectedWires = new int[2];
    
    private Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow};

    private int[] wireColors;

    const int MIN_STEPS = 15;
    const int MAX_STEPS = 30;

    public override int GetDifficulty()
    {
        return 2;
    }

    public override void InitPuzzle(float difficulty)
    {
        selectedWires[0] = 0;
        selectedWires[1] = 4;
        wireColors = new int[Wires.Length];
        RandomizeWires();
        CheckWires();
    }

    public void RandomizeWires()
    {
        // Assign Default Colors
        for (int i = 0; i < colors.Length; i++)
        {
            Wires[i].GetComponent<Image>().color = colors[i];
            wireColors[i] = i;
            Wires[4+i].GetComponent<Image>().color = colors[i]; 
            wireColors[4+i] = i;
        }

        // Randomize Colors (Knuth Shuffle)
        for(int i = 0; i < 4; i++){
            int cur = Random.Range(i, 4);
            int temp = wireColors[i];
            wireColors[i] = wireColors[cur];
            wireColors[cur] = temp;

            Wires[i].GetComponent<Image>().color = colors[wireColors[i]];
            Wires[cur].GetComponent<Image>().color = colors[wireColors[cur]];            

            cur = Random.Range(4+i, 8);
            temp = wireColors[4+i];
            wireColors[4+i] = wireColors[cur];
            wireColors[cur] = temp;

            Wires[4+i].GetComponent<Image>().color = colors[wireColors[4+i]];
            Wires[cur].GetComponent<Image>().color = colors[wireColors[cur]];
        }
    }

    public override void OnPuzzle1()
    {
        if (selectedWires[0] < 3){
            selectedWires[0]++;
            LeftSelect.position = Wires[selectedWires[0]].position;
        }
        else{
            selectedWires[0] = 0;
            LeftSelect.position = Wires[selectedWires[0]].position;
        }
        CheckWires();
    }

    public override void OnPuzzle2()
    {
        if (selectedWires[1] < 7){
            selectedWires[1]++;
            RightSelect.position = Wires[selectedWires[1]].position;
        }
        else{
            selectedWires[1] = 4;
            RightSelect.position = Wires[selectedWires[1]].position;
        }
        CheckWires();
    }

    public void CheckWires()
    {
        if (!finishedColors.Contains(wireColors[selectedWires[0]]) && (wireColors[selectedWires[0]] == wireColors[selectedWires[1]])){
            wiresConnected++;
            finishedColors.Add(wireColors[selectedWires[0]]);
            Wires[selectedWires[0]].GetComponent<Image>().color = Color.black;
            Wires[selectedWires[1]].GetComponent<Image>().color = Color.black;
            if (wiresConnected == 4) PuzzleManager.Instance.CompletePuzzle(index);
        }
        ScoreText.text = (4 - wiresConnected).ToString();
    }
}
