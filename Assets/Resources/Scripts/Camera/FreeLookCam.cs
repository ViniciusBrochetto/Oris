using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

public class FreeLookCam : PivotBasedCameraRig
{
    // This script is designed to be placed on the root object of a camera rig,
    // comprising 3 gameobjects, each parented to the next:

    // 	Camera Rig
    // 		Pivot
    // 			Camera
    [SerializeField]
    private Transform m_SecondaryTarget;
    [SerializeField]
    private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
    [Range(0f, 10f)]
    [SerializeField]
    private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    [SerializeField]
    private float m_TurnSmoothing = 0.1f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
    [SerializeField]
    private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField]
    private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
    [SerializeField]
    private bool m_LockCursor = false;                   // Whether the cursor should be hidden and locked.t.
    [SerializeField]
    public bool m_CameraLockedForBoss = false;

    private float m_LookAngle;                    // The rig's y axis rotation.
    private float m_TiltAngle;                    // The pivot's x axis rotation.
    private const float k_LookDistance = 50f;    // How far in front of the pivot the character's look target is.
    private Vector3 m_PivotEulers;
    private Quaternion m_PivotTargetRot;
    private Quaternion m_TransformTargetRot;
    private ProtectCameraFromWallClip m_ProtectFromWall;
    private CameraFixedPath m_CamFixedPath;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !m_LockCursor;
        m_PivotEulers = m_Pivot.rotation.eulerAngles;

        m_PivotTargetRot = m_Pivot.transform.localRotation;
        m_TransformTargetRot = transform.localRotation;
        m_ProtectFromWall = GetComponent<ProtectCameraFromWallClip>();
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Quaternion targetRot = Quaternion.LookRotation(m_SecondaryTarget.position - transform.position, Vector3.up);
            targetRot = Quaternion.Slerp(m_Pivot.rotation, targetRot, 3f * Time.deltaTime);
            targetRot.eulerAngles = Vector3.Scale(targetRot.eulerAngles, new Vector3(1f, 1f, 0f));
            m_Pivot.rotation = targetRot;

            m_TransformTargetRot = targetRot;
        }
        else
        {
            if (m_CamFixedPath != null)
            {
                if (m_Target.GetComponent<ThirdPersonCharacter>())
                {
                    if (m_Target.GetComponent<ThirdPersonCharacter>().m_IsClimbing || !m_CamFixedPath.climbOnly)
                        m_CameraLockedForBoss = true;
                    else
                        m_CameraLockedForBoss = false;
                }
            }

            HandleRotationMovement();
            m_ProtectFromWall.enabled = !m_CameraLockedForBoss;

        }

        if (m_LockCursor && Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
        }

        if (GameController.instance.playerController.m_IsClimbing || GameController.instance.playerController.m_IsGroundedOnBoss)
            m_UpdateType = UpdateType.LateUpdate;
        else
            m_UpdateType = UpdateType.FixedUpdate;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    protected override void FollowTarget(float deltaTime)
    {
        if (m_Target == null) return;

        if (!m_CameraLockedForBoss)
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * m_MoveSpeed);
        else
            FollowFixedPath();
    }

    private void FollowFixedPath()
    {
        Vector3 tPos = m_CamFixedPath.GetPosition(m_Target.position);
        if (m_CamFixedPath.camDirection == null)
            tPos = tPos - Vector3.ProjectOnPlane(m_Target.forward, Vector3.up) * 5f;
        else
            tPos = tPos - m_CamFixedPath.camDirection.forward * 5f;


        this.transform.position = Vector3.Lerp(transform.position, tPos, Time.deltaTime * 3f);
    }

    private void HandleRotationMovement()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        var x = CrossPlatformInputManager.GetAxis("Mouse X");
        var y = CrossPlatformInputManager.GetAxis("Mouse Y");

        m_LookAngle += x * m_TurnSpeed;
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

        m_TiltAngle -= y * m_TurnSpeed;
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
        m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

        if (m_TurnSmoothing > 0)
        {
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
        }
        else
        {
            m_Pivot.localRotation = m_PivotTargetRot;
            transform.localRotation = m_TransformTargetRot;
        }
    }

    public void SetFixedCam(CameraFixedPath fixPath)
    {
        m_CamFixedPath = fixPath;

        if (fixPath == null)
            m_CameraLockedForBoss = false;
    }
}
