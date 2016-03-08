using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]    private Animator m_Animator;

    [SerializeField]    private Transform m_Hip;
    [SerializeField]    private Transform m_Spine;
    [SerializeField]    private Transform m_Head;

    [SerializeField]    private Transform m_LUpperLeg;
    [SerializeField]    private Transform m_LLowerLeg;
    [SerializeField]    private Transform m_RUpperLeg;
    [SerializeField]    private Transform m_RLowerLeg;

    [SerializeField]    private Transform m_LUpperArm;
    [SerializeField]    private Transform m_LLowerArm;
    [SerializeField]    private Transform m_RUpperArm;
    [SerializeField]    private Transform m_RLowerArm;

    public void SetRagdollActive(bool active)
    {
        if(m_Animator)
        {
            m_Animator.enabled = !active;
        }
        Rigidbody r = GetComponent<Rigidbody>();

        m_Hip.GetComponent<Rigidbody>().useGravity = active;
        m_Hip.GetComponent<Rigidbody>().velocity = r.velocity;
        m_Hip.GetComponent<Collider>().enabled = active;


        m_Spine.GetComponent<Rigidbody>().useGravity = active;
        m_Spine.GetComponent<Rigidbody>().velocity = r.velocity;
        m_Spine.GetComponent<Collider>().enabled = active;

        m_Head.GetComponent<Rigidbody>().useGravity = active;
        m_Head.GetComponent<Rigidbody>().velocity = r.velocity;
        m_Head.GetComponent<Collider>().enabled = active;

        m_LUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LUpperLeg.GetComponent<Rigidbody>().velocity = r.velocity;
        m_LUpperLeg.GetComponent<Collider>().enabled = active;

        m_LLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LLowerLeg.GetComponent<Rigidbody>().velocity = r.velocity;
        m_LLowerLeg.GetComponent<Collider>().enabled = active;

        m_RUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RUpperLeg.GetComponent<Rigidbody>().velocity = r.velocity;
        m_RUpperLeg.GetComponent<Collider>().enabled = active;

        m_RLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RLowerLeg.GetComponent<Rigidbody>().velocity = r.velocity;
        m_RLowerLeg.GetComponent<Collider>().enabled = active;

        m_LUpperArm.GetComponent<Rigidbody>().useGravity = active;
        m_LUpperArm.GetComponent<Rigidbody>().velocity = r.velocity;
        m_LUpperArm.GetComponent<Collider>().enabled = active;

        m_LLowerArm.GetComponent<Rigidbody>().useGravity = active;
        m_LLowerArm.GetComponent<Rigidbody>().velocity = r.velocity;
        m_LLowerArm.GetComponent<Collider>().enabled = active;

        m_RUpperArm.GetComponent<Rigidbody>().useGravity = active;
        m_RUpperArm.GetComponent<Rigidbody>().velocity = r.velocity;
        m_RUpperArm.GetComponent<Collider>().enabled = active;

        m_RLowerArm.GetComponent<Rigidbody>().useGravity = active;
        m_RLowerArm.GetComponent<Rigidbody>().velocity = r.velocity;
        m_RLowerArm.GetComponent<Collider>().enabled = active;
    }
}
