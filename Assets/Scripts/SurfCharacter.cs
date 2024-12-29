using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

    public class SurfCharacter : MonoBehaviour
    {
        public static Inventory Inventory;
        [Header("Interact")]
        public LayerMask layerMask;
        public float rayDistance = 100f;
        public static KeyManager key;
        GameObject interactobject;
        Interact interact;
        [Header("Camera Settings")]
        public float mouseSensitivity = 150f;
        public float maxLookAngle = 90f;
        public float cameraSmoothSpeed = 5f;
        public float defaultFOV = 60f;
        public float sprintFOV = 75f;
        public float fovTransitionSpeed = 5f;
        public float tiltAngle = 10f;
        public float tiltSpeed = 5f;

        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 10f;
        public float speedTransitionRate = 10f;
        public float smoothAcceleration = 10f;

        [Header("Jump Settings")]
        public float jumpForce = 5f;
        public LayerMask groundLayer;
        public float groundCheckDistance = 0.2f;
        public float airControlFactor = 0.5f;

        [Header("Head Bobbing")]
        public float walkBobSpeed = 10f;
        public float walkBobAmount = 0.05f;
        public float sprintBobSpeed = 15f;
        public float sprintBobAmount = 0.1f;
        public float smoothReturnSpeed = 5f;
        public float jumpCameraOffset = 0.2f;
        public float fallCameraOffset = 0.2f;

        private Rigidbody rb;
        private Vector3 moveDirection;
        private Camera playerCamera;
        private float currentSpeed;
        private float verticalLookRotation;
        private float defaultYPosition;
        private float headBobTimer;

        private bool isGrounded;
        private bool isJumping;

        private void Awake()
        {
        Inventory = GetComponent<Inventory>();
        key = GetComponent<KeyManager>();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            playerCamera = GetComponentInChildren<Camera>();
            defaultYPosition = playerCamera.transform.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovementInput();
            HandleJumpInput();
            HandleFOVAndTilt();
            HandleHeadBobbing();
            Interact();

        }
    void Interact()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            if (interactobject != hit.collider.gameObject)
            {
                if (interact != null) interact.interact.ExecuteUnhover();
                interactobject = hit.collider.gameObject;
                interact = interactobject.GetComponent<Interact>();
                interact.interact.ExecuteHover();
            }
            else if (interactobject != null && interact != null)
            {
                interact.interact.ExecutePresshover();
                if (Input.GetKeyDown(key.Interact))
                {
                    interact.interact.ExecuteClick();
                }
                if (Input.GetKeyUp(key.Interact))
                {
                    interact.interact.ExecuteUnclick();
                }
                if (Input.GetKey(key.Interact))
                {
                    interact.interact.ExecutePress();
                }
            }
        }
        else
        {
            if (interact != null) interact.interact.ExecuteUnhover();
            interactobject = null;
            interact = null;
        }
    }
    private void FixedUpdate()
        {
            ApplyMovement();
            ApplyAirControl();
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);
            verticalLookRotation = Mathf.Clamp(verticalLookRotation - mouseY, -maxLookAngle, maxLookAngle);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        }

        private void HandleMovementInput()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            bool isSprinting = Input.GetKey(key.Run);
            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedTransitionRate);

            moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized * currentSpeed;
        }

        private void HandleJumpInput()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

            if (Input.GetKeyDown(key.Jump) && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
        }

        private void ApplyMovement()
        {
            Vector3 targetVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * smoothAcceleration);
        }

        private void ApplyAirControl()
        {
            if (!isGrounded && isJumping)
            {
                Vector3 airControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * airControlFactor;
                rb.AddForce(airControl, ForceMode.Acceleration);
            }
        }

        private void HandleFOVAndTilt()
        {
            float targetFOV = Input.GetKey(key.Run) ? sprintFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        }

        private void HandleHeadBobbing()
        {
            if (IsPlayerMoving() && isGrounded)
            {
                bool isSprinting = Input.GetKey(KeyCode.LeftShift);
                float bobSpeed = isSprinting ? sprintBobSpeed : walkBobSpeed;
                float bobAmount = isSprinting ? sprintBobAmount : walkBobAmount;

                headBobTimer += Time.deltaTime * bobSpeed;
                float newY = defaultYPosition + Mathf.Sin(headBobTimer) * bobAmount;
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newY, playerCamera.transform.localPosition.z);
            }
            else
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(playerCamera.transform.localPosition.x, defaultYPosition, playerCamera.transform.localPosition.z), Time.deltaTime * smoothReturnSpeed);
            }
        }

        private bool IsPlayerMoving()
        {
            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }
    }
