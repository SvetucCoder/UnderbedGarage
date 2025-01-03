using UnityEngine;
    public class SurfCharacter : MonoBehaviour
    {
        public static Inventory Inventory;
        [Header("Interact")]
        public LayerMask layerMask;
        public float rayDistance = 100f;
        public static KeyManager key;
        public CapsuleCollider Capsule;
        public CapsuleCollider Capsule2;
        public Transform CheckGround;
        public Transform CheckUp;

        [Header("Camera Settings")]
        public float BasicHeight = 2.1f;
        public float BasicCenter = 2.1f;
        public Vector3 CameraPos;
        public float mouseSensitivity = 150f;
        public float maxLookAngle = 90f;
        public float cameraSmoothSpeed = 5f;
        public float defaultFOV = 60f;
        public float sprintFOV = 75f;
        public float fovTransitionSpeed = 5f;
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 10f;
        public float speedTransitionRate = 10f;
        public float smoothAcceleration = 10f;
        public float walkBobSpeed = 10f;
        public float walkBobAmount = 0.05f;
        public float sprintBobSpeed = 15f;
        public float sprintBobAmount = 0.1f;
        [Header("Jump Settings")]
        public float jumpForce = 5f;
        public LayerMask groundLayer;
        public float groundCheckDistance = 0.2f;
        [Header("Crouch Settings")]
        public float CrouchHeight = 2.1f;
        public float CrouchCenter = 2.1f;
        public float CrouchSpeed = 5f;
        public float CrouchFOV = 75f;
        public float CrouchBobSpeed = 10f;
        public float CrouchBobAmount = 0.05f;
        public float speedCrouchHeightRate = 10f;
        public float CrouchUpDistance = 2f;
        public Vector3 CameraCrouchPos;
        [Header("Lay Settings")]
        public float LayHeight = 2.1f;
        public float LayCenter = 0.2f;
        public float LaySpeed = 5f;
        public float LayFOV = 75f;
        public float LayBobSpeed = 10f;
        public float LayBobAmount = 0.05f;
        public float speedLayHeightRate = 10f;
        public float LayUpDistance = 2f;
        public Vector3 CameraLayPos;

        private Rigidbody rb;
        private Vector3 moveDirection;
        private Camera playerCamera;
        private float currentSpeed;
        private float verticalLookRotation;
        private float defaultYPosition;
        private float headBobTimer;
        bool canUp = true;
        float bobSpeed = 0;
        float bobAmount = 0;
        bool isLay;
        bool isSprinting;
        bool isJump;
        bool isCrouch;

    void Awake()
    {
        Inventory = GetComponent<Inventory>();
        key = GetComponent<KeyManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    } 
    void CheckKey()
    {
        isJump = Input.GetKey(key.Jump);
        isSprinting = Input.GetKey(key.Run);
        if (Input.GetKey(key.Crouch))
        {
            isCrouch = true;
        }
        else
        {
            if(!Physics.Raycast(CheckUp.transform.position, Vector3.up, CrouchUpDistance, layerMask))
            {
                isCrouch = false;
                canUp = true;
            }
            else canUp = false;
        }
        if (Input.GetKey(key.Lay))
        {
            isLay = true;
        }
        else
        {
            if (!Physics.Raycast(CheckUp.transform.position, Vector3.up, LayUpDistance, layerMask))
            {
                isLay = false;
            }
        }
            if(!isCrouch) canUp = true;
    }
    private void Update()
    {
        CheckKey();
        HandleMouseLook();
        Movement();
        ChangeFOV();
        Interact();
        Crouch();
        Bobbing();
    }
    void Interact()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out AnimationCreator AnimationCreator))
            {
                if (Input.GetKeyDown(key.Interact))
                {
                    AnimationCreator.Interact();
                }
            }
        }

    }
    void FixedUpdate()
    {
        ApplyMovement();
    }
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        verticalLookRotation = Mathf.Clamp(verticalLookRotation - mouseY, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }
    void Crouch()
    {
        float height = BasicHeight;
        float height2 = 0;
        float center = BasicCenter;
        float speed = CrouchSpeed;
        Vector3 CameraPosition = CameraPos;
        if (isCrouch)
        {
            height = CrouchHeight;
            center = CrouchCenter;
            speed = CrouchSpeed;
            CameraPosition = CameraCrouchPos;
        }
        if (isLay)
        {
            height2 = BasicHeight;
            height = 0;
            center = LayCenter;
            speed = LaySpeed;
            CameraPosition = CameraLayPos;
        }
        Capsule.height = Mathf.Lerp(Capsule.height, height, Time.deltaTime * speed);
        Capsule.center = new Vector3(Capsule.center.x, Mathf.Lerp(Capsule.center.y, center, Time.deltaTime * speed), Capsule.center.z);
        Capsule2.height = Mathf.Lerp(Capsule2.height, height2, Time.deltaTime * speed);
        Capsule2.center = new Vector3(Capsule2.center.x, Mathf.Lerp(Capsule2.center.y, center, Time.deltaTime * speed), Capsule2.center.z);
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, CameraPosition, Time.deltaTime * speed);
        defaultYPosition = Mathf.Lerp(defaultYPosition, CameraPosition.y, Time.deltaTime * speed);
    }
    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float targetSpeed = walkSpeed;

        if (isCrouch) targetSpeed = CrouchSpeed;
        if (isLay) targetSpeed = LaySpeed;
        if (isSprinting) targetSpeed = targetSpeed * 2;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedTransitionRate);

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized * currentSpeed;

        bool isGrounded = Physics.Raycast(CheckGround.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isJump && isGrounded && canUp && !isLay)
        {
            float jumpforce2 = jumpForce;
            if (isCrouch)
            {
                jumpforce2 = 150;
            }
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpforce2, ForceMode.Impulse);
        }
    }
    void ApplyMovement()
    {
        Vector3 targetVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * smoothAcceleration);
    }
    void ChangeFOV()
    {
        float targetFOV = defaultFOV;
        if (isSprinting)
        {
            targetFOV = sprintFOV;
        }
        if (isCrouch)
        {
            targetFOV = CrouchFOV;
        }
        if (isCrouch && isSprinting)
        {
            targetFOV = defaultFOV;
        }
        if (isLay)
        {
            targetFOV = LayFOV;
        }
        if (isLay && isSprinting)
        {
            targetFOV = CrouchFOV;
        }
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
    }
    private void Bobbing()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            bobSpeed = walkBobSpeed;
            bobAmount = walkBobAmount;
            if (isSprinting)
            {
                bobSpeed = sprintBobSpeed;
                bobAmount = sprintBobAmount;
            }
            if (isCrouch)
            {
                bobSpeed = CrouchBobSpeed;
                bobAmount = CrouchBobAmount;
            }
            if (isCrouch && isSprinting)
            {
                bobSpeed = walkBobSpeed;
                bobAmount = walkBobAmount;
            }
            if (isLay)
            {
                bobSpeed = LayBobSpeed;
                bobAmount = LayBobAmount;
            }
            if (isLay && isSprinting)
            {
                bobSpeed = CrouchBobSpeed;
                bobAmount = CrouchBobAmount;
            }

            headBobTimer += Time.deltaTime * bobSpeed;
            float newY = defaultYPosition + Mathf.Sin(headBobTimer) * bobAmount;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newY, playerCamera.transform.localPosition.z);
        }
    }
}

