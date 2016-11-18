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
    float m_RollPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    Animator m_Animator;

    public bool m_IsClimbing;
    bool m_IsGrounded;
    bool m_IsRolling;
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
    private IKController m_IKController;
    public HingeJoint m_Joint;
    public Rigidbody m_JointRB;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        m_ClimbController = GetComponent<ClimbController>();
        m_RagdollController = GetComponent<RagdollController>();
        m_IKController = GetComponent<IKController>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        Move(move, crouch, jump, false, false, false, false);
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool jumpRelease, bool climb, bool roll, bool interact)
    {
        if (m_IsStruggling)
        {
            return;
        }

        if (move.magnitude > 1f)
            move.Normalize();

        Vector3 world_Move = move;
        move = transform.InverseTransformDirection(move);

        if (interact)
        {
            if (GameController.instance.interactable != null)
            {
                GameController.instance.interactable.Interact();
            }
        }

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

                if (m_IsPreparingJump)
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
                    if (jump)
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
                            m_Joint.transform.parent = m_ClimbInfo.parentTransform;

                            transform.position = Vector3.Lerp(transform.position, m_ClimbInfo.grabPosition + m_ClimbInfo.avgNormal * m_Capsule.radius, Time.deltaTime * 5f);
                            transform.LookAt(m_ClimbInfo.grabPosition);

                            m_Joint.transform.position = m_ClimbInfo.grabPosition + m_ClimbInfo.avgNormal * m_Capsule.radius + (transform.up * 1.5f);
                            m_Joint.connectedBody = null;
                            m_JointRB.useGravity = false;
                            m_JointRB.isKinematic = true;
                            m_JointRB.constraints = RigidbodyConstraints.FreezeAll;
                            m_RagdollController.SetRagdollActive(false);
                        }
                    }
                    else
                    {
                        m_Joint.transform.position = m_ClimbInfo.grabPosition + m_ClimbInfo.avgNormal * m_Capsule.radius + (transform.up * 1.5f);
                        if (m_Joint.connectedBody == null)
                        {
                            m_Joint.connectedBody = m_JointRB;
                            m_JointRB.constraints = RigidbodyConstraints.None;
                            //m_JointRB.useGravity = true;
                            //m_JointRB.isKinematic = false;
                            m_Animator.enabled = false;
                            m_RagdollController.SetRagdollActive(true);
                        }
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
        m_Animator.enabled = true;
        m_JointRB.constraints = RigidbodyConstraints.FreezeAll;
        m_JointRB.useGravity = false;
        m_JointRB.isKinematic = true;
        m_RagdollController.SetRagdollActive(false);

        CheckGroundStatus();

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        transform.eulerAngles = Vector3.Scale(transform.rotation.eulerAngles, Vector3.up);

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            //if (roll)
            //{
            //    StartCoroutine(HandleRoll());
            //}
            //else
            //{
            //if (m_IsRolling)
            //{
            //    transform.Translate(move * m_RollPower * Time.deltaTime);
            //}
            //else
            //{
            HandleGroundedMovement(crouch, jump);
            //}
            //}
        }
        else
        {
            HandleAirborneMovement(move);
        }

        ScaleCapsuleForCrouch(crouch || m_IsRolling);
        PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);

    }

    bool temp_HangWait = false;
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


        m_IsGrounded = false;
        m_Animator.applyRootMotion = false;
        m_GroundCheckDistance = 0.01f;
        m_Rigidbody.useGravity = true;
        m_IsClimbing = false;
    }

    void ScaleCapsuleForCrouch(bool crouch)
    {
        if (m_IsGrounded && crouch)
        {
            if (m_IsCrouching) return;
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
            m_IsCrouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
            {
                m_IsCrouching = true;
                return;
            }
            m_Capsule.height = m_CapsuleHeight;
            m_Capsule.center = m_CapsuleCenter;
            m_IsCrouching = false;
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
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        }
        else
        {
            if (GameController.instance.bossController.isShaking || !m_CanClimbNextFrame || m_IsPreparingJump)
            {
                m_Animator.SetFloat("Forward", 0f, 0.1f, Time.deltaTime);
                m_Animator.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
            }
            else
            {
                m_Animator.SetFloat("Forward", move.y, 0f, Time.deltaTime);
                m_Animator.SetFloat("Turn", move.x, 0f, Time.deltaTime);
            }
        }
        m_Animator.SetBool("Crouch", m_IsCrouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        m_Animator.SetBool("Climbing", m_IsClimbing);
        m_Animator.SetFloat("ClimbLeg", m_ClimbLeg);

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
        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }

    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            Jump();
        }
    }

    IEnumerator HandleRoll()
    {
        m_IsRolling = true;

        yield return new WaitForSeconds(0.4f);

        m_IsRolling = false;
    }

    void Jump()
    {
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
        m_IsGrounded = false;
        m_GroundCheckDistance = 0.01f;
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
            v.y = m_Rigidbody.velocity.y;
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
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;

            if (hitInfo.transform.tag.Contains("Boss"))
            {
                if (hitInfo.transform.GetComponent<shitscript>())
                {
                    this.transform.parent = hitInfo.transform.GetComponent<shitscript>().parentBone.transform;
                    m_IsGroundedOnBoss = true;
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

            m_IsGroundedOnBoss = hitInfo.transform.tag.Contains("Boss");

        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
            //this.transform.parent = null;
        }
    }


    public void SwitchClimbLeg()
    {
        if (m_ClimbLeg < 0)
            m_ClimbLeg = 1;
        else
            m_ClimbLeg = -1;
    }
}

