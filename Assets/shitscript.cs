using UnityEngine;
using System.Collections;

public class shitscript : MonoBehaviour {

    SkinnedMeshRenderer rend;
    MeshCollider rend1;

    // Use this for initialization
    void Start () {
        rend = GetComponent<SkinnedMeshRenderer>();
        rend1 = GetComponent<MeshCollider>();

    }
	
	// Update is called once per frame
	void LateUpdate () {
        Mesh colliderMesh = new Mesh();
        rend.BakeMesh(colliderMesh);
        rend1.sharedMesh = null;
        rend1.sharedMesh = colliderMesh;
    }
}
