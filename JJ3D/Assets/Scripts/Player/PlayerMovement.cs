using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Serializable]
    public class Look
    {
        public enum Perspective { FPP, TPP }

        public EasyJoystick.Joystick lookJoystick;

        public Transform camParent;
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;
        [HideInInspector] public Perspective perspective;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            if (perspective == Perspective.TPP)
            {
                camParent.transform.localRotation = Quaternion.Euler(0, 0, 0);
                // camera.LookAt(camParent);
            }

            UpdateCursorLock();
        }

        public void LookAround(Transform camera)
        {
            float yRot = lookJoystick.Horizontal() * XSensitivity;
            float xRot = lookJoystick.Vertical() * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            if (smooth)
            {
                camParent.localRotation = Quaternion.Slerp(camParent.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                camParent.localRotation = m_CharacterTargetRot;
            }

            camera.LookAt(camParent);
            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }

    [Serializable]
    public class Movement
    {
        [SerializeField] PlayerAttack playerAttack;
        public float ForwardSpeed = 8.0f;   // Speed when walking forward
        public float BackwardSpeed = 4.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways
        public float JumpForce = 30f;
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        private bool m_Running;
        [HideInInspector] public float CurrentTargetSpeed = 8f;
        [HideInInspector] public Item shoes;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                // CurrentTargetSpeed = StrafeSpeed; - Original just Commented

                // Added By DK
                if (playerAttack.isAming || playerAttack.isShoothing) CurrentTargetSpeed = StrafeSpeed;
                else CurrentTargetSpeed = ForwardSpeed;
            }

            if (shoes)
            {
                if (shoes.currHealth > 0)
                {
                    CurrentTargetSpeed *= shoes.speedModifire;
                    shoes.currHealth -= 0.1f;
                }
                else
                {
                    shoes.DestroyItem();
                }
            }
        }

        public bool Running
        {
            get { return m_Running; }
        }
    }

    [Serializable]
    public class Advanced
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    [SerializeField] Animator animator;

    public Camera cam;
    public Look look = new Look();
    public Movement movement = new Movement();
    public Advanced advanced = new Advanced();

    [Header("Effect")]
    [SerializeField] ParticleSystem psFall;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipJump;
    [SerializeField] AudioClip clipWalk;
    [SerializeField] AudioClip clipFall;
    [SerializeField] AudioClip clipSilp;

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;

    public Vector3 Velocity
    {
        get { return m_RigidBody.velocity; }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    public bool Jumping
    {
        get { return m_Jumping; }
    }

    public bool Running
    {
        get
        {
            return movement.Running;
        }
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        look.Init(transform, cam.transform);
        PerspectiveButton();
        GameManager.instance.equipementManager.onEquipementChanged += OnEquipmentChanged;
    }

    private void Update()
    {
        animator.SetFloat("vertical", GetInput().y);
        animator.SetFloat("horrizontal", GetInput().x);

        animator.SetBool("isJump", m_Jumping);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PerspectiveButton();
        }

        RotateView();

        if (Input.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advanced.airControl || m_IsGrounded))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * input.y + cam.transform.right * input.x;
            // Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movement.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movement.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movement.CurrentTargetSpeed;
            if (m_RigidBody.velocity.sqrMagnitude < (movement.CurrentTargetSpeed * movement.CurrentTargetSpeed))
            {
                m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }
        }

        if (m_IsGrounded && (input == Vector2.zero) && (Mathf.Abs(m_RigidBody.velocity.magnitude) > 6))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.08f;
                audioSource.loop = true;
                audioSource.clip = clipSilp;
                audioSource.Play();
                psFall.Play();
            }
        }
        else
        {
            audioSource.loop = false;
        }

        if (m_IsGrounded)
        {
            m_RigidBody.drag = 5f;

            if (m_Jump)
            {
                m_RigidBody.drag = 0f;
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                m_RigidBody.AddForce(new Vector3(0f, movement.JumpForce, 0f), ForceMode.Impulse);
                m_Jumping = true;
                audioSource.volume = 1;
                audioSource.PlayOneShot(clipJump);
            }

            if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
            {
                m_RigidBody.Sleep();
            }
        }
        else
        {
            m_RigidBody.drag = 0f;
            if (m_PreviouslyGrounded && !m_Jumping)
            {
                StickToGroundHelper();
            }
        }
        m_Jump = false;
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movement.SlopeCurveModifier.Evaluate(angle);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advanced.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) +
                               advanced.stickToGroundHelperDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        movement.UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        if (look.lookJoystick.Vertical() == 0 && look.lookJoystick.Horizontal() == 0)
        {
            look.LookRotation(transform, cam.transform);
        }
        else
        {
            look.LookAround(cam.transform);
        }

        if (m_IsGrounded || advanced.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advanced.shellOffset) / 4, Vector3.down, out hitInfo, ((m_Capsule.height / 2f) - m_Capsule.radius) + advanced.groundCheckDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (!m_IsGrounded)
            {
                psFall.Play();
                audioSource.PlayOneShot(clipFall);
            }
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
        }
    }

    public void PerspectiveButton()
    {
        if (look.perspective == Look.Perspective.FPP)
        {
            look.perspective = Look.Perspective.TPP;
            cam.transform.localPosition = new Vector3(0, 2f, -4f);
            cam.transform.rotation = Quaternion.Euler(30, 0, 0);
            look.lookJoystick.gameObject.SetActive(true);
        }
        else
        {
            look.perspective = Look.Perspective.FPP;
            cam.transform.localPosition = Vector3.zero;
            cam.transform.rotation = Quaternion.Euler(0, 0, 0);
            cam.transform.eulerAngles = new Vector3(0, 0, 0);
            look.lookJoystick.gameObject.SetActive(false);
        }
    }

    private void OnEquipmentChanged(Item newItem, Item oldItem)
    {
        if (oldItem && oldItem.itemType == ItemType.Shoes) movement.shoes = null;
        if (newItem && newItem.itemType == ItemType.Shoes) movement.shoes = newItem;
    }

    public void WalkSound()
    {
        if (!m_IsGrounded) return;
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(clipWalk);
    }
}