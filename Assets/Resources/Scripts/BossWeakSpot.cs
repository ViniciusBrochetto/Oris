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
        GameController.instance.bossController.SetPhase(m_BossPhase);
    }
}
