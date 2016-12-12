using UnityEngine;
using System.Collections;

public class GameAudioController : MonoBehaviour
{
    public AudioSource m_BattleTheme;

    public void PlayBattleTheme(bool fromBeggining)
    {
        if (!m_BattleTheme.isPlaying)
        {
            if (fromBeggining)
                m_BattleTheme.time = 0f;

            m_BattleTheme.Play();
        }
    }

    public void PauseBattleTheme()
    {
        if (m_BattleTheme.isPlaying)
            m_BattleTheme.Pause();
    }

    public static IEnumerator AudioFade(AudioSource source)
    {
        yield return 0;
    }
}
