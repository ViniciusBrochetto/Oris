using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private int m_IdTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.instance.tutorialController.ShowTutorial(m_IdTutorial);
            Destroy(gameObject);
        }
    }
}
