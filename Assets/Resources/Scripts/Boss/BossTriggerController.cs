using UnityEngine;
using System.Collections;

public class BossTriggerController : MonoBehaviour
{
    private void Awake()
    {
        if (CheckpointController.GetLastCheckpoint() >= 6)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.audioController.PlayBattleTheme(true);
            GameController.instance.bossController.PlayerNearby(true);
            Destroy(gameObject);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            GameController.instance.bossController.PlayerNearby(false);
    }
}
