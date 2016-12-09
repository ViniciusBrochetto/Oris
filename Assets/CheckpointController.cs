using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour
{
    public static string CHECKPOINT_SAVE = "LastCheckpoint";

    [SerializeField]
    private Transform[] m_SpawnLocationsTutorial;

    [SerializeField]
    private Transform[] m_SpawnLocationsMainLevel;

    public Transform GetCheckpointPosition()
    {
        return m_SpawnLocationsMainLevel[GetLastCheckpoint()];
    }

    public static void SetLastCheckpoint(int c)
    {
        Debug.Log("New checkpoint set (code " + c.ToString() + ")");
        PlayerPrefs.SetInt(CHECKPOINT_SAVE, c);
    }

    public static int GetLastCheckpoint()
    {
        return PlayerPrefs.GetInt(CHECKPOINT_SAVE, 0);
    }
}
