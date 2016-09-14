using UnityEngine;
using System.Collections;

public class PrototypeControllers : MonoBehaviour
{
    bool ragdolls;
    bool ik;

    void LateUpdate()
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
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameObject.FindObjectOfType<CameraShake>().RequestShake();
        }
    }
}
