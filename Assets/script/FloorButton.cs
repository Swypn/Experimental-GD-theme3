using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] AudioData unlockSfx;
    private bool hasPlayerSFX = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Rubber"))
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Metal") && !hasPlayerSFX)
        {
            door.SetActive(false);
            hasPlayerSFX = true;
            AudioManager.Instance.PlaySFX(unlockSfx);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        door.SetActive(true);
        hasPlayerSFX = false;
    }
}
