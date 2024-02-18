using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject brokenVersion;
    public float breakVelocityThreshold = 7f;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {

        float velocity = rb.velocity.magnitude;

        if (velocity > breakVelocityThreshold)
        {
            Instantiate(brokenVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
