using UnityEngine;
using System.Collections;
using System;

public class BossWeakSpot : MonoBehaviour, IInteractable
{
    [SerializeField]
    private BossController.BossPhases m_BossPhase;

    [SerializeField]
    private Transform animationPosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.instance.interactable = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.instance.interactable = null;
        }
    }

    public void Interact()
    {
        switch (m_BossPhase)
        {
            case BossController.BossPhases.f2:
                CheckpointController.SetLastCheckpoint(6);
                break;
            case BossController.BossPhases.f3:
                CheckpointController.SetLastCheckpoint(7);
                break;
            case BossController.BossPhases.f4:
                CheckpointController.SetLastCheckpoint(8);
                break;
            case BossController.BossPhases.f5:
                CheckpointController.SetLastCheckpoint(9);
                break;
            case BossController.BossPhases.f6:
                break;
            default:
                break;
        }

        GameController.instance.bossController.SetPhase(m_BossPhase);

        Destroy(gameObject);
    }
}
