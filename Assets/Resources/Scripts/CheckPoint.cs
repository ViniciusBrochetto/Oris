using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private int CheckpointNumber = -1;

    private void Awake()
    {
        if (CheckpointNumber == -1)
            Destroy(this);

        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            int lastCheckpoint = PlayerPrefs.GetInt(CheckpointController.CHECKPOINT_SAVE, 0);
            if (lastCheckpoint >= CheckpointNumber)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CheckpointController.SetLastCheckpoint(CheckpointNumber);
        }
    }
}
