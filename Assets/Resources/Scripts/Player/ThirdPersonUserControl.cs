using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool m_JumpRelease;
    private bool m_Climb;
    private bool m_ClimbFixed;
    private bool m_Roll;
    private bool m_Interact;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
        if (GameController.instance.isPlayerControllable)
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!m_JumpRelease)
            {
                m_JumpRelease = CrossPlatformInputManager.GetButtonUp("Jump");
            }
            if (!m_Roll)
            {
                m_Roll = Input.GetKeyDown(KeyCode.V);
            }
            if (!m_Interact)
            {
                m_Interact = Input.GetKeyDown(KeyCode.F);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
            {
                m_ClimbFixed = !m_ClimbFixed;
            }

            m_Climb = !Input.GetKey(KeyCode.E);

            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            if (m_Character.m_IsClimbing)
            {
                m_CamForward = Vector3.Scale(m_Cam.up, new Vector3(0, 1, 0)).normalized;
                m_Move = v * m_CamForward + h * m_Character.transform.right;
            }
            else
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }

            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift))
                m_Move *= 0.5f;

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump, m_JumpRelease, m_Climb || m_ClimbFixed, m_Roll, m_Interact);
            m_Jump = false;
            m_JumpRelease = false;
            m_Roll = false;
            m_Interact = false;
        }
    }
}

