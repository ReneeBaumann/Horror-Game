using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float sprintMultiplier = 1.5f;
    public float gravity = -9.81f;
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float interactRange = 3f;

    public AudioClip[] woodFootstepSounds;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector3 velocity;
    private float xRotation = 0f;

    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>(); // Automatically get the attached AudioSource
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        RotateCamera();
        HandleInteraction();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * sprintMultiplier : speed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if ((moveX != 0 || moveZ != 0) && controller.isGrounded && !isWalking && !isFootstepCoroutineRunning)
        {
            isWalking = true;
            StartCoroutine(PlayFootstepSounds(1.3f / currentSpeed));
        }
        else if ((moveX == 0 && moveZ == 0) || !controller.isGrounded)
        {
            isWalking = false;
        }
    }

    IEnumerator PlayFootstepSounds(float delay)
    {
        isFootstepCoroutineRunning = true;

        while (isWalking)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                if (hit.collider.CompareTag("Wood") && woodFootstepSounds.Length > 0)
                {
                    int index = Random.Range(0, woodFootstepSounds.Length);
                    audioSource.clip = woodFootstepSounds[index];
                    audioSource.Play();
                }
            }

            yield return new WaitForSeconds(delay);
        }

        isFootstepCoroutineRunning = false;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleInteraction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                if (hit.collider.CompareTag("Reach"))
                {
                    UseChest chest = hit.collider.GetComponent<UseChest>();
                    if (chest != null)
                    {
                        chest.ActivateChest();
                    }
                }
            }
        }
    }
}
