using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private Transform m_Hip;
    [SerializeField]
    private Transform m_Spine;
    [SerializeField]
    private Transform m_Head;

    [SerializeField]
    private Transform m_LUpperLeg;
    [SerializeField]
    private Transform m_LLowerLeg;
    [SerializeField]
    private Transform m_RUpperLeg;
    [SerializeField]
    private Transform m_RLowerLeg;

    [SerializeField]
    private Transform m_LUpperArm;
    [SerializeField]
    private Transform m_LLowerArm;
    [SerializeField]
    private Transform m_RUpperArm;
    [SerializeField]
    private Transform m_RLowerArm;

    public bool isRagdollActive = false;

    public void Start()
    {
        SetFullRagdollActive(false);
    }

    public void SetFullRagdollActive(bool active)
    {
        isRagdollActive = active;

        if (m_Animator)
        {
            m_Animator.enabled = !active;
        }

        float force = 5000f;

        m_Hip.GetComponent<Rigidbody>().useGravity = active;
        m_Hip.GetComponent<Rigidbody>().isKinematic = !active;
        m_Hip.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_Hip.GetComponent<Collider>().enabled = active;


        m_Spine.GetComponent<Rigidbody>().useGravity = active;
        m_Spine.GetComponent<Rigidbody>().isKinematic = !active;
        m_Spine.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_Spine.GetComponent<Collider>().enabled = active;

        m_Head.GetComponent<Rigidbody>().useGravity = active;
        m_Head.GetComponent<Rigidbody>().isKinematic = !active;
        m_Head.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_Head.GetComponent<Collider>().enabled = active;

        m_LUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LUpperLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_LUpperLeg.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_LUpperLeg.GetComponent<Collider>().enabled = active;

        m_LLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LLowerLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_LLowerLeg.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_LLowerLeg.GetComponent<Collider>().enabled = active;

        m_RUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RUpperLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_RUpperLeg.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_RUpperLeg.GetComponent<Collider>().enabled = active;

        m_RLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RLowerLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_RLowerLeg.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_RLowerLeg.GetComponent<Collider>().enabled = active;

        m_LUpperArm.GetComponent<Rigidbody>().useGravity = active;
        m_LUpperArm.GetComponent<Rigidbody>().isKinematic = !active;
        m_LUpperArm.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_LUpperArm.GetComponent<Collider>().enabled = active;

        m_LLowerArm.GetComponent<Rigidbody>().useGravity = active;
        m_LLowerArm.GetComponent<Rigidbody>().isKinematic = !active;
        m_LLowerArm.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_LLowerArm.GetComponent<Collider>().enabled = active;

        m_RUpperArm.GetComponent<Rigidbody>().useGravity = active;
        m_RUpperArm.GetComponent<Rigidbody>().isKinematic = !active;
        m_RUpperArm.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_RUpperArm.GetComponent<Collider>().enabled = active;

        m_RLowerArm.GetComponent<Rigidbody>().useGravity = active;
        m_RLowerArm.GetComponent<Rigidbody>().isKinematic = !active;
        m_RLowerArm.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        m_RLowerArm.GetComponent<Collider>().enabled = active;
    }

    public void SetRagdollActive(bool active)
    {
        isRagdollActive = active;

        if (m_Animator)
        {
            m_Animator.enabled = !active;
        }

        m_Hip.GetComponent<Rigidbody>().useGravity = active;
        m_Hip.GetComponent<Rigidbody>().isKinematic = !active;
        m_Hip.GetComponent<Collider>().enabled = active;


        m_Spine.GetComponent<Rigidbody>().useGravity = active;
        m_Spine.GetComponent<Rigidbody>().isKinematic = !active;
        m_Spine.GetComponent<Collider>().enabled = active;

        m_LUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LUpperLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_LUpperLeg.GetComponent<Collider>().enabled = active;

        m_LLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_LLowerLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_LLowerLeg.GetComponent<Collider>().enabled = active;

        m_RUpperLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RUpperLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_RUpperLeg.GetComponent<Collider>().enabled = active;

        m_RLowerLeg.GetComponent<Rigidbody>().useGravity = active;
        m_RLowerLeg.GetComponent<Rigidbody>().isKinematic = !active;
        m_RLowerLeg.GetComponent<Collider>().enabled = active;
    }
}
