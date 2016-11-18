using UnityEngine;
using System.Collections;

public class DisableFrustumCulling : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //SkinnedMeshRenderer mesh = GetComponent<SkinnedMeshRenderer>();
        //mesh.bounds.SetMinMax(new Vector3(-Mathf.Infinity, Mathf.Infinity, -Mathf.Infinity), new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));
    }

    // Update is called once per frame
    void Update()
    {
        // boundsTarget is the center of the camera's frustum, in world coordinates:
        Vector3 camPosition = Camera.main.transform.position;
        Vector3 normCamForward = Vector3.Normalize(Camera.main.transform.forward);
        float boundsDistance = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2 + Camera.main.nearClipPlane;
        Vector3 boundsTarget = camPosition + (normCamForward * boundsDistance);

        // The game object's transform will be applied to the mesh's bounds for frustum culling checking.
        // We need to "undo" this transform by making the boundsTarget relative to the game object's transform:
        Vector3 realtiveBoundsTarget = this.transform.InverseTransformPoint(boundsTarget);

        // Set the bounds of the mesh to be a 1x1x1 cube (actually doesn't matter what the size is)
        SkinnedMeshRenderer mesh = GetComponent<SkinnedMeshRenderer>();
        mesh.sharedMesh.bounds = new Bounds(realtiveBoundsTarget, Vector3.one);
    }

}
