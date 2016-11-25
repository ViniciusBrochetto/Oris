using UnityEngine;
using System.Collections;

public class shitscript : MonoBehaviour
{
    SkinnedMeshRenderer m_SkinnedMesh;
    MeshCollider m_MeshCollider;
    Mesh m_MeshToMakeCollisionFrom;

    public Transform m_ParentBone;
    public PhysicMaterial m_PhysMat;

    // Use this for initialization
    void Start()
    {
        m_SkinnedMesh = GetComponent<SkinnedMeshRenderer>();
        m_MeshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Destroy(m_MeshToMakeCollisionFrom);

        m_MeshToMakeCollisionFrom = new Mesh();
        m_SkinnedMesh.BakeMesh(m_MeshToMakeCollisionFrom);

        m_MeshCollider.sharedMesh = null;
        m_MeshCollider.sharedMesh = m_MeshToMakeCollisionFrom;
        m_MeshCollider.sharedMaterial = m_PhysMat;
    }
}
