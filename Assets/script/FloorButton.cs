using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Metal") || other.gameObject.CompareTag("Rubber") || other.gameObject.CompareTag("Player"))
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        door.SetActive(true);
    }
}
