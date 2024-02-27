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
    [SerializeField] AudioData edSFX;
    private bool hasPlayerSFX = false;

    private void Awake()
    {
        waitForDuration = new WaitForSeconds(duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Metal") && !hasPlayerSFX)
        {
            door.SetActive(false);
            victoryUI.SetActive(true);
            AudioManager.Instance.PlayBGM(edSFX);
            hasPlayerSFX = true;
            StartCoroutine(nameof(CloseVictoryUI));
        }
    }

    IEnumerator CloseVictoryUI()
    {
        yield return waitForDuration;
        victoryUI.SetActive(false);
    }
}
