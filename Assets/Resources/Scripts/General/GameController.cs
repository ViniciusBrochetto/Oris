using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool START_FROM_CHECKPOINT = true;

    public static GameController instance;
    public static GameControllerProperties gameControllerProperties;

    public BossController bossController;
    public ThirdPersonCharacter playerController;
    public CameraShake cameraShakeController;
    public FreeLookCam cameraController;
    public CheckpointController checkpointController;
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

            CheckpointController.SetLastCheckpoint(1);

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
        yield return new WaitForSeconds(1f);

        isPlayerControllable = false;
        isCameraControllable = false;
        isPausable = false;

        if (START_FROM_CHECKPOINT)
        {
            int cp = CheckpointController.GetLastCheckpoint();
            Transform t = checkpointController.GetCheckpointPosition();


            switch (cp)
            {
                case 2:
                    bossController.m_BossPhase = BossController.BossPhases.f2;
                    bossController.m_Anim.Play("anm_boss_idle_f2");
                    break;
                case 3:
                    bossController.m_BossPhase = BossController.BossPhases.f3;
                    bossController.m_Anim.Play("anm_boss_idle_f3");
                    break;
                case 4:
                    bossController.m_BossPhase = BossController.BossPhases.f4;
                    bossController.m_Anim.Play("anm_boss_idle_f4");
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(1f);

            playerController.transform.position = t.position;
            playerController.transform.rotation = t.rotation;

            cameraController.transform.position = t.position;
            cameraController.transform.rotation = t.rotation;
        }


        cameraController.RequestFadeFromBlack();


        isPlayerControllable = true;
        isCameraControllable = true;
        isPausable = true;

        yield return 0;
    }

    #endregion
}