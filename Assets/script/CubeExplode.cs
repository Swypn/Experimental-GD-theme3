using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplode : MonoBehaviour
{
    [Header("---Material---")]
    [SerializeField] Material transparentMaterial;
    [SerializeField] Material brokenMatrial;
    [SerializeField] Material originalMaterial;
    MeshRenderer meshRenderer;

    [Header("---Door---")]
    [SerializeField] GameObject door;

    [Header("---Audio---")]
    [SerializeField] AudioData crackingSFX;
    [SerializeField] AudioData breakSFX;
    [SerializeField] AudioData unlockSFX;

    [Header("---Explode Factors---")]
    public int cubePerAxis = 8;
    public int sizeFactor = 2;
    public float force = 500f;
    public float radius = 3f;
    private int hits = 0;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Metal"))
        {
            meshRenderer.material = brokenMatrial;
            hits++;
            if(hits == 1)
            {
                AudioManager.Instance.PlaySFX(crackingSFX);
            }
            if (hits >= 2)
            {
                CreateCubes();
                AudioManager.Instance.PlaySFX(breakSFX);
                DoorOpened();
                AudioManager.Instance.PlaySFX(unlockSFX);
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
                    StartCoroutine(CreateCube(new Vector3(x, y, z)));
                }
            }
        }
        meshRenderer.material = transparentMaterial;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void DoorOpened()
    {
        door.SetActive(false);
    }

    IEnumerator CreateCube(Vector3 coordinate)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = originalMaterial;

        cube.transform.localScale = transform.localScale / cubePerAxis / sizeFactor;

        Vector3 firstCube = transform.position - transform.localScale / 2 + transform.localScale / 2;
        cube.transform.localPosition = firstCube + Vector3.Scale(coordinate, cube.transform.localScale);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);

        yield return new WaitForSeconds(2.5f);
        Destroy(cube);

        yield return null;
        Destroy(gameObject);
    }
}
