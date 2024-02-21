using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplode : MonoBehaviour
{
    public int cubePerAxis = 8;
    public float delay = 0.5f;
    public float force = 500f;
    public float radius = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("CreateCubes", delay);
    }

    void CreateCubes()
    {
        for (int x = 0; x < cubePerAxis; x++)
        {
            for (int y = 0; y < cubePerAxis; y++)
            {
                for (int z = 0; z < cubePerAxis; z++)
                {
                    CreateCube(new Vector3(x, y, z));
                }
            }
        }
        Destroy(gameObject);
    }

    void CreateCube(Vector3 coordinate)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
    }
}
