using UnityEngine;
using System.Collections;

public class TeleportGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GameController.instance.TeleportPlayer());
        }
    }
}
