using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    public List<RectTransform> Tiles;
    public RectTransform BottomLeft;
    public RectTransform BottomRight;
    public RectTransform UpperLeft;
    public RectTransform UpperRight;
    
    [Range(0.0f, 1.0f)]
    public float ParallaxIntensity;
}
