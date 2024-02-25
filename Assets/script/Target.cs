using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("yes");
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Metal") || other.gameObject.CompareTag("Rubber"))
        {
            door.SetActive(false);
            gameObject.SetActive(false); // Deactivate the target

            // Notify the ThrowingLevelManager that this target was hit
            //ThrowingChallengeManager manager = FindObjectOfType<ThrowingChallengeManager>();
            //if (manager != null)
            //{
            //    manager.TargetHit(gameObject);
            //}
        }
    }
}
