using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplode : MonoBehaviour
{
    [SerializeField] Material material;
    public int cubePerAxis = 8;
    public float force = 500f;
    public float radius = 3f;
    private int hits = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            hits++;
            if(hits >= 2)
            {
                CreateCubes();
                DoorOpened();
            }
        }
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

    void DoorOpened()
    {
        Debug.Log("Open");
        GameObject door = GameObject.Find("Door 1");
        door.gameObject.SetActive(false);
    }

    void CreateCube(Vector3 coordinate)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = material;

        cube.transform.localScale = transform.localScale / cubePerAxis;

        Vector3 firstCube = transform.position - transform.localScale / 2 + transform.localScale / 2;
        cube.transform.localPosition = firstCube + Vector3.Scale(coordinate, cube.transform.localScale);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);
    }
}
