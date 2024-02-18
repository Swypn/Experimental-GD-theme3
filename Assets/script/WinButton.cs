using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinButton : MonoBehaviour
{
    public GameObject victoryUI; 
    public float interactionDistance = 2f;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && IsPlayerAimingAtButton())
        {
            WinGame();
        }
    }

    private bool IsPlayerAimingAtButton()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject) // Check if the raycast hit this button
            {
                return true;
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a throwable object
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        victoryUI.SetActive(true);
        Debug.Log("Congratulations! You've won!");
    }
}
