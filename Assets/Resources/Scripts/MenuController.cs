using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static bool LOAD_CREDITS = false;

    public Button btn_Continue;

    public GameObject pnl_Credits;
    public GameObject pnl_Menu;


    // Use this for initialization
    void Awake()
    {
        btn_Continue.interactable = CheckpointController.GetLastCheckpoint() != 0;
        Cursor.visible = true;

        if (LOAD_CREDITS)
        {
            pnl_Credits.SetActive(true);
            pnl_Menu.SetActive(false);
        }
        else
        {
            pnl_Credits.SetActive(false);
            pnl_Menu.SetActive(true);
        }

        LOAD_CREDITS = false;
    }

    #region GAME_START/LOAD/OPTIONS/QUIT
    public void StartNewGame()
    {
        CutsceneController.PLAY_END_GAME = false;
        SceneManager.LoadScene("LoadingGame");
        LoadingController.LEVEL_TO_LOAD = 0;
        CheckpointController.SetLastCheckpoint(0);
        Cursor.visible = false;
    }

    public void ContinueGame()
    {
        CutsceneController.PLAY_END_GAME = false;
        SceneManager.LoadScene("LoadingGame");
        LoadingController.LEVEL_TO_LOAD = 1;
        Cursor.visible = false;
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
