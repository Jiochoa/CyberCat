using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{

    public LineRenderer myLineRenderer;

    private void Awake()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine (Vector3 startPoint, Vector3 endPoint)
    {
        myLineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        myLineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        myLineRenderer.positionCount = 0;
    }


}
