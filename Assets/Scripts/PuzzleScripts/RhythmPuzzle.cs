
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RhythmPuzzle : Puzzle
{
    public RhythmPuzzleTrack[] trackList;
    RhythmPuzzleTrack track;
    public Image LTileTemplate, RTileTemplate;
    public Image line;

    public float speed;

    bool failed;
    bool completed;

    List<Image> LTiles, RTiles;
    Collider2D col;

    int tileCount = 0;
    int successCount = 0;

    public override void InitPuzzle(float difficulty)
    {
        track = trackList[Random.Range(0,trackList.Length)];

        col = line.GetComponent<Collider2D>();
        LTiles = new List<Image>();
        RTiles = new List<Image>();

        CreateTiles();
        foreach (bool t in track.trackL)
        {
            if (t) tileCount++;
        }
        foreach (bool t in track.trackR)
        {
            if (t) tileCount++;
        }
    }

    void CreateTiles()
    {
        foreach (Image tile in LTiles)
        {
            Destroy(tile.gameObject);
        }
        foreach (Image tile in RTiles)
        {
            Destroy(tile.gameObject);
        }
        LTiles.Clear();
        RTiles.Clear();
        for (int i = 0; i < track.trackL.Length; i++)
        {
            Image newTile;
            newTile = Instantiate(LTileTemplate);
            newTile.transform.SetParent(line.transform, false);
            newTile.rectTransform.anchoredPosition += new Vector2(0, newTile.rectTransform.rect.height * 1.05f * i);
            LTiles.Add(newTile);
            if (!track.trackL[i])
            {
                newTile.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
            }

            newTile = Instantiate(RTileTemplate);
            newTile.transform.SetParent(line.transform, false);
            newTile.rectTransform.anchoredPosition += new Vector2(0, newTile.rectTransform.rect.height * 1.05f * i);
            if (!track.trackR[i]) newTile.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
            RTiles.Add(newTile);
        }

        foreach (Image tile in LTiles)
        {
            tile.rectTransform.anchoredPosition += new Vector2(0, 1000);
        }
        foreach (Image tile in RTiles)
        {
            tile.rectTransform.anchoredPosition += new Vector2(0, 1000);
        }
    }

    private void Update()
    {
        if(!isActiveAndEnabled) return;
        foreach (Image tile in LTiles)
        {
            tile.rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
        }
        foreach (Image tile in RTiles)
        {
            tile.rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
        }

        if (LTiles[LTiles.Count - 1].rectTransform.anchoredPosition.y < -500)
        {
            if (failed || successCount < tileCount)
            {
                CreateTiles();
                failed = false;
                successCount = 0;
            }
            else if (!completed)
            {
                completed = true;
                StartCoroutine(Complete());
            }
        }
    }

    public override void OnPuzzle1()
    {
        CheckTile(true);
    }

    public override void OnPuzzle2()
    {
        CheckTile(false);
    }

    void CheckTile(bool isLeft)
    {
        bool[] trackSide;
        List<Image> tileList;
        if (isLeft)
        {
            trackSide = track.trackL;
            tileList = LTiles;
        }
        else
        {
            trackSide = track.trackR;
            tileList = RTiles;
        }
        foreach (Image tile in tileList)
        {
            Vector2 pos = tile.rectTransform.anchoredPosition;
            if (pos.y <= 0 && pos.y + tile.rectTransform.rect.height >= 0)
            {
                int i = tileList.IndexOf(tile);
                if (trackSide[i] && tile.enabled)
                {
                    successCount++;
                    tile.enabled = false;
                }
                else
                {
                    failed = true;
                    tile.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
                }
                
            }
        }

    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(1);
        PuzzleManager.Instance.CompletePuzzle(index);
    }

    public override int GetDifficulty()
    {
        return 6;
    }

}

