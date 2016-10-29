using UnityEngine;
using System.Collections;

public class BossTriggerController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            GameController.instance.bossController.canAttack = true;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            GameController.instance.bossController.canAttack = false;
    }
}
