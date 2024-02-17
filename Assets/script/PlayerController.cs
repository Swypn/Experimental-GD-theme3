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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        bool isReallyGrounded = IsReallyGrounded();
        groundedPlayer = characterController.isGrounded || isReallyGrounded; // Use OR to combine both checks
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
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    bool IsReallyGrounded()
    {
        float extraHeight = 0.1f; // Extra height to check for ground
        RaycastHit hit;
        if (Physics.Raycast(characterController.bounds.center, Vector3.down, out hit, characterController.bounds.extents.y + extraHeight))
        {
            return hit.collider != null; // Ground is detected
        }
        return false; // Ground is not detected
    }
}

