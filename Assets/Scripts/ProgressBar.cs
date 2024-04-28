using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image segment;

    public List<RectTransform> allSegments = new List<RectTransform>();
    float initialWidth;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialWidth = rectTransform.rect.width;
    }

    public void SetLevel(float level)
    {
        if (rectTransform != null) rectTransform.sizeDelta = new Vector2(level * initialWidth, 0);
    }

    public void SetSegments(int segments)
    {
        foreach (RectTransform segment in allSegments)
        {
            segment.gameObject.SetActive(false);
            Destroy(segment.gameObject);
        }
        allSegments.Clear();
        for (int i = 1; i < segments; i++)
        {
            RectTransform rect = Instantiate(segment).rectTransform;
            rect.SetParent(transform, false);
            rect.anchoredPosition = new Vector2((initialWidth / segments) * i, 0);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rectTransform.rect.height);
            allSegments.Add(rect);
        }
    }

    public float GetLevel()
    {
        return rectTransform.sizeDelta.x;
    }
}
