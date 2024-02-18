using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public Transform grapPosition;
    public Image chargeBarFill;

    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;
    public float chargeSpeed = 10f;

    private GameObject heldObject = null;
    private float currentThrowForce;
    private bool isCharging = false;

 

    void Update()
    {
        HandleInput();

        if (isCharging && heldObject != null)
        {
            currentThrowForce += chargeSpeed * Time.deltaTime;
            currentThrowForce = Mathf.Min(currentThrowForce, maxThrowForce);

            chargeBarFill.fillAmount = (currentThrowForce - minThrowForce) / (maxThrowForce - minThrowForce);
        }

        else if (!isCharging)
        {
            // Reset the charge bar when not charging
            chargeBarFill.fillAmount = 0;
        }

    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }

        // Start charging when Q is pressed
        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;
        }

        // Throw the object when Q is released
        if (Input.GetKeyUp(KeyCode.Q) && isCharging)
        {
            isCharging = false;
            ThrowObject();
        }
    }

    private void TryPickupObject()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 2f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); // To check what the raycast hits
            if (hit.collider.gameObject.CompareTag("Grabbable"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                heldObject.transform.position = grapPosition.position; // Move to grab point
                heldObject.transform.parent = grapPosition; // Parent to the grab point
                Debug.Log("Picked up: " + hit.collider.name);
            }
        }
    }

    private void ThrowObject()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.parent = null;
        heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * currentThrowForce, ForceMode.VelocityChange);
        heldObject = null;
        currentThrowForce = 0;
    }

    private void DropObject()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.parent = null;
        heldObject = null;
    }
}
