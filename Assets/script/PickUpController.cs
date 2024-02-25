using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    [Header("---Materials---")]
    [SerializeField] Material transparentMaterial;
    //[SerializeField] Material rubberMaterial;
    public Transform grapPosition;
    public Image chargeBarFill;
    public Image pickupIndicator;

    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;
    public float chargeSpeed = 10f;

    public float reachLenght = 2.0f;

    public AudioClip pickupSound;
    public AudioClip chargeSound;
    public AudioClip throwSound;

    Material originalMaterial;
    private GameObject heldObject = null;
    private float currentThrowForce;
    private bool isCharging = false;
    private AudioSource audioSource;

    private void Awake()
    {
        chargeBarFill.fillAmount = 0;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();

        if (isCharging && heldObject != null)
        {
            currentThrowForce += chargeSpeed * Time.deltaTime;
            currentThrowForce = Mathf.Min(currentThrowForce, maxThrowForce);

            chargeBarFill.fillAmount = (currentThrowForce - minThrowForce) / (maxThrowForce - minThrowForce);
        }

    }
    private void HandleInput()
    {
        RaycastHit hit;
        bool canPickUp = Physics.Raycast(transform.position, transform.forward, out hit, reachLenght) && hit.collider.CompareTag("Grabbable");
        pickupIndicator.enabled = canPickUp && heldObject == null;       

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!heldObject && canPickUp)
            {
                TryPickupObject(hit.collider.gameObject);
                pickupIndicator.enabled = false;
            }
            else
            {
                DropObject();

            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldObject)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;
            audioSource.PlayOneShot(chargeSound, 0.7f);
        }

        if (Input.GetKeyUp(KeyCode.Q) && isCharging)
        {
            ThrowObject();
            audioSource.Stop();
            audioSource.PlayOneShot(throwSound);
        }
    }

    private void TryPickupObject(GameObject objectToPickUp)
    {
        heldObject = objectToPickUp;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.transform.position = grapPosition.position;
        heldObject.transform.parent = grapPosition;
        originalMaterial = heldObject.GetComponent<MeshRenderer>().material;
        heldObject.GetComponent<MeshRenderer>().material = transparentMaterial;
        audioSource.PlayOneShot(pickupSound);
    }

    private void ThrowObject()
    {
        if (!heldObject) return;

        Rigidbody objectRigidbody = heldObject.GetComponent<Rigidbody>();
        if (objectRigidbody)
        {
            objectRigidbody.isKinematic = false;
            heldObject.transform.parent = null;
            float adjustedForce = currentThrowForce / objectRigidbody.mass;
            objectRigidbody.AddForce(transform.forward * adjustedForce, ForceMode.VelocityChange);
            heldObject.GetComponent<MeshRenderer>().material = originalMaterial;
        }

        ResetAfterAction();
    }

    private void DropObject()
    {
        if (!heldObject) return;
        
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.parent = null;
        heldObject.GetComponent<MeshRenderer>().material = originalMaterial;
        audioSource.Stop();
        ResetAfterAction();
    }

    private void ResetAfterAction()
    {
        heldObject = null;
        chargeBarFill.fillAmount = 0;
        currentThrowForce = 0;
        isCharging = false;
    }
}
