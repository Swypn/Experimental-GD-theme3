using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public bool IsMetalBall => isMetalBall;

    [Header("---Materials---")]
    [SerializeField] Material transparentMaterial;
    [SerializeField] Material rubberMaterial;
    [SerializeField] Material metalMaterial;

    [Header("---PhysicMaterial---")]
    [SerializeField] PhysicMaterial rubber;
    [SerializeField] PhysicMaterial metal;
    [SerializeField] float rubberMass = 1;
    [SerializeField] float metalMass = 3;

    [Header("---Persistent Ball---")]
    [SerializeField] GameObject persistentBall;
    public Transform grapPosition;
    public Image chargeBarFill;
    public Image pickupIndicator;
    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;
    public float chargeSpeed = 10f;
    public float reachLenght = 2.0f;

    [Header("---Audio---")]
    [SerializeField] AudioData pickupSFX;
    [SerializeField] AudioData chargeSFX;
    [SerializeField] AudioData convertSFX;
    [SerializeField] AudioData teleportSFX;

    Material originalMaterial;
    Collider persistentBallCollider;
    Rigidbody persistentBallRb;
    MeshRenderer persistentBallRenderer;

    private bool isRKeyPressed = false;
    private GameObject heldObject = null;
    private float currentThrowForce;
    private bool isCharging = false;
    private bool isMetalBall = true;

    private void Awake()
    {
        chargeBarFill.fillAmount = 0;
        persistentBallCollider = persistentBall.GetComponent<Collider>();
        persistentBallRb = persistentBall.GetComponent<Rigidbody>();
        persistentBallRenderer = persistentBall.GetComponent<MeshRenderer>();
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
        bool canPickUp = Physics.Raycast(transform.position, transform.forward, out hit, reachLenght) && (hit.collider.CompareTag("Grabbable") 
            || hit.collider.CompareTag("Metal") || hit.collider.CompareTag("Rubber"));

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

        if (Input.GetKeyDown(KeyCode.Mouse2) && !heldObject)
        {
            isRKeyPressed = !isRKeyPressed;

            if(isRKeyPressed)
            {
                ChangeMaterial();
                AudioManager.Instance.PlaySFX(convertSFX);
            }
            else
            {
                RestoreMaterial();
                AudioManager.Instance.PlaySFX(convertSFX);
            }

            /*if (isRKeyPressed && !heldObject && canPickUp)
            {
                ChangePhysicsMaterial(hit.collider.gameObject);
                ChangeMeshRendererMaterial(hit.collider.gameObject);
            }
            else if(!isRKeyPressed && !heldObject && canPickUp)
            {
                RestorePhysicsMaterial(hit.collider.gameObject);
                RestoreMeshRendererMaterial(hit.collider.gameObject);
            }*/
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Instantiate(metalBall, grapPosition.position, Quaternion.identity);
            persistentBall.transform.position = grapPosition.position;
            persistentBallRb.velocity = Vector3.zero;
            AudioManager.Instance.PlaySFX(teleportSFX);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && heldObject)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;
            AudioManager.Instance.PlaySFX(chargeSFX);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && isCharging)
        {
            ThrowObject();
            AudioManager.Instance.Shutdown();
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
        AudioManager.Instance.PlaySFX(pickupSFX);
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
        ResetAfterAction();
    }

    private void ResetAfterAction()
    {
        heldObject = null;
        chargeBarFill.fillAmount = 0;
        currentThrowForce = 0;
        isCharging = false;
    }

    //For persistentaBall
    private void ChangeMaterial()
    {
        persistentBallRenderer.material = rubberMaterial;
        persistentBallRb.mass = rubberMass;
        persistentBallCollider.material = rubber;
        persistentBall.tag = "Rubber";
        isMetalBall = false;
    }

    private void RestoreMaterial()
    {
        persistentBallRenderer.material = metalMaterial;
        persistentBallRb.mass = metalMass;
        persistentBallCollider.material = metal;
        persistentBall.tag = "Metal";
        isMetalBall = true;
    }



    //For general GameObject
    //private void ChangePhysicsMaterial(GameObject obj)
    //{
    //    Collider objectCollider = obj.GetComponent<Collider>();
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();
    //    if (objectCollider != null)
    //    {
    //        objectCollider.material = rubber;
    //    }

    //    if (rb != null)
    //    {
    //        rb.mass = rubberMass;
    //    }
    //}

    //private void RestorePhysicsMaterial(GameObject obj)
    //{
    //    Collider objectCollider = obj.GetComponent<Collider>();
    //    Rigidbody rb = obj.GetComponent<Rigidbody>();

    //    if (objectCollider != null)
    //    {
    //        objectCollider.material = metal;
    //    }

    //    if (rb != null)
    //    {
    //        rb.mass = metalMass;
    //    }
    //}

    //private void ChangeMeshRendererMaterial(GameObject obj)
    //{
    //    MeshRenderer objectRenderer = obj.GetComponent<MeshRenderer>();
    //    if (objectRenderer != null)
    //    {
    //        objectRenderer.material = rubberMaterial;
    //    }
    //}

    //private void RestoreMeshRendererMaterial(GameObject obj)
    //{
    //    MeshRenderer objectRenderer = obj.GetComponent<MeshRenderer>();
    //    if (objectRenderer != null)
    //    {
    //        objectRenderer.material = metalMaterial;
    //    }
    //}
}
