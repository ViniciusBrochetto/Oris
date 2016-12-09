using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CheckpointController.GetLastCheckpoint() == 9)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        CutsceneController.PLAY_END_GAME = true;
        GameController.instance.isPausable = false;
        GameController.instance.isPlayerControllable = false;
        GameController.instance.cameraController.RequestFadeToBlack();
        yield return new WaitForSeconds(2f);
        LoadingController.LEVEL_TO_LOAD = 0;
        SceneManager.LoadScene("LoadingGame");
    }
}
