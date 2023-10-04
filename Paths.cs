using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paths : MonoBehaviour
{
    private Rigidbody rb;

    public enum MovementPath { Square, Circle, FigureEight };
    public MovementPath path;

    public float radius = 5.0f; //for the circle and figure-eight
    public float speed = 0f;

    public bool renderPath = false;
    public int renderPathResolution = 1000;
    public float renderPathHeight = 1.0f;

    private float timeElapsed = 0.0f;
    
    private LineRenderer lineRenderer;
    private List<Vector3> pathPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
         rb = GetComponent<Rigidbody>();

         if (renderPath) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
         
            lineRenderer.material = new Material(Shader.Find("Standard"));
            lineRenderer.positionCount = renderPathResolution;
            lineRenderer.widthMultiplier = 0.1f;  // Adjust the width as necessary

            switch (path) {
                case MovementPath.Square:

                    break;
                
                case MovementPath.Circle:
                    float stepSize = 2 * Mathf.PI / renderPathResolution;
                    float curTheta = 0.0f;

                    List<Vector3> pathPositions = new List<Vector3>();

                    while (curTheta < 2 * Mathf.PI) {
                        Vector3 newPosition = new Vector3(0.0f, 0.0f, 0.0f);
                        newPosition.x = radius * Mathf.Cos(curTheta);
                        newPosition.y = renderPathHeight;
                        newPosition.z = radius * Mathf.Sin(curTheta);

                        pathPositions.Add(newPosition);

                        curTheta += stepSize;
                    }

                    Debug.Log(pathPositions.ToArray());
                    
                    lineRenderer.SetPositions(pathPositions.ToArray());

                    

                    break;

            }
         }
         
    }

    // Update is called once per frame
    private void Update()
    {
        timeElapsed += Time.deltaTime * speed;

        Vector3 newPosition = new Vector3(0.0f, 2.0f, 0.0f);

        switch (path)
        {
            case MovementPath.Square:
                float t = timeElapsed % 4;  // This will give a value between 0 and 4
                if (t < 1) { // Bottom side (from left to right)
                    newPosition.x = Mathf.Lerp(-radius / 2, radius / 2, t);
                    newPosition.z = -radius / 2;
                }
                else if (t < 2) { // Right side (from bottom to top)
                    newPosition.x = radius / 2;
                    newPosition.z = Mathf.Lerp(-radius / 2, radius / 2, t - 1);
                }
                else if (t < 3) { // Top side (from right to left)
                    newPosition.x = Mathf.Lerp(radius / 2, -radius / 2, t - 2);
                    newPosition.z = radius / 2;
                }
                else { // Left side (from top to bottom)
                    newPosition.x = -radius / 2;
                    newPosition.z = Mathf.Lerp(radius / 2, -radius / 2, t - 3);
                }
                break;

            case MovementPath.Circle:
                newPosition.x = radius * Mathf.Cos(timeElapsed);
                newPosition.z = radius * Mathf.Sin(timeElapsed);
                break;

            case MovementPath.FigureEight:
                newPosition.x = radius * Mathf.Sin(timeElapsed);
                newPosition.z = radius * Mathf.Sin(timeElapsed) * Mathf.Cos(timeElapsed);
                break;
        }

        transform.position = newPosition;

        // // Update LineRenderer to trace the path
        // pathPositions.Add(newPosition);
        // lineRenderer.positionCount = pathPositions.Count;
        // lineRenderer.SetPositions(pathPositions.ToArray());
    }
}
