using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

namespace Fragsurf.Movement {


    public class SurfCharacter : MonoBehaviour {
        public LayerMask layerMask;
        public float rayDistance = 100f;
        public KeyCode InteractKey = KeyCode.E;

        public float transitionSpeed = 5f;

        public float bobSpeed = 10f; // Скорость покачивания
        public float bobAmount = 0.05f; // Амплитуда покачивания

        private float defaultYPosition;
        private float timer = 0f;

        public Transform playerTransform; // Ссылка на игрока (для отслеживания скорости)

        ///// Fields /////

        [Header("Physics Settings")]
        public Vector3 colliderSize = new Vector3 (1f, 2f, 1f);


        [Header ("Crouching setup")]
        public float crouchingHeightMultiplier = 0.5f;
        public float crouchingSpeed = 10f;
        float defaultHeight;
        bool allowCrouch = true; 

        [Header ("Step offset (can be buggy, enable at your own risk)")]
        public bool useStepOffset = false;
        public float stepOffset = 0.35f;

        [Header ("Movement Config")]
        
        public CapsuleCollider _collider;

        public static GameObject Player;
        public static Transform PlayerTransform;
        public static Vector3 PlayerPos;
        public static Inventory Inventory;
        ///// Methods /////

        private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube( transform.position, colliderSize );
		}
		
        private void Awake () {
            Player = gameObject;
            PlayerTransform = gameObject.transform;
            PlayerPos = gameObject.transform.position;
            Inventory = GetComponent<Inventory>();

            if (cameraTransform == null)
                cameraTransform = GetComponent<Transform>();

            defaultYPosition = cameraTransform.localPosition.y;

        }
        public Camera mainCamera;
        public GameObject interactobject;
        Interact interact;


        private void Start () {
            defaultRotation = cameraTransform.localRotation;

            rb = GetComponent<Rigidbody>();

            // Отключаем физическое вращение Rigidbody
            rb.freezeRotation = true;

            // Прячем курсор
            Cursor.lockState = CursorLockMode.Locked;
        }
        [Header("Скорость")]
        public float walkSpeed = 5f; // Скорость ходьбы
        public float sprintSpeed = 10f; // Скорость бега
        public float speedTransitionRate = 10f; // Скорость плавного перехода между ходьбой и бегом

        private float currentSpeed; // Текущая скорость игрока

        [Header("Движение")]
        public float moveSpeed = 5f; // Скорость передвижения
        public float smoothAcceleration = 10f; // Плавность разгона

        [Header("Прыжок")]
        public float jumpForce = 5f; // Сила прыжка
        public LayerMask groundLayer; // Слой земли
        public float groundCheckDistance = 0.2f; // Дистанция проверки земли

        [Header("Камера")]
        public Transform cameraTransform; // Ссылка на камеру
        public float mouseSensitivity = 150f; // Чувствительность мыши
        public float maxLookAngle = 90f; // Максимальный угол наклона камеры вверх/вниз
        public float cameraSmoothSpeed = 5f;


        private Rigidbody rb;
        private Vector3 moveDirection;
        private float verticalLookRotation = 0f;

        public Camera playerCamera;
        public float defaultFOV = 60f;
        public float sprintFOV = 75f;
        public float fovTransitionSpeed = 5f;

        public float tiltAngle = 10f; // Угол наклона
        public float tiltSpeed = 5f; // Скорость перехода

        private Quaternion defaultRotation;

        public float walkBobSpeed = 10f; // Скорость покачивания при ходьбе
        public float walkBobAmount = 0.05f; // Амплитуда покачивания при ходьбе
        public float sprintBobSpeed = 15f; // Скорость покачивания при беге
        public float sprintBobAmount = 0.1f; // Амплитуда покачивания при беге
        public float smoothReturnSpeed = 5f; // Скорость плавного возвращения
        void Interact()
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                if (interactobject != hit.collider.gameObject)
                {
                    if (interact != null) interact.ExecuteUnhover();
                    interactobject = hit.collider.gameObject;
                    interact = interactobject.GetComponent<Interact>();
                    interact.ExecuteHover();
                }
                else if (interactobject != null && interact != null)
                {
                    interact.ExecutePresshover();
                    if (Input.GetKeyDown(InteractKey))
                    {
                        interact.ExecuteClick();
                    }
                    if (Input.GetKeyUp(InteractKey))
                    {
                        interact.ExecuteUnclick();
                    }
                    if (Input.GetKey(InteractKey))
                    {
                        interact.ExecutePress();
                    }
                }
            }
            else
            {
                if (interact != null) interact.ExecuteUnhover();
                interactobject = null;
                interact = null;
            }
        }

        private bool isFalling = false;           // Проверка падения
        public float jumpCameraOffset = 0.2f;     // Смещение камеры при прыжке
        public float fallCameraOffset = 0.2f;     // Смещение камеры при падении
        public float jumpReturnSpeed = 2f;        // Скорость возврата камеры после прыжка
        private bool isGrounded = true;
        void Headbobbing()
        {
            if (IsPlayerMoving() && isGrounded)
            {
                // Определяем параметры для ходьбы или бега
                bool isSprinting = Input.GetKey(KeyCode.LeftShift);
                float currentBobSpeed = isSprinting ? sprintBobSpeed : walkBobSpeed;
                float currentBobAmount = isSprinting ? sprintBobAmount : walkBobAmount;

                // Обновляем таймер и высчитываем новую позицию камеры
                timer += Time.deltaTime * currentBobSpeed;
                float newY = defaultYPosition + Mathf.Sin(timer) * currentBobAmount;
                cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, newY, cameraTransform.localPosition.z);
            }
            else if (isJumping)
            {
                // Смещение камеры вверх при прыжке
                cameraTransform.localPosition = Vector3.Lerp(
                    cameraTransform.localPosition,
                    new Vector3(cameraTransform.localPosition.x, defaultYPosition + jumpCameraOffset, cameraTransform.localPosition.z),
                    Time.deltaTime * jumpReturnSpeed
                );
            }
            else if (!isGrounded && rb.linearVelocity.y < 0)
            {
                // Смещение камеры вниз и добавление тряски
                fallShakeTimer += Time.deltaTime * fallShakeSpeed;
                float shakeOffset = Mathf.Sin(fallShakeTimer) * fallShakeAmount;
                cameraTransform.localPosition = Vector3.Lerp(
                    cameraTransform.localPosition,
                    new Vector3(cameraTransform.localPosition.x, defaultYPosition - fallCameraOffset + shakeOffset, cameraTransform.localPosition.z),
                    Time.deltaTime * jumpReturnSpeed
                );
            }
            else
            {
                SmoothReturn();
            }
        }

        void SmoothReturn()
        {
            // Плавный возврат камеры в исходное положение
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                new Vector3(cameraTransform.localPosition.x, defaultYPosition, cameraTransform.localPosition.z),
                Time.deltaTime * jumpReturnSpeed
            );
        }


        void HandleMovementInput()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);

            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedTransitionRate);

            Vector3 moveHorizontal = transform.right * moveX;
            Vector3 moveVertical = transform.forward * moveZ;

            moveDirection = (moveHorizontal + moveVertical).normalized * currentSpeed;
        }
        private bool isJumping = false;
        public float airControlFactor = 0.5f;
        void HandleJumpInput()
        {
            isGrounded = IsGrounded();

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        public float fallShakeAmount = 0.05f;     // Амплитуда тряски камеры при падении
        public float fallShakeSpeed = 25f;        // Скорость тряски камеры при падении
        private float fallShakeTimer = 0f;

        void Jump()
        {
            // Сброс вертикальной скорости для стабильного прыжка
            // Сброс вертикальной скорости для стабильного прыжка
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            // Добавление силы вверх
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isFalling = false;
        }
        private void FixedUpdate()
        {
            MoveCharacter();
            if (!IsGrounded() && isJumping)
            {
                Vector3 airControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * airControlFactor;
                rb.AddForce(airControl, ForceMode.Acceleration);
            }
            else
            {
                isJumping = false;
            }
        }

        void HandleMouseLook()
        {
            // Получаем вход от мыши
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Поворачиваем персонажа по оси Y
            transform.Rotate(Vector3.up * mouseX);

            // Поворачиваем камеру по оси X (вверх/вниз)
            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);

            cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        }
        void MoveCharacter()
        {
            // Плавно применяем движение к Rigidbody
            Vector3 targetVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
            Vector3 smoothedVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * smoothAcceleration);
            rb.linearVelocity = smoothedVelocity;

        }

        bool IsGrounded()
        {

            return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance,groundLayer);
        }

        private void Update () {
            float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);

            Quaternion targetRotation = Input.GetKey(KeyCode.LeftShift) ? Quaternion.Euler(tiltAngle, 0, 0) : defaultRotation;

            cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);

            Headbobbing();

            Interact();
            HandleMovementInput();
            HandleJumpInput();
            HandleMouseLook();
        }
        
        private bool IsPlayerMoving()
        {
            if (playerTransform == null) return false;

            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }

    }

}

