﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class BossController : MonoBehaviour
{
    public BossPhases m_BossPhase = BossPhases.f0;

    private bool m_PhaseUpdated;
    public Animator m_Anim;

    [SerializeField]
    private float m_ShakeAvgTime = 10f;
    [SerializeField]
    private float m_AttackAvgTime = 10f;

    public bool isAttacking = false;
    public bool isShaking = false;
    public bool isPlayingIntro = false;
    public bool isPlayerNearby = false;
    public bool isTaunting = false;

    public bool canAttack = false;
    public bool canShake = false;

    [SerializeField]
    private Transform[] m_Particles;

    [SerializeField]
    private Transform[] m_ParticlePositions;

    [SerializeField]
    private Transform[] m_CameraPositions;

    [SerializeField]
    public Transform m_Mask;
    [SerializeField]
    public Transform m_MaskBroken;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (m_BossPhase)
        {
            case BossPhases.f0:
                if (canAttack && !isAttacking && !isTaunting)
                {
                    canAttack = false;
                    m_Anim.SetTrigger("start_attack");
                    StartCoroutine(AttackCooldown());
                }
                break;
            case BossPhases.f1:
                if (canShake && !isShaking && GameController.instance.playerController.m_IsClimbing && GameController.instance.cameraController.m_CameraLockedForBoss)
                {
                    canShake = false;
                    m_Anim.SetTrigger("start_shake");
                    StartCoroutine(ShakeCooldown());
                }
                break;
            case BossPhases.f2:
                if (canShake && !isShaking && !GameController.instance.playerController.m_IsClimbing)
                {
                    canShake = false;
                    m_Anim.SetTrigger("start_shake");
                    StartCoroutine(ShakeCooldown());
                }
                break;
            case BossPhases.f3:
                break;
            case BossPhases.f4:
                if (canAttack && !isAttacking)
                {
                    canAttack = false;
                    m_Anim.SetTrigger("start_attack");
                    StartCoroutine(AttackCooldown());
                }
                break;
            case BossPhases.f5:
                break;
            default:
                break;
        }

    }

    private IEnumerator AttackCooldown()
    {
        while (isAttacking)
            yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(m_AttackAvgTime + (Random.value - 0.5f) * 4f);

        canAttack = true;
        yield return null;
    }

    private IEnumerator ShakeCooldown()
    {
        while (isShaking)
            yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(m_ShakeAvgTime + (Random.value - 0.5f) * 4f);

        if (!GameController.instance.playerController.m_IsClimbing)
            canShake = true;

        yield return null;
    }

    public void SetPhase(BossPhases phase)
    {
        isShaking = false;
        isAttacking = false;
        switch (phase)
        {
            case BossPhases.f0:
                break;
            case BossPhases.f1:
                break;
            case BossPhases.f2:
                StartCoroutine(BossDamageScene(BossPhases.f2));
                break;
            case BossPhases.f3:
                StartCoroutine(BossDamageScene(BossPhases.f3));
                break;
            case BossPhases.f4:
                StartCoroutine(BossDamageScene(BossPhases.f4));
                break;
            case BossPhases.f5:
                StartCoroutine(BossDamageScene(BossPhases.f5));
                break;
            default:
                break;
        }
        m_BossPhase = phase;
    }

    public void SetPhase(string tag)
    {
        if (tag == "Boss_StartF1" && m_BossPhase != BossPhases.f1)
        {
            isShaking = false;
            isAttacking = false;
            m_Anim.SetTrigger("start_f1");
            m_BossPhase = BossPhases.f1;
            canShake = true;
        }
    }

    public void SetPlayerClimbing(bool isClimbing)
    {
        m_Anim.SetBool("player_climbing", isClimbing);
    }

    public void setShaking(int i)
    {
        isShaking = i == 1;
    }

    public void setAttacking(int i)
    {
        isAttacking = i == 1;
    }

    public void setTaunting(int i)
    {
        isTaunting = i == 1;
    }

    public void PlayerNearby(bool b)
    {
        isPlayerNearby = b;

        if (b)
        {
            m_Anim.SetTrigger("boss_roar");
            isTaunting = true;
            canAttack = true;
        }
    }

    public void StartCameraShake()
    {
        GameController.instance.cameraShakeController.RequestShake(3f, 0.25f, true);
    }

    public void InstantiateParticle(int particle_Num)
    {
        Instantiate(m_Particles[particle_Num % 10], m_ParticlePositions[particle_Num / 10].position, Quaternion.identity);
    }

    public IEnumerator BossDamageScene(BossPhases phase)
    {
        GameController.instance.isPlayerControllable = false;
        GameController.instance.isCameraControllable = false;
        GameController.instance.isPausable = false;
        GameController.instance.playerController.m_CanDie = false;


        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(GameController.instance.cameraController.FadeToBlack());
        yield return new WaitForSeconds(2f);
        GameController.instance.playerController.gameObject.SetActive(false);

        switch (phase)
        {
            case BossPhases.f0:
                break;
            case BossPhases.f1:
                break;
            case BossPhases.f2:

                GameController.instance.cameraController.transform.position = m_CameraPositions[0].position;
                GameController.instance.cameraController.SetLookRotation(m_CameraPositions[0].rotation);
                GameController.instance.cameraController.SetFixedCam(null);

                m_Anim.SetTrigger("start_f2");


                yield return StartCoroutine(GameController.instance.cameraController.FadeFromBlack());
                yield return new WaitForSeconds(10f);

                GameController.instance.playerController.transform.position = GameController.instance.checkpointController.GetCheckpointPosition().position;
                GameController.instance.playerController.transform.rotation = GameController.instance.checkpointController.GetCheckpointPosition().rotation;

                break;
            case BossPhases.f3:
                GameController.instance.cameraController.transform.position = m_CameraPositions[1].position;
                GameController.instance.cameraController.SetLookRotation(m_CameraPositions[1].rotation);
                m_Anim.SetTrigger("start_f3");


                yield return StartCoroutine(GameController.instance.cameraController.FadeFromBlack());
                yield return new WaitForSeconds(10f);

                GameController.instance.playerController.transform.position = GameController.instance.checkpointController.GetCheckpointPosition().position;
                GameController.instance.playerController.transform.rotation = GameController.instance.checkpointController.GetCheckpointPosition().rotation;
                break;
            case BossPhases.f4:

                GameController.instance.cameraController.transform.position = m_CameraPositions[2].position;
                GameController.instance.cameraController.SetLookRotation(m_CameraPositions[2].rotation);


                m_Anim.SetTrigger("start_f4");

                yield return StartCoroutine(GameController.instance.cameraController.FadeFromBlack());
                yield return new WaitForSeconds(10f);

                GameController.instance.playerController.transform.parent = null;
                GameController.instance.playerController.transform.position = GameController.instance.checkpointController.GetCheckpointPosition().position;
                GameController.instance.playerController.transform.rotation = GameController.instance.checkpointController.GetCheckpointPosition().rotation;
                GameController.instance.bossController.canAttack = true;
                GameController.instance.bossController.isPlayerNearby = true;
                break;

            case BossPhases.f5:
                GameController.instance.playerController.transform.parent = null;
                GameController.instance.cameraController.transform.position = m_CameraPositions[2].position;
                GameController.instance.cameraController.SetLookRotation(m_CameraPositions[2].rotation);

                m_Anim.SetTrigger("end_game");


                yield return StartCoroutine(GameController.instance.cameraController.FadeFromBlack());
                yield return new WaitForSeconds(10f);

                GameController.instance.playerController.transform.position = GameController.instance.checkpointController.GetCheckpointPosition().position;
                GameController.instance.playerController.transform.rotation = GameController.instance.checkpointController.GetCheckpointPosition().rotation;
                break;
            default:
                break;
        }

        yield return StartCoroutine(GameController.instance.cameraController.FadeToBlack());

        GameController.instance.playerController.gameObject.SetActive(true);
        GameController.instance.isPlayerControllable = true;
        GameController.instance.isCameraControllable = true;
        GameController.instance.isPausable = true;
        isShaking = false;
        isAttacking = false;

        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(GameController.instance.cameraController.FadeFromBlack());

        GameController.instance.playerController.m_CanDie = true;

        yield return 0;
    }

    public void BreakMask()
    {
        m_Mask.gameObject.SetActive(false);
        m_MaskBroken.gameObject.SetActive(true);
        m_MaskBroken.transform.parent = null;
    }

    public enum BossPhases
    {
        f0,
        f1,
        f2,
        f3,
        f4,
        f5
    }
}
