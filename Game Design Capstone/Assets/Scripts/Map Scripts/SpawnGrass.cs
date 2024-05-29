using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrass : MonoBehaviour
{
    [SerializeField] GameObject grassPrefab;    // Grass prefab
    [SerializeField] int numberOfGrass = 100;   // Number of grass to spawn

    private Matrix4x4[] matrices;

    /*
     * Name: Start (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Initializes and updates matrices
     */
    void Start()
    {
        matrices = new Matrix4x4[numberOfGrass];
        UpdateMatrices();
    }

    /*
     * Name: Update (Unity)
     * Inputs: none
     * Outputs: none
     * Description: Draw grass instances
     */
    void Update()
    {
        DrawGrass();
    }

    /*
     * Name: UpdateMatrices
     * Inputs: none
     * Outputs: none
     * Description: Randomly places grass on the matrix
     */
    void UpdateMatrices()
    {
        for (int i = 0; i < numberOfGrass; i++)
        {
            Vector3 position = GetRandomPointOnPlane(transform); // Use the transform of the current GameObject
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), -90);
            float scale = Random.Range(0.8f, 1.2f);
            matrices[i] = Matrix4x4.TRS(position, rotation, Vector3.one * scale);
        }
    }

    /*
     * Name: DrawGrass
     * Inputs: none
     * Outputs: none
     * Description: Draws a grass instance
     */
    void DrawGrass()
    {
        Mesh mesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
        Material material = grassPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }

    /*
     * Name: GetRandomPointOnPlane
     * Inputs: transform of plane
     * Outputs: vector3 of randomized point
     * Description: Finds a random point on a plane
     */
    Vector3 GetRandomPointOnPlane(Transform planeTransform)
    {
        // Cast a ray from above the mesh downward to find a point on the surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit))
        {
            return hit.point;
        }

        // Default to the transform's position if no surface is hit
        return transform.position;

        // Get the bounds of the plane
        // Bounds bounds = planeTransform.GetComponent<MeshRenderer>().bounds;

        // // Calculate random positions within the bounds of the plane
        // float randomX = Random.Range(bounds.min.x, bounds.max.x);
        // float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // // Return a random point on the plane's surface
        // return new Vector3(randomX, planeTransform.position.y, randomZ);
    }
}
