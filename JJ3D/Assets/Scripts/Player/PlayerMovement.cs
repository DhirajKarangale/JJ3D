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
        public float sensitivity = 2f;
        public bool clampVerticalRotation = true;
        private float MaximumX = 90F;
        private float MinimumX = -90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion playerTargetRot;
        private Quaternion camTargetRot;
        private bool m_cursorIsLocked = true;
        [HideInInspector] public Perspective perspective;

        public void Init(Transform character, Transform camera)
        {
            playerTargetRot = character.localRotation;
            camTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * sensitivity;
            float xRot = Input.GetAxis("Mouse Y") * sensitivity;

            playerTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            camTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
            {
                camTargetRot = ClampRotationAroundXAxis(camTargetRot);
            }

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, playerTargetRot, smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, camTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = playerTargetRot;
                camera.localRotation = camTargetRot;
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
            float yRot = lookJoystick.Horizontal() * sensitivity;
            float xRot = lookJoystick.Vertical() * sensitivity;

            playerTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            camTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
            {
                camTargetRot = ClampRotationAroundXAxis(camTargetRot);
            }

            if (smooth)
            {
                camParent.localRotation = Quaternion.Slerp(camParent.localRotation, playerTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                camParent.localRotation = playerTargetRot;
            }

            camera.LookAt(camParent);
            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            if (lockCursor) InternalLockUpdate();
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
        // [SerializeField] PlayerAttack playerAttack;
        public float forwardSpeed = 8.0f;
        public float backwardSpeed = 4.0f;
        public float sideSpeed = 4.0f;
        public float jumpForce = 30f;
        [HideInInspector] public float currSpeed = 8f;
        [HideInInspector] public bool isRunning;
        public AnimationCurve slopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;

            if (input.x > 0 || input.x < 0) currSpeed = sideSpeed;
            if (input.y < 0) currSpeed = backwardSpeed;
            if (input.y > 0) currSpeed = forwardSpeed;
        }
    }

    [Serializable]
    public class Advanced
    {
        public float groundCheckDistance = 0.01f;
        public float stickToGroundHelperDistance = 0.5f;
        public float slowDownRate = 20f;
        public bool airControl;
        public float shellOffset; //set it to 0.1 or more if you get stuck in wall
    }

    [SerializeField] Player player;
    [SerializeField] Look look = new Look();
    [SerializeField] internal Movement movement = new Movement();
    [SerializeField] Advanced advanced = new Advanced();

    [Header("Audio Clip")]
    [SerializeField] AudioClip clipJump;
    [SerializeField] AudioClip clipWalk;
    [SerializeField] AudioClip clipFall;
    [SerializeField] AudioClip clipSilp;

    [Header("Effect")]
    [SerializeField] ParticleSystem psFall;

    private Camera cam;
    private Animator animator;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private Vector3 m_GroundContactNormal;
    private float m_YRotation;
    private bool isJump, isPreviouslyGrounded, isJumping, isGrounded;

    public Vector3 Velocity
    {
        get { return rigidBody.velocity; }
    }

    private void Start()
    {
        cam = player.cam;
        animator = player.animator;
        rigidBody = player.rigidBody;
        capsuleCollider = player.capsuleCollider;
        look.Init(transform, cam.transform);
        PerspectiveButton();
    }

    private void Update()
    {
        animator.SetFloat("vertical", GetInput().y);
        animator.SetFloat("horrizontal", GetInput().x);
        animator.SetBool("isJump", isJumping);

        RotateView();

        if (Input.GetKeyDown(KeyCode.P)) // Move on a Button
        {
            PerspectiveButton();
        }

        if (Input.GetButtonDown("Jump") && !isJump) // Move on a Button
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advanced.airControl || isGrounded))
        {
            Vector3 desiredMove = transform.forward * input.y + cam.transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movement.currSpeed;
            desiredMove.z = desiredMove.z * movement.currSpeed;
            desiredMove.y = desiredMove.y * movement.currSpeed;

            if (rigidBody.velocity.sqrMagnitude < (movement.currSpeed * movement.currSpeed))
            {
                rigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }
        }

        // Slip Effect
        // if (isGrounded && (input == Vector2.zero) && (Mathf.Abs(rigidBody.velocity.magnitude) > 6))
        // {
        //     if (!audioSource.isPlaying)
        //     {
        //         audioSource.volume = 0.08f;
        //         audioSource.loop = true;
        //         audioSource.clip = clipSilp;
        //         audioSource.Play();
        //         psFall.Play();
        //     }
        // }
        // else
        // {
        //     audioSource.loop = false;
        // }

        if (isGrounded)
        {
            rigidBody.drag = 5f;

            if (isJump)
            {
                rigidBody.drag = 0f;
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
                rigidBody.AddForce(new Vector3(0f, movement.jumpForce, 0f), ForceMode.Impulse);
                isJumping = true;
                player.PlayAudio(clipJump);
            }

            if (!isJumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && rigidBody.velocity.magnitude < 1f)
            {
                rigidBody.Sleep();
            }
        }
        else
        {
            rigidBody.drag = 0f;
            if (isPreviouslyGrounded && !isJumping)
            {
                StickToGroundHelper();
            }
        }
        isJump = false;
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movement.slopeCurveModifier.Evaluate(angle);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius * (1.0f - advanced.shellOffset), Vector3.down, out hitInfo, ((capsuleCollider.height / 2f) - capsuleCollider.radius) + advanced.stickToGroundHelperDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                rigidBody.velocity = Vector3.ProjectOnPlane(rigidBody.velocity, hitInfo.normal);
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
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        float oldYRotation = transform.eulerAngles.y;

        if (look.lookJoystick.Vertical() == 0 && look.lookJoystick.Horizontal() == 0)
        {
            look.LookRotation(transform, cam.transform);
        }
        else
        {
            look.LookAround(cam.transform);
        }

        if (isGrounded || advanced.airControl)
        {
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            rigidBody.velocity = velRotation * rigidBody.velocity;
        }
    }

    private void GroundCheck()
    {
        isPreviouslyGrounded = isGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius * (1.0f - advanced.shellOffset) / 4, Vector3.down, out hitInfo, ((capsuleCollider.height / 2f) - capsuleCollider.radius) + advanced.groundCheckDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            if (!isGrounded)
            {
                psFall.Play();
                player.PlayAudio(clipFall);
            }
            isGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            isGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!isPreviouslyGrounded && isGrounded && isJumping)
        {
            isJumping = false;
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

    public void WalkSound()
    {
        if (!isGrounded) return;
        player.PlayAudio(clipWalk, 0.1f);
    }
}