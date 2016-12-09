using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button btn_Continue;


    // Use this for initialization
    void Awake()
    {
        btn_Continue.interactable = CheckpointController.GetLastCheckpoint() != 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region GAME_START/LOAD/OPTIONS/QUIT
    public void StartNewGame()
    {
        CutsceneController.PLAY_END_GAME = false;
        SceneManager.LoadScene("LoadingGame");
        LoadingController.LEVEL_TO_LOAD = 0;
        CheckpointController.SetLastCheckpoint(0);
    }

    public void ContinueGame()
    {
        CutsceneController.PLAY_END_GAME = false;
        SceneManager.LoadScene("LoadingGame");
        LoadingController.LEVEL_TO_LOAD = 1;
    }

    public void ApplyOptions()
    {
        //TODO Apply Options to XML
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
