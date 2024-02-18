using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject brokenVersion;
    public float breakForceThreshold = 50f;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        float impactForce = collision.relativeVelocity.magnitude * (collision.rigidbody != null ? collision.rigidbody.mass : 1);

        if (impactForce > breakForceThreshold)
        {
            Instantiate(brokenVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
