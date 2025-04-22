using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float sprintMultiplier = 1.5f;
    public float gravity = -9.81f;
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float interactRange = 3f; // How close the player must be to interact

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

	//public GameObject handUI; (added new)


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock cursor to the middle of the screen 
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
            velocity.y = -2f; // small downward force to stick to ground
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
        if (Input.GetButtonDown("Interact")) // Ensure you're using the correct "Interact" input
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange)) // Perform raycast
            {
                if (hit.collider.CompareTag("Reach")) // Ensure the raycast hits the chest
                {
                    UseChest chest = hit.collider.GetComponent<UseChest>();
                    if (chest != null)
                    {
                        chest.ActivateChest(); // Trigger chest opening
                    }
                }
            }
        }
    }

}

