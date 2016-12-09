using UnityEngine;
using System.Collections;

public class DepenetrationController : MonoBehaviour {
    public float maxVelocity;
    private void Awake()
    {
        GetComponent<Rigidbody>().maxDepenetrationVelocity = maxVelocity;
    }
}
