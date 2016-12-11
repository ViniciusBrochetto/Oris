using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ClimbController))]

public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_JumpPower = 12f;
    [SerializeField]
    float m_ExtraRollPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    Animator m_Animator;

    public bool m_CanDie = true;
    public bool m_IsClimbing;
    bool m_IsGrounded;
    bool m_IsRolling;
    bool m_CanRoll = true;
    bool m_IsCrouching;
    bool m_IsPreparingJump;
    bool m_isInteracting;
    public bool m_IsGroundedOnBoss;
    public bool m_IsStruggling;

    bool m_CanClimb = false;
    bool m_CanClimbNextFrame = false;

    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_ClimbLeg = 1f;
    Vector3 m_GroundNormal;
    [SerializeField]
    Vector3 m_WallNormal;
    [SerializeField]
    Transform m_WallPosition;

    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;

    private ClimbController m_ClimbController;
    private RagdollController m_RagdollController;
    private PlayerAudioController m_AudioController;
    public HingeJoint m_Joint;
    public Rigidbody m_JointRB;

    private float m_StartJumpHeight;

    [SerializeField]
    private float m_MaxFallHeight;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        m_ClimbController = GetComponent<ClimbController>();
        m_RagdollController = GetComponent<RagdollController>();
        m_AudioController = GetComponent<PlayerAudioController>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    void Update()
    {
        transform.localScale = transform.localScale;

    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        Move(move, crouch, jump, false, false, false, false);
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool jumpRelease, bool climb, bool roll, bool interact)
    {
        if (m_IsStruggling)
        {
            if (!GameController.instance.bossController.isShaking)
                m_IsStruggling = false;
            else
                return;
        }

        if (move.magnitude > 1f)
            move.Normalize();

        Vector3 world_Move = move;
        move = transform.InverseTransformDirection(move);

        if (!temp_HangWait && climb)
        {
            ClimbInfo m_ClimbInfo;
            m_ClimbInfo = m_ClimbController.Climb();
            m_CanClimb = m_ClimbInfo.handsConnected && m_ClimbInfo.feetConnected;
            m_CanClimb = m_CanClimb && Vector3.Angle(Vector3.up, m_ClimbInfo.avgNormal) > 40f;

            if (m_ClimbInfo.isBoss)
                GameController.instance.bossController.SetPhase(m_ClimbInfo.parentTransform.tag);

            GameController.instance.bossController.SetPlayerClimbing(m_CanClimb && m_ClimbInfo.isBoss);

            if (m_CanClimb)
            {
                m_IsClimbing = true;
                m_Capsule.enabled = false;
                UpdateGroundHeight();

                if (m_IsPreparingJump && m_IsClimbing)
                {
                    if (jumpRelease)
                    {
                        m_IsPreparingJump = false;
                        WallJump();
                        StartCoroutine(WallJumpTimer());
                        return;
                    }
                }
                else
                {
                    if (jump && m_IsClimbing)
                    {
                        m_IsPreparingJump = true;
                        return;
                    }

                    m_ClimbInfo = m_ClimbController.Climb(world_Move.normalized * Time.deltaTime * 4f);
                    m_CanClimbNextFrame = m_ClimbInfo.handsConnected && m_ClimbInfo.feetConnected;

                    if (!GameController.instance.bossController.isShaking)
                    {
                        if (m_CanClimbNextFrame)
                        {
                            m_Animator.enabled = true;
                            m_Rigidbody.useGravity = false;

                            transform.parent = m_ClimbInfo.parentTransform;


                            Vector3 p = Vector3.Lerp(transform.position, m_ClimbInfo.grabPosition + m_ClimbInfo.avgNormal * m_Capsule.radius, Time.deltaTime * 5f);
                            transform.position = p;

                            float r;
                            if (transform.rotation.eulerAngles.z > 180)
                                r = Mathf.Lerp(transform.rotation.eulerAngles.z, 360f, Time.deltaTime * move.normalized.magnitude * 0.5f);
                            else
                                r = Mathf.Lerp(transform.rotation.eulerAngles.z, 0f, Time.deltaTime * move.normalized.magnitude * 0.5f);

                            transform.LookAt(m_ClimbInfo.grabPosition);

                            transform.rotation *= Quaternion.Euler(0f, 0f, r);
                        }
                        else
                        {
                            Debug.Log("Cant climb");
                        }
                    }
                    else
                    {
                        m_IsStruggling = true;
                    }
                }

                UpdateAnimator(move);
                return;
            }
        }

        GameController.instance.bossController.SetPlayerClimbing(false);
        m_IsClimbing = false;
        m_Rigidbody.useGravity = true;
        m_IsPreparingJump = false;
        m_Capsule.enabled = true;

        if (m_Joint)
        {
            m_JointRB.constraints = RigidbodyConstraints.FreezeRotation;
            m_JointRB.useGravity = true;
        }

        CheckGroundStatus();

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        if (interact)
        {
            if (GameController.instance.interactable != null && GameController.instance.interactable.GetInteractable())
            {
                m_Animator.SetTrigger("Interact");
                GameController.instance.interactable.Interact();
            }
        }

        transform.eulerAngles = Vector3.Scale(transform.rotation.eulerAngles, Vector3.up);
        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            if (roll && m_CanRoll)
            {
                m_IsRolling = true;
                m_CanRoll = false;
            }
            else
            {
                if (m_IsRolling)
                {
                    transform.Translate(move * m_ExtraRollPower * Time.deltaTime);
                }
                else
                {
                    HandleGroundedMovement(crouch, jump);
                }
            }
        }
        else
        {
            HandleAirborneMovement(move);
        }

        //ScaleCapsuleForCrouch();
        //PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);

    }

    public bool temp_HangWait = false;
    IEnumerator WallJumpTimer()
    {
        temp_HangWait = true;
        yield return new WaitForSeconds(0.7f);
        temp_HangWait = false;
    }

    void WallJump()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 jumpDir = ((v * m_CamForward + h * Camera.main.transform.right)).normalized * m_JumpPower * 0.5f;
        jumpDir.y = m_JumpPower;

        m_Rigidbody.velocity = jumpDir;

        //if (jumpDir.y != m_JumpPower)
        //    transform.localRotation = Quaternion.LookRotation(Vector3.Scale(m_Rigidbody.velocity, new Vector3(1, 0, 1)));

        UpdateGroundHeight();
        m_IsGrounded = false;
        m_Animator.applyRootMotion = false;
        m_GroundCheckDistance = 0.05f;
        m_Rigidbody.useGravity = true;
        m_IsClimbing = false;

        m_AudioController.Jump();
    }

    void ScaleCapsuleForCrouch()
    {
        if (m_IsGrounded && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))
        {
            if (m_IsRolling) return;
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
        }
        else
        {

            m_Capsule.height = m_CapsuleHeight;
            m_Capsule.center = m_CapsuleCenter;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!m_IsCrouching)
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
            {
                m_IsCrouching = true;
            }
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        if (!m_IsClimbing)
        {
            if (!m_IsRolling)
            {
                m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
                m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            }
            else
            {
                m_Animator.SetFloat("Forward", 0f);
                m_Animator.SetFloat("Turn", 0f);
            }
        }
        else
        {
            if (!m_CanClimbNextFrame)
            {
                m_Animator.SetFloat("Forward", 0f);
                m_Animator.SetFloat("Turn", 0f);
            }
            else if (GameController.instance.bossController.isShaking || m_IsPreparingJump)
            {
                m_Animator.SetFloat("Forward", 0f);
                m_Animator.SetFloat("Turn", 0f);
            }
            else
            {
                m_Animator.SetFloat("Forward", move.y, 0.1f, Time.deltaTime);
                m_Animator.SetFloat("Turn", move.x, 0.1f, Time.deltaTime);
            }
        }

        m_Animator.SetBool("Crouch", m_IsCrouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        m_Animator.SetBool("Climbing", m_IsClimbing);
        //m_Animator.SetFloat("walking_on_boss", m_IsGroundedOnBoss ? 1f : 0f);

        if (move.x > 0.1f)
            m_Animator.SetFloat("ClimbLeg", 1);
        else if (move.x < -0.1f)
            m_Animator.SetFloat("ClimbLeg", 0);
        else
            m_Animator.SetFloat("ClimbLeg", m_ClimbLeg);



        m_Animator.SetBool("Roll", m_IsRolling);
        m_Animator.SetBool("Struggling", m_IsStruggling);

        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if ((m_IsGrounded || m_IsClimbing) && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }

    void HandleAirborneMovement(Vector3 move)
    {
        // apply extra gravity from multiplier:

        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        transform.Translate(move * Time.deltaTime);
        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.05f;
    }

    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            Jump();
        }
    }

    public void SetRolling(int b)
    {
        m_IsRolling = b == 1;
        StartCoroutine(RollCD());
    }

    IEnumerator RollCD()
    {
        yield return new WaitForSeconds(0.2f);
        m_CanRoll = true;
    }

    void Jump()
    {
        UpdateGroundHeight();

        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower * .8f, m_Rigidbody.velocity.z);
        m_IsGrounded = false;
        m_GroundCheckDistance = 0.05f;
        m_AudioController.Jump();
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);

        if (m_IsRolling)
        {
            turnSpeed *= 10f;
        }

        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (!m_IsClimbing && m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = Mathf.Min(0f, m_Rigidbody.velocity.y * m_GravityMultiplier);
            m_Rigidbody.velocity = v;
        }
        else if (m_IsClimbing)
        {
            if (m_CanClimbNextFrame)
                m_Rigidbody.velocity = (m_Animator.deltaPosition * 2f) / Time.deltaTime;
            else
                m_Rigidbody.velocity = Vector3.zero;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            if (!m_IsGrounded)
            {
                if (m_StartJumpHeight - transform.position.y > m_MaxFallHeight)
                {
                    StartCoroutine(Die());
                    return;
                }

                if (m_StartJumpHeight - transform.position.y > m_MaxFallHeight * 0.5f)
                {
                    m_AudioController.Jump();
                }
            }
            else
            {
                UpdateGroundHeight();
            }

            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;

            if (hitInfo.transform.tag.Contains("Boss"))
            {
                if (hitInfo.transform.GetComponent<shitscript>())
                {
                    this.transform.parent = hitInfo.transform.GetComponent<shitscript>().m_ParentBone.transform;
                }
                else
                {
                    this.transform.parent = hitInfo.transform;
                }
            }
            else
            {
                this.transform.parent = null;
            }

            //m_IsGroundedOnBoss = hitInfo.transform.tag.Contains("Boss");

        }
        else
        {
            m_IsRolling = false;
            m_CanRoll = true;
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
            //this.transform.parent = null;
        }
    }

    public void UpdateGroundHeight()
    {
        m_StartJumpHeight = transform.position.y;
    }

    IEnumerator Die()
    {
        if (m_CanDie)
        {
            m_AudioController.Damage();
            m_Rigidbody.isKinematic = true;
            GameController.instance.isPausable = false;
            m_RagdollController.SetFullRagdollActive(true);
            GameController.instance.isPlayerControllable = false;

            yield return new WaitForSeconds(5f);

            GameController.instance.cameraController.RequestFadeToBlack();

            yield return new WaitForSeconds(1f);

            GameController.instance.ReloadGame();
        }
        yield return 0;
    }

    public void SwitchClimbLeg()
    {
        if (m_ClimbLeg < 0)
            m_ClimbLeg = 1;
        else
            m_ClimbLeg = -1;
    }
}

