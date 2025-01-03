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

        [Header("Camera Settings")]
        public float BasicHeight = 2.1f;
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

    [Header("Crouch Settings")]
    public float CrouchHeight = 2.1f;
    public float speedCrouchHeightRate = 10f;
    public float CrouchSpeed = 5f;
    public float CrouchFOV = 75f;
    public float CrouchBobSpeed = 10f;
    public float CrouchBobAmount = 0.05f;

    [Header("Lay Settings")]
    public float LayHeight = 2.1f;
    public float speedLayHeightRate = 10f;
    public float LaySpeed = 5f;
    public float LayFOV = 75f;
    public float LayBobSpeed = 10f;
    public float LayBobAmount = 0.05f;


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
    public CapsuleCollider cc;
    public CapsuleCollider cc2;


    public Vector3 posjump;
    public Vector3 poslayjump;
    public Vector3 poscrouchjump;
    public Transform currentposJump;

    public float crouchrayDistance = 2f;
    public float layrayDistance = 2f;
    public Transform checkup;
    public bool canjumpup = true;
    private void Awake()
    {

        Inventory = GetComponent<Inventory>();
        key = GetComponent<KeyManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerCamera = GetComponentInChildren<Camera>();
        defaultYPosition = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        CamPos = playerCamera.transform.localPosition;
        poslayjump = posjump;
        poscrouchjump = posjump;
        poscrouchjump.y += 0.5f;
        poslayjump.y += 0.85f;
        LayCamPos = playerCamera.transform.localPosition;
        LayCamPos.y -= 0.75f;
        LayCamPos.z += 0.75f;
    }
    public Renderer targetRenderer;
    bool IsLay;
        private void Update()
        {

            isSprinting = Input.GetKey(key.Run);
        if (!isCrouch)
        {
            isCrouch = Input.GetKey(key.Crouch);
        }
        if (!IsLay)
        {
            IsLay = Input.GetKey(key.Lay);
        }
        if (!Physics.Raycast(checkup.transform.position, Vector3.up, crouchrayDistance, layerMask) && !Input.GetKey(key.Crouch))
        {
            isCrouch = false;
            canjumpup = true;

        }
        if (Physics.Raycast(checkup.transform.position, Vector3.up, crouchrayDistance, layerMask) && isCrouch)
        {
            canjumpup = false;
        }
        else if(!Physics.Raycast(checkup.transform.position, Vector3.up, crouchrayDistance, layerMask) && isCrouch)
        {
            canjumpup = true;
        }
        if (!Physics.Raycast(checkup.transform.position, Vector3.up, layrayDistance, layerMask) && !Input.GetKey(key.Lay))
        {
            IsLay = false;
        }
        if (Physics.Raycast(checkup.transform.position, Vector3.up, layrayDistance, layerMask) && !Input.GetKey(key.Lay) && Input.GetKey(key.Crouch))
        {
            IsLay = false;
        }
        HandleMouseLook();
            HandleMovementInput();
            HandleJumpInput();
            HandleFOVAndTilt();
  
            Interact();
        Crouch();

    }
    void Interact()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent<AnimationCreator>(out AnimationCreator AnimationCreator))
            {
                if (Input.GetKeyDown(key.Interact))
                {
                    AnimationCreator.Interact();
                }
            }
        }

    }
    public Vector3 CamPos;
    public Vector3 LayCamPos;
    private void FixedUpdate()
        {
            ApplyMovement();


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
            bool isCrouch = Input.GetKey(key.Crouch);
            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            if (isCrouch && !isSprinting)
            {
                targetSpeed = CrouchSpeed;
            }
            else if (isCrouch && isSprinting)
            {
                targetSpeed = CrouchSpeed * 2;
            }
            if (IsLay && !isSprinting)
            {
                targetSpeed = LaySpeed;
            }
            else if (IsLay && isSprinting)
            {
                targetSpeed = LaySpeed * 2;
            }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedTransitionRate);

            moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized * currentSpeed;
        }
        private void Crouch()
        {
            if (isCrouch)
            {
                if (!IsLay) currentposJump.localPosition = new Vector3(currentposJump.localPosition.x,Mathf.Lerp(currentposJump.localPosition.y, poscrouchjump.y, Time.deltaTime * speedCrouchHeightRate), currentposJump.localPosition.z);
                if (!IsLay) cc.height = Mathf.Lerp(cc.height, CrouchHeight, Time.deltaTime * speedCrouchHeightRate);
            }
            else if(!isCrouch )
            {
            if (!IsLay) currentposJump.localPosition = new Vector3(currentposJump.localPosition.x, Mathf.Lerp(currentposJump.localPosition.y, posjump.y, Time.deltaTime * speedCrouchHeightRate), currentposJump.localPosition.z);
            if (!IsLay) cc.height = Mathf.Lerp(cc.height, BasicHeight, Time.deltaTime * speedCrouchHeightRate);
            }
            if (IsLay)
            {
            currentposJump.localPosition = new Vector3(currentposJump.localPosition.x, Mathf.Lerp(currentposJump.localPosition.y, poslayjump.y, Time.deltaTime * speedCrouchHeightRate), currentposJump.localPosition.z);
            cc.height = Mathf.Lerp(cc.height, 0, Time.deltaTime * speedLayHeightRate);
                cc2.height = Mathf.Lerp(cc2.height, LayHeight, Time.deltaTime * speedLayHeightRate);
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, LayCamPos, Time.deltaTime * speedLayHeightRate);
            }
            else if (!IsLay)
            {
                if(!isCrouch) cc.height = Mathf.Lerp(cc.height, LayHeight, Time.deltaTime * speedLayHeightRate);
                if (!isCrouch) currentposJump.localPosition = new Vector3(currentposJump.localPosition.x, Mathf.Lerp(currentposJump.localPosition.y, posjump.y, Time.deltaTime * speedCrouchHeightRate), currentposJump.localPosition.z);
                cc2.height = Mathf.Lerp(cc2.height, 0, Time.deltaTime * speedLayHeightRate);
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, CamPos, Time.deltaTime * speedLayHeightRate);
            }
        }

        private void HandleJumpInput()
        {

        isGrounded = Physics.Raycast(currentposJump.position, Vector3.down, groundCheckDistance, groundLayer);

            if (Input.GetKey(key.Jump) && isGrounded && canjumpup)
            {
            float jumpforce2 = jumpForce;
            if (isCrouch)
            {
                jumpforce2 = 150;
            }
            if (IsLay)
            {
                jumpforce2 = 0;
            }
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpforce2, ForceMode.Impulse);
                isJumping = true;
            isGrounded = false;
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
            if (isCrouch)
            {
                targetFOV = CrouchFOV;
            }
            else if (isCrouch && isSprinting)
            {
                targetFOV = defaultFOV;
            }
            if (IsLay)
            {
                targetFOV = LayFOV;
            }
            else if (IsLay && isSprinting)
            {
                targetFOV = CrouchFOV;
            }
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        }
        public float bobSpeed = 0;
        public float bobAmount = 0;
    bool isSprinting;
      bool isCrouch;
    public Transform jumptransform;
    private void HandleHeadBobbing()
        {
            if (IsPlayerMoving() && isGrounded)
            {
                if (isSprinting)
                {
                    bobSpeed = sprintBobSpeed;
                    bobAmount = sprintBobAmount;
                }
                else
                {
                    bobSpeed = walkBobSpeed;
                    bobAmount = walkBobAmount;
                }
                if (isCrouch)
                {
                    bobSpeed = CrouchBobSpeed;
                    bobAmount = CrouchBobAmount;
                }
                else if (isCrouch && isSprinting)
                {   
                bobSpeed = CrouchBobSpeed*2;
                bobAmount = CrouchBobAmount * 2;
            }
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
