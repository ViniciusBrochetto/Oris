using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static GameControllerProperties gameControllerProperties;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("Game Controller instance already exists.");
    }

    #region PAUSE/RESUME/RETURN TO MENU/SAVE
    public void PauseGame()
    {
        gameControllerProperties.gameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        gameControllerProperties.gameState = GameState.Playing;
    }

    public void ReturnToMainMenu()
    {
        SaveGame();
    }

    public void SaveGame()
    {

    }
    #endregion

    #region GAME_START/LOAD/OPTIONS/QUIT
    public void StartGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadGame()
    {
        //TODO Loading Game
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