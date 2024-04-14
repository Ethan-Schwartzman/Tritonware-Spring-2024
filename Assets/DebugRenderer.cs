using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer l1;
    [SerializeField] LineRenderer l2;

    public static LineRenderer lineRenderer1, lineRenderer2;
    private void Awake()
    {
        lineRenderer1 = l1;
        lineRenderer2 = l2;
    }
}
