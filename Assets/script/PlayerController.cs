using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 5.0f;
    public float mouseSensitivity = 1.0f;
    public Transform playerCamera;
    public float gravity = -9.81f;
    public float groundCheckRadius = 0.2f;
    //public float jumpHeight = 2.0f;
    public Scrollbar sensitivityScrollbar;
    bool isPaused = false;
    bool ableToMove = true;

    private float cameraPitch = 0.0f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    //private AudioSource audioSource;
    //public AudioClip jumpSound;
    //public AudioClip[] landingSounds;
    //private bool wasInAir = false;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(ableToMove)
        {
            Move();
        }

        ShowScrollBar();
        ExitGame();
    }

    private void Move()
    {
        groundedPlayer = IsGrounded();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!groundedPlayer)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * Time.deltaTime * speed);

        //if (Input.GetButtonDown("Jump") && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        //    audioSource.PlayOneShot(jumpSound);
        //}

        //if (wasInAir && groundedPlayer)
        //{
        //    AudioClip landingSound = landingSounds[Random.Range(0, landingSounds.Length)];
        //    audioSource.PlayOneShot(landingSound);
        //}

        //wasInAir = !IsGrounded();

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    void ShowScrollBar()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            sensitivityScrollbar.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            ableToMove = false;
        }
        else if(Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            sensitivityScrollbar.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            ableToMove = true;
        }
    }

    void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    bool IsGrounded()
    {
        Vector3 groundCheckPosition = transform.position + new Vector3(0, -characterController.height / 2, 0); // Position at the bottom of the character

        // Use ~0 to consider all layers in the ground check
        int layerMask = ~LayerMask.GetMask("Player");

        bool isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, layerMask, QueryTriggerInteraction.Ignore);
        return isGrounded;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 groundCheckPosition = transform.position + new Vector3(0, -characterController.height / 2, 0);
        Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);
    }

    public void SetMouseSensitivity()
    {
        float mappedSensitivity = Mathf.Lerp(100f, 1000f, sensitivityScrollbar.value);
        mouseSensitivity = mappedSensitivity;
    }
    
    
}

