using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Transform m_Tutorial;
    public Text m_TutorialText;
    public string[] m_TutorialTexts;

    public float m_TutStartTime;

    public void Awake()
    {
        if (CheckpointController.GetLastCheckpoint() > 0)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && Time.unscaledTime - m_TutStartTime > 3f )
        {
            CancelTutorial();
        }
    }

    public void ShowTutorial(int tutorialIndex)
    {
        m_TutorialText.text = m_TutorialTexts[tutorialIndex];
        m_Tutorial.gameObject.SetActive(true);

        GameController.instance.isPlayerControllable = false;
        GameController.instance.isCameraControllable = false;
        GameController.instance.isPausable = false;
        Time.timeScale = 0f;

        m_TutStartTime = Time.unscaledTime;
    }

    private void CancelTutorial()
    {
        m_Tutorial.gameObject.SetActive(false);

        GameController.instance.isPlayerControllable = true;
        GameController.instance.isCameraControllable = true;
        GameController.instance.isPausable = true;
        Time.timeScale = 1f;
    }
}
