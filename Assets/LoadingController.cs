using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public static int LEVEL_TO_LOAD = 0;

    [SerializeField]
    private Slider m_Slider;

    void Start()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {

        yield return new WaitForSeconds(2f);

        AsyncOperation loading = SceneManager.LoadSceneAsync("MainGame");

        while (!loading.isDone)
        {
            m_Slider.value = loading.progress;
            yield return new WaitForEndOfFrame();
        }

        loading.allowSceneActivation = true;


        yield return 0;
    }
}
