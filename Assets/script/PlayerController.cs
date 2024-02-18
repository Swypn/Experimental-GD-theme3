using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public Transform playerCamera;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    private float cameraPitch = 0.0f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip[] landingSounds;
    private bool wasInAir = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        groundedPlayer = IsGrounded();// Use OR to combine both checks
        Debug.Log("Is Grounded: " + groundedPlayer);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * Time.deltaTime * speed);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSource.PlayOneShot(jumpSound);
        }

        if (wasInAir && groundedPlayer)
        {
            // Play a random landing sound from the array
            AudioClip landingSound = landingSounds[Random.Range(0, landingSounds.Length)];
            audioSource.PlayOneShot(landingSound);
        }

        wasInAir = !IsGrounded();

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    bool IsGrounded()
    {
        float groundCheckDistance = 0.1f; // How far to check for the ground
        Vector3 groundCheckPosition = transform.position + new Vector3(0, -characterController.height / 2, 0); // Position at the bottom of the character
        float groundCheckRadius = characterController.radius * 0.9f; // Check sphere radius

        // Use ~0 to consider all layers in the ground check
        int layerMask = ~LayerMask.GetMask("Player");

        bool isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, layerMask, QueryTriggerInteraction.Ignore);
        return isGrounded;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 groundCheckPosition = transform.position + new Vector3(0, -characterController.height / 2, 0);
        float groundCheckRadius = characterController.radius * 0.9f;
        Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);
    }
}

