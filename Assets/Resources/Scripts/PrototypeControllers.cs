using UnityEngine;
using System.Collections;

public class PrototypeControllers : MonoBehaviour
{
    bool ragdolls;
    bool ik;

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && GameController.instance.isPausable)
        {
            GameController.instance.PauseGame();
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            ragdolls = !ragdolls;
            GameObject.FindObjectOfType<RagdollController>().SetFullRagdollActive(ragdolls);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ik = !ik;
            GameObject.FindObjectOfType<IKController>().ikActive = ik;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameController.instance.cameraShakeController.RequestShake();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            StartCoroutine(GameController.instance.bossController.BossDamageScene(BossController.BossPhases.f5));
        }
    }
}
