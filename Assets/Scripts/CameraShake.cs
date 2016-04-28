using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool doShakeDecay;
    public bool doShakePosition;
    public bool doSlowPositionShake;
    public bool doShakeRotation;

    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.02f;
    public float slowPositionShakeTime;

    private Coroutine shakeCR;

    private Vector3 originalCamPos = Camera.main.transform.localPosition;
    private Quaternion originalCamRot = Camera.main.transform.localRotation;

    void Start()
    {
        originalCamPos = Camera.main.transform.localPosition;
        originalCamRot = Camera.main.transform.localRotation;
    }

    public void RequestShake()
    {
        if (shakeCR != null)
        {
            StopCoroutine(shakeCR);
        }

        shakeCR = StartCoroutine(ProcessShake(shakeDuration, shakeIntensity, doShakeDecay));
    }

    public void RequestShake(float duration, float intensity, bool decay)
    {

        if (shakeCR != null)
        {
            StopCoroutine(shakeCR);

            Camera.main.transform.localPosition = originalCamPos;
            Camera.main.transform.localRotation = originalCamRot;
        }

        originalCamPos = Camera.main.transform.localPosition;
        originalCamRot = Camera.main.transform.localRotation;
        shakeCR = StartCoroutine(ProcessShake(duration, intensity, decay));
    }

    IEnumerator ProcessShake(float duration, float intensity, bool decay)
    {
        Vector3 slowPos = Camera.main.transform.localPosition;


        float decayPerSec = intensity / duration;
        float slowTimer = duration - slowPositionShakeTime;

        while (duration > 0)
        {
            if (doShakePosition)
            {
                if (!doSlowPositionShake)
                {
                    Camera.main.transform.localPosition = originalCamPos + Random.insideUnitSphere * intensity;
                }
                else
                {
                    if (slowTimer > duration)
                    {
                        slowPos = originalCamPos + Random.insideUnitSphere * 2f;
                    }

                    Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, slowPos, Time.deltaTime * slowPositionShakeTime);
                }
            }

            if (doShakeRotation)
            {
                Quaternion q = new Quaternion(originalCamRot.x + Random.Range(-intensity, intensity) * .2f,
                                              originalCamRot.y + Random.Range(-intensity, intensity) * .2f,
                                              originalCamRot.z + Random.Range(-intensity, intensity) * .2f,
                                              originalCamRot.w + Random.Range(-intensity, intensity) * .2f);

                Camera.main.transform.localRotation = q;
            }

            if (decay)
                intensity -= decayPerSec * Time.deltaTime;

            duration -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
