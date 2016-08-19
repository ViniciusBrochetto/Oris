using UnityEngine;
using System.Collections;

public class GameControllerProperties : MonoBehaviour
{
    public GameState gameState;
}

public enum GameState
{
    MainMenu,
    Paused,
    Playing;
}
