using System;
using System.Collections.Generic;
using UnityEngine;

public class TrailMeshGenerator : MonoBehaviour
{
    public GameObject motorcycle;   // Reference to the player's motorcycle object.
    public float trailWidth = 0.5f; // Horizontal width of the trail.
    public float trailHeight = 1.0f; // Vertical height of the trail (wall height).
    public float stepRate = 0.1f;   // Time interval (in seconds) to add new trail segments.
    
    private Mesh trailMesh;  // The mesh that will be updated dynamically to create the trail.
    private List<Vector3> vertices = new List<Vector3>();  // Store vertices for the trail.
    private List<int> triangles = new List<int>();  // Store triangle indices.
    private float timeSinceLastStep = 0f;  // Timer to control when to add the next step.
    
    private MeshCollider meshCollider;

    private bool running = false;


    private void Awake()
    {
        running = false;
    }
    
    void ResetMesh()
    {
        trailMesh = new Mesh();
        trailMesh.name = gameObject.name + "Mesh";
        GetComponent<MeshFilter>().mesh = trailMesh;
        
        meshCollider = gameObject.GetComponent<MeshCollider>();
        
        meshCollider.sharedMesh = null;
        
        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        AddTrailSegment();
    }

    private void OnEnable()
    {
        GlobalEvents.PlayerLost += OnPlayerLost;
        GlobalEvents.RoundStart += OnRoundStart;
        GlobalEvents.CountdownEnd += OnCountdownEnd;
    }

    private void OnDisable()
    {
        GlobalEvents.PlayerLost -= OnPlayerLost;
        GlobalEvents.RoundStart -= OnRoundStart;
        GlobalEvents.CountdownEnd -= OnCountdownEnd;
    }

    void Update()
    {
        // Track time to add new trail segments based on the specified stepRate
        timeSinceLastStep += Time.deltaTime;

        if (running && timeSinceLastStep >= stepRate)
        {
            // Add a new trail segment every 'stepRate' seconds
            AddTrailSegment();
            UpdateMesh();
            timeSinceLastStep = 0f;  // Reset the timer
        }
    }

    // Adds a new segment to the trail based on the player's current position
    void AddTrailSegment()
    {
        Vector3 currentPos = motorcycle.transform.position;

        // Calculate the offset for the width (horizontal displacement to the left and right)
        Vector3 horizontalOffset = motorcycle.transform.right * trailWidth / 2;
        
        // Define the vertical offset for the height of the trail
        Vector3 verticalOffset = Vector3.up * trailHeight;

        // Add the 4 vertices for this segment (with height)
        Vector3 bottomLeft = currentPos - horizontalOffset;           // Bottom left
        Vector3 topLeft = currentPos - horizontalOffset + verticalOffset; // Top left
        Vector3 bottomRight = currentPos + horizontalOffset;          // Bottom right
        Vector3 topRight = currentPos + horizontalOffset + verticalOffset; // Top right

        // Append the 4 vertices (we always add new vertices in steps)
        vertices.Add(bottomLeft);   // Vertex 1
        vertices.Add(topLeft);      // Vertex 2
        vertices.Add(bottomRight);  // Vertex 3
        vertices.Add(topRight);     // Vertex 4

        // Calculate how many vertices we've added so far
        int vertexIndex = vertices.Count;

        // Add triangles: 2 triangles to form the rectangle (quad) for this section
        if (vertexIndex >= 8)
        {
            // Triangles for the quad (two triangles for each step)
            //Left side
            // Triangle 1
            triangles.Add(vertexIndex - 8); // Previous bottom-left
            triangles.Add(vertexIndex - 4); // Current bottom-left
            triangles.Add(vertexIndex - 7); // Previous bottom-right

            // Triangle 2
            triangles.Add(vertexIndex - 7); // Previous bottom-right
            triangles.Add(vertexIndex - 4); // Current bottom-left
            triangles.Add(vertexIndex - 3); // Current bottom-right //////

            //Right Side
            // Triangle 3
            triangles.Add(vertexIndex - 6); // Previous top-left
            triangles.Add(vertexIndex - 5); // Previous top-right
            triangles.Add(vertexIndex - 2); // Current top-left

            // Triangle 4
            triangles.Add(vertexIndex - 5); // Previous top-right
            triangles.Add(vertexIndex - 1); // Current top-right //////
            triangles.Add(vertexIndex - 2); // Current top-left
            
            // ** New Top Quad (2 triangles) **
            // Triangle 5 (covering top of trail)
            triangles.Add(vertexIndex - 3); // Current bottom-right
            triangles.Add(vertexIndex - 1); // Current top-right
            triangles.Add(vertexIndex - 7); // Previous bottom-right

            // Triangle 6 (covering top of trail)
            triangles.Add(vertexIndex - 7); // Previous bottom-right
            triangles.Add(vertexIndex - 1); // Current top-right
            triangles.Add(vertexIndex - 5); // Previous top-right
        }
    }

    // Updates the mesh with the new vertices and triangles
    void UpdateMesh()
    {
        // Clear the mesh so it can be updated
        trailMesh.Clear();

        // Assign the vertices and triangles to the mesh
        trailMesh.vertices = vertices.ToArray();
        trailMesh.triangles = triangles.ToArray();

        // Recalculate normals and bounds for correct rendering
        trailMesh.RecalculateNormals();
        trailMesh.RecalculateBounds();
        
        // Update the MeshCollider with the new mesh
        meshCollider.sharedMesh = null;  // Reset the mesh to ensure the update is applied
        meshCollider.sharedMesh = trailMesh;
    }

    void OnPlayerLost(Team _)
    {
        running = false;
        meshCollider.sharedMesh = null;
    }

    void OnRoundStart()
    {
        ResetMesh();
        timeSinceLastStep = 0f;
    }

    void OnCountdownEnd()
    {
        ResetMesh();
        running = true;
    }
}
