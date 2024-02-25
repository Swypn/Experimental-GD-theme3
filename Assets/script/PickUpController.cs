using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    [Header("---Materials---")]
    [SerializeField] Material transparentMaterial;
    [SerializeField] Material rubberMaterial;
    [SerializeField] Material metalMaterial;

    [Header("---PhysicMaterial---")]
    [SerializeField] PhysicMaterial rubber;
    [SerializeField] PhysicMaterial metal;

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
    PhysicMaterial originalPhysicMaterial;
    private float rubberMass = 1;
    private float metalMass = 3;
    private bool isRKeyPressed = false;
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            isRKeyPressed = !isRKeyPressed;

            if (isRKeyPressed && !heldObject && canPickUp)
            {
                ChangePhysicsMaterial(hit.collider.gameObject);
                ChangeMeshRendererMaterial(hit.collider.gameObject);
            }
            else if(!isRKeyPressed && !heldObject && canPickUp)
            {
                RestorePhysicsMaterial(hit.collider.gameObject);
                RestoreMeshRendererMaterial(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && heldObject)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;
            audioSource.PlayOneShot(chargeSound, 0.7f);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && isCharging)
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

    private void ChangePhysicsMaterial(GameObject obj)
    {
        Collider objectCollider = obj.GetComponent<Collider>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (objectCollider != null)
        {
            objectCollider.material = rubber;
        }

        if (rb != null)
        {
            rb.mass = rubberMass;
        }
    }

    private void RestorePhysicsMaterial(GameObject obj)
    {
        Collider objectCollider = obj.GetComponent<Collider>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (objectCollider != null)
        {
            objectCollider.material = metal;
        }

        if (rb != null)
        {
            rb.mass = metalMass;
        }
    }

    private void ChangeMeshRendererMaterial(GameObject obj)
    {
        MeshRenderer objectRenderer = obj.GetComponent<MeshRenderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material = rubberMaterial;
        }
    }

    private void RestoreMeshRendererMaterial(GameObject obj)
    {
        MeshRenderer objectRenderer = obj.GetComponent<MeshRenderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material = metalMaterial;
        }
    }
}
