using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static GameControllerProperties gameControllerProperties;

    public BossController bossController;
    public ThirdPersonCharacter playerController;
    public CameraShake cameraShakeController;
    public FreeLookCam cameraController;
    public CheckpointController checkpointController;
    public TutorialController tutorialController;
    public IInteractable interactable;

    public bool isCameraControllable = true;
    public bool isPlayerControllable = true;
    public bool isPausable = true;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            bossController = FindObjectOfType<BossController>();
            playerController = FindObjectOfType<ThirdPersonCharacter>();
            cameraShakeController = FindObjectOfType<CameraShake>();
            cameraController = FindObjectOfType<FreeLookCam>();
            checkpointController = FindObjectOfType<CheckpointController>();
            tutorialController = FindObjectOfType<TutorialController>();

            //CheckpointController.SetLastCheckpoint(5);

            StartCoroutine(LoadGame());
        }
        else
        {
            Debug.Log("Game Controller instance already exists.");
        }
    }

    #region PAUSE/RESUME/RETURN TO MENU/SAVE/LOAD
    public void PauseGame()
    {
        isCameraControllable = false;
        isPlayerControllable = false;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isCameraControllable = true;
        isPlayerControllable = true;
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        cameraController.RequestFadeToBlack();
        SceneManager.LoadScene("MainMenu");
        instance = null;
    }

    public IEnumerator LoadGame()
    {
        playerController.m_CanDie = false;
        yield return new WaitForSeconds(1f);

        isPlayerControllable = false;
        isCameraControllable = false;
        isPausable = false;

        int cp = CheckpointController.GetLastCheckpoint();
        Transform t = checkpointController.GetCheckpointPosition();

        //SET ANIMATIONS
        switch (cp)
        {
            case 6:
                bossController.m_BossPhase = BossController.BossPhases.f2;
                playerController.transform.parent = t.parent;
                bossController.m_Anim.Play("anm_boss_idle_f2");
                break;
            case 7:
                bossController.m_BossPhase = BossController.BossPhases.f3;
                playerController.transform.parent = t.parent;
                bossController.m_Anim.Play("anm_boss_idle_f3");
                break;
            case 8:
                bossController.m_BossPhase = BossController.BossPhases.f4;
                bossController.m_Anim.Play("anm_boss_idle_f4");
                break;
            case 9:
                bossController.m_BossPhase = BossController.BossPhases.f5;
                bossController.m_Anim.Play("anm_boss_ending_idle");
                bossController.m_Mask.gameObject.SetActive(false);
                bossController.m_MaskBroken.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        //Disable Interactions
        GameObject g;
        if (cp >= 6)
        {
            g = GameObject.Find("WeakSpotF1");
            g.GetComponent<BossWeakSpot>().isInteractable = false;
        }
        if (cp >= 7)
        {
            g = GameObject.Find("WeakSpotF2");
            g.GetComponent<BossWeakSpot>().isInteractable = false;
        }
        if (cp >= 8)
        {
            g = GameObject.Find("WeakSpotF3");
            g.GetComponent<BossWeakSpot>().isInteractable = false;
        }
        if (cp >= 9)
        {
            g = GameObject.Find("WeakSpotF4");
            g.GetComponent<BossWeakSpot>().isInteractable = false;
        }

        yield return new WaitForSeconds(1f);

        playerController.transform.position = t.position;
        playerController.transform.rotation = t.rotation;
        playerController.UpdateGroundHeight();

        cameraController.transform.position = t.position;
        cameraController.transform.rotation = t.rotation;
        cameraController.RequestFadeFromBlack();

        isPlayerControllable = true;
        isCameraControllable = true;
        isPausable = true;

        yield return new WaitForSeconds(1f);

        playerController.m_CanDie = true;

        yield return 0;
    }

    public IEnumerator TeleportPlayer()
    {
        CheckpointController.SetLastCheckpoint(4);
        isPlayerControllable = false;
        isCameraControllable = false;

        cameraController.RequestFadeToBlack();
        yield return new WaitForSeconds(3f);

        Transform t = checkpointController.GetCheckpointPosition();
        playerController.transform.position = t.position;
        playerController.transform.rotation = t.rotation;
        playerController.UpdateGroundHeight();

        cameraController.RequestFadeFromBlack();

        isPlayerControllable = true;
        isCameraControllable = true;
    }

    public void ReloadGame()
    {
        LoadingController.LEVEL_TO_LOAD = 1;
        SceneManager.LoadScene("LoadingGame");
    }

    #endregion
}