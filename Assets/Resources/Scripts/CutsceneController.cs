using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public static bool PLAY_END_GAME = false;

    public Transform[] m_Stills;
    public Transform[] m_StillPositions;
    public Camera m_Camera;
    public Image m_FadeImage;

    public AudioSource m_Background;
    public AudioSource m_BackgroundFloresta;
    public AudioSource m_BackgroundBoss;
    public AudioSource m_BossBattle;
    public AudioSource m_BackgroundFim;

    private bool skip = false;

    private void Awake()
    {
        m_FadeImage.enabled = true;
        m_FadeImage.color = new Color(0f, 0f, 0f, 1f);

        if (!PLAY_END_GAME)
            StartCoroutine(PlayCutscene());
        else
            StartCoroutine(PlayEndCutscene());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) && !skip)
        {
            skip = true;

            StopAllCoroutines();
            StartCoroutine(FadeToBlack(1f));

            if (PLAY_END_GAME)
            {
                LoadingController.LEVEL_TO_LOAD = -1;
                CheckpointController.SetLastCheckpoint(0);
                MenuController.LOAD_CREDITS = true;
            }
            else
                LoadingController.LEVEL_TO_LOAD = 1;

            SceneManager.LoadScene("LoadingGame");
        }
    }

    private IEnumerator PlayEndCutscene()
    {
        int idx = 8;

        m_BackgroundFim.Play();

        //-------------- Still 00 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(Vector3.forward, 8f, 0.1f));
        yield return StartCoroutine(FadeFromBlack(4f));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        idx = 9;

        //-------------- Still 00 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(Vector3.forward, 8f, 0.1f));
        yield return StartCoroutine(FadeFromBlack(2f));
        yield return new WaitForSeconds(6.3f);
        yield return StartCoroutine(FadeToBlack(1.2f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------



        yield return StartCoroutine(FadeToBlack(1.2f));
        yield return new WaitForSeconds(2f);
        CheckpointController.SetLastCheckpoint(0);

        SceneManager.LoadScene("MainMenu");
        MenuController.LOAD_CREDITS = true;

        yield return 0;
    }

    private IEnumerator PlayCutscene()
    {
        int idx = 0;

        m_Background.Play();

        //-------------- Still 00 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(-m_Camera.transform.right, 10f, 1f));
        yield return StartCoroutine(FadeFromBlack(5f));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 01 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(Vector3.forward, 5f, 1f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 02 ------------------------
        SetupCamPosition(idx);

        m_BackgroundFloresta.Play();

        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(-Vector3.forward + Vector3.down * 0.2f, 7f, 1f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 03 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(Vector3.forward, 5f, 0.3f));
        yield return StartCoroutine(FadeFromBlack(1f));

        m_BackgroundBoss.Play();
        m_BossBattle.Play();
        m_Background.Stop();
        m_BackgroundFloresta.Stop();

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 04 ------------------------
        SetupCamPosition(idx);


        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(Vector3.forward, 5f, 0.5f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 05 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(RotateCamera(-Vector3.right * 20f, 3f, 0.1f));
        StartCoroutine(TranslateCamera(-m_Camera.transform.forward, 10f, 0.05f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RotateCamera(-Vector3.right * 25f, 2.5f, 0.1f));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(RotateCamera(-Vector3.right * 30f, 4.5f, 0.1f));
        yield return new WaitForSeconds(3.5f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 06 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(m_Camera.transform.right, 5f, 1f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        idx++;
        //------------------------------------------------


        //-------------- Still 07 ------------------------
        SetupCamPosition(idx);
        m_Stills[idx].gameObject.SetActive(true);
        StartCoroutine(TranslateCamera(m_Camera.transform.right, 5f, 1f));
        yield return StartCoroutine(FadeFromBlack(1f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeToBlack(1f));
        m_Stills[idx].gameObject.SetActive(false);
        //------------------------------------------------


        yield return StartCoroutine(FadeToBlack(1f));
        LoadingController.LEVEL_TO_LOAD = 1;
        CheckpointController.SetLastCheckpoint(0);
        SceneManager.LoadScene("LoadingGame");

        yield return 0;
    }

    private IEnumerator FadeToBlack(float duration)
    {
        Color blackA0 = new Color(0f, 0f, 0f, 0f);
        Color blackA1 = new Color(0f, 0f, 0f, 1f);

        float time = 0f;
        float timeMax = duration;

        m_FadeImage.enabled = true;

        while (time <= timeMax)
        {
            m_FadeImage.color = Color.Lerp(blackA0, blackA1, time / timeMax);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        m_FadeImage.color = blackA1;

        yield return 0;
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        Color blackA0 = new Color(0f, 0f, 0f, 0f);
        Color blackA1 = new Color(0f, 0f, 0f, 1f);

        float time = 0f;
        float timeMax = duration;

        time = 0f;
        while (time <= timeMax)
        {
            m_FadeImage.color = Color.Lerp(blackA1, blackA0, time / timeMax);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_FadeImage.color = blackA0;

        m_FadeImage.enabled = false;

        yield return 0;
    }

    private void SetupCamPosition(int index)
    {
        m_Camera.transform.position = m_StillPositions[index].position;
        m_Camera.transform.rotation = m_StillPositions[index].rotation;
    }

    private IEnumerator TranslateCamera(Vector3 direction, float time, float speed)
    {
        float t = 0f;
        while (t < time)
        {
            m_Camera.transform.Translate(direction * Time.deltaTime * speed);
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }

    private IEnumerator RotateCamera(Vector3 direction, float time, float speed)
    {
        float t = 0f;
        while (t < time)
        {
            m_Camera.transform.Rotate(direction * Time.deltaTime * speed);
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }

    private IEnumerator FromAtoB(Vector3 posA, Vector3 posB, float duration)
    {
        float t = 0f;
        while (m_Camera.transform.position != posB)
        {
            m_Camera.transform.position = Vector3.Lerp(posA, posB, t / duration);
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
