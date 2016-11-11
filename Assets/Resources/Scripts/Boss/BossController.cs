using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class BossController : MonoBehaviour
{
    public BossPhases m_BossPhase = BossPhases.f0;

    private bool m_PhaseUpdated;
    private Animator m_Anim;

    [SerializeField]
    private float m_ShakeAvgTime = 10f;
    [SerializeField]
    private float m_AttackAvgTime = 10f;

    public bool isAttacking = false;
    public bool isShaking = false;
    public bool isPlayingIntro = false;

    public bool canAttack = false;
    public bool canShake = false;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (m_BossPhase)
        {
            case BossPhases.f0:
                if (canAttack && !isAttacking)
                {
                    canAttack = false;
                    m_Anim.SetTrigger("start_attack");
                    StartCoroutine(AttackCooldown());
                }
                break;
            case BossPhases.f1:
                if (canAttack && !isShaking)
                {
                    m_BossPhase = BossPhases.f0;
                }
                else if (canShake && !isShaking && GameController.instance.playerController.m_IsClimbing && GameController.instance.cameraController.m_CameraLockedForBoss)
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
                break;
            case BossPhases.f5:
                break;
            case BossPhases.f6:
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
        switch (phase)
        {
            case BossPhases.f0:
                break;
            case BossPhases.f1:
                break;
            case BossPhases.f2:
                m_Anim.SetTrigger("start_f2");
                isShaking = false;
                isAttacking = false;
                break;
            case BossPhases.f3:
                m_Anim.SetTrigger("start_f3");
                isShaking = false;
                isAttacking = false;
                break;
            case BossPhases.f4:
                m_Anim.SetTrigger("start_f4");
                isShaking = false;
                isAttacking = false;
                break;
            case BossPhases.f5:
                m_Anim.SetTrigger("start_f5");
                isShaking = false;
                isAttacking = false;
                break;
            case BossPhases.f6:
                m_Anim.SetTrigger("start_f6");
                isShaking = false;
                isAttacking = false;
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

    public void StartCameraShake()
    {
        GameController.instance.cameraShakeController.RequestShake(3f, 0.25f, true);
    }

    public enum BossPhases
    {
        f0,
        f1,
        f2,
        f3,
        f4,
        f5,
        f6
    }
}
