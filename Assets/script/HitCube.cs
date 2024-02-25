using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCube : MonoBehaviour
{
    [SerializeField] GameObject brokenPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Metal"))
        {
            Instantiate(brokenPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
