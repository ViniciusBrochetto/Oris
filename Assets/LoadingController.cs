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

        AsyncOperation loading;
        if (LEVEL_TO_LOAD == 0)
            loading = SceneManager.LoadSceneAsync("Cutscenes");
        else
            loading = SceneManager.LoadSceneAsync("MainGame");

        loading.allowSceneActivation = false;

        while (!loading.isDone)
        {
            m_Slider.value = Mathf.Lerp(m_Slider.value, ((loading.progress + 0.1f) * 100f) / 100f, Time.deltaTime * 3f);
            //m_Slider.value = (loading.progress * 100f) / 100f;

            if (m_Slider.value > 0.98f)
            {
                yield return new WaitForSeconds(1f);
                loading.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
