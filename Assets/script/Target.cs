using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] AudioData unlockSFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Metal") || other.gameObject.CompareTag("Rubber"))
        {
            door.SetActive(false);
            gameObject.SetActive(false); 
            AudioManager.Instance.PlaySFX(unlockSFX);

            // Notify the ThrowingLevelManager that this target was hit
            //ThrowingChallengeManager manager = FindObjectOfType<ThrowingChallengeManager>();
            //if (manager != null)
            //{
            //    manager.TargetHit(gameObject);
            //}
        }
    }
}
