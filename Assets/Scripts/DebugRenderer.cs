using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer l1, l2, l3;

    public static LineRenderer lineRenderer1, lineRenderer2, lineRenderer3;
    private void Awake()
    {
        lineRenderer1 = l1;
        lineRenderer2 = l2;
        lineRenderer3 = l3;
    }
}
