using UnityEngine;
using System.Collections;

public class PrototypeControllers : MonoBehaviour
{
    bool ragdolls;
    bool ik;

    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.F1))
        {
            ragdolls = !ragdolls;
            GameObject.FindObjectOfType<RagdollController>().SetRagdollActive(ragdolls);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ik = !ik;
            GameObject.FindObjectOfType<IKController>().ikActive = ik;
        }
    }
}
