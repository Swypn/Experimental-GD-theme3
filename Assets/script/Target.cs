using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            gameObject.SetActive(false); // Deactivate the target

            // Notify the ThrowingLevelManager that this target was hit
            ThrowingChallengeManager manager = FindObjectOfType<ThrowingChallengeManager>();
            if (manager != null)
            {
                manager.TargetHit();
            }
        }
    }
}
