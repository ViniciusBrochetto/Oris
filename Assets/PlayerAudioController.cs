using UnityEngine;
using System.Collections;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_AS_Jump;
    [SerializeField]
    private AudioClip[] m_AS_Walk;
    [SerializeField]
    private AudioClip[] m_AS_Damage;
    [SerializeField]
    private AudioClip[] m_AS_Roll;

    [SerializeField]
    private AudioSource m_AudioSource_Walk;
    [SerializeField]
    private AudioSource m_AudioSource_Jump;
    [SerializeField]
    private AudioSource m_AudioSource_Damage;
    [SerializeField]
    private AudioSource m_AudioSource_Roll;

    public void Walk()
    {
        RequestAudio(Type.Walk);
    }

    public void Jump()
    {
        RequestAudio(Type.Jump);
    }

    public void Damage()
    {
        RequestAudio(Type.Damage);
    }

    public void Roll()
    {
        RequestAudio(Type.Roll);
    }


    public void RequestAudio(Type t)
    {
        int r = 0;

        switch (t)
        {
            case Type.Walk:
                r = Random.Range(0, m_AS_Walk.Length);
                m_AudioSource_Walk.clip = m_AS_Walk[r];
                m_AudioSource_Walk.Play();
                break;
            case Type.Roll:
                r = Random.Range(0, m_AS_Roll.Length);
                m_AudioSource_Roll.clip = m_AS_Roll[r];
                m_AudioSource_Roll.Play();
                break;
            case Type.Jump:
                r = Random.Range(0, m_AS_Jump.Length);
                m_AudioSource_Jump.clip = m_AS_Jump[r];
                m_AudioSource_Jump.Play();
                break;
            case Type.Damage:
                r = Random.Range(0, m_AS_Damage.Length);
                m_AudioSource_Damage.PlayOneShot(m_AS_Damage[r]);
                break;
            default:
                break;
        }
    }

    public enum Type
    {
        Jump,
        Walk,
        Damage,
        Roll
    }
}
