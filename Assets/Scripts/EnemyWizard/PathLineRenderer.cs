using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathLineRenderer : MonoBehaviour
{
    public Transform[] waypoints;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        // Safety check: Make sure we have at least 2 waypoints
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning("Not enough waypoints assigned to draw a path!");
            return;
        }

        // Set the number of positions in the Line Renderer
        lineRenderer.positionCount = waypoints.Length;

        // Update the Line Renderer positions
        for (int i = 0; i < waypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }

        // You can adjust width, material, etc. here
        // e.g., lineRenderer.startWidth = 0.3f;
        // lineRenderer.endWidth = 0.3f;
    }
}
