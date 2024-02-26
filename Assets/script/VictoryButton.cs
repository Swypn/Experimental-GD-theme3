using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VictoryButton : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject victoryUI;
    [SerializeField] float duration = 3f;
    WaitForSeconds waitForDuration;

    private void Awake()
    {
        waitForDuration = new WaitForSeconds(duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Metal") || other.gameObject.CompareTag("Rubber") || other.gameObject.CompareTag("Player"))
        {
            door.SetActive(false);
            victoryUI.SetActive(true);
            StartCoroutine(nameof(CloseVictoryUI));
        }
    }

    IEnumerator CloseVictoryUI()
    {
        yield return waitForDuration;
        victoryUI.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        door.SetActive(true);
    }
}
