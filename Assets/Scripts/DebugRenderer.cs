using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer l1, l2, l3;
    public bool drawVelocityLine, drawLiftLine;


    public static LineRenderer lineRenderer1, lineRenderer2, lineRenderer3;
    private void Awake()
    {
        lineRenderer1 = l1;
        lineRenderer2 = l2;
        lineRenderer3 = l3;

        l1.enabled = drawVelocityLine;
        l2.enabled = drawLiftLine;

    }
}
