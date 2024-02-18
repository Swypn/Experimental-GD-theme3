using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    public GameObject victoryUI;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            victoryUI.SetActive(true);
            Time.timeScale = 0f; // Note: This will pause everything, including animations and UI interactions
            Debug.Log("Congratulations! Level Complete!");
        }
    }
}
