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

    public void RequestShake()
    {
        if (shakeCR != null)
            StopCoroutine(shakeCR);

        shakeCR = StartCoroutine(ProcessShake(shakeDuration, shakeIntensity, doShakeDecay));
    }

    public void RequestShake(float duration, float intensity, bool decay)
    {
        if (shakeCR != null)
            StopCoroutine(shakeCR);

        shakeCR = StartCoroutine(ProcessShake(duration, intensity, decay));
    }

    IEnumerator ProcessShake(float duration, float intensity, bool decay)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        Vector3 slowPos = Camera.main.transform.localPosition;
        Quaternion originalRot = Camera.main.transform.localRotation;


        float decayPerSec = intensity / duration;
        float slowTimer = duration - slowPositionShakeTime;
        while (duration > 0)
        {
            if (doShakePosition)
            {
                if (!doSlowPositionShake)
                {
                    Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * intensity;
                }
                else
                {
                    if (slowTimer > duration)
                    {
                        slowPos = originalPos + Random.insideUnitSphere * 2f;
                    }

                    Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, slowPos, Time.deltaTime * slowPositionShakeTime);
                }
            }
        }
        if (doShakeRotation)
        {
            Quaternion q = new Quaternion(originalRot.x + Random.Range(-intensity, intensity) * .2f,
                                                originalRot.y + Random.Range(-intensity, intensity) * .2f,
                                                originalRot.z + Random.Range(-intensity, intensity) * .2f,
                                                originalRot.w + Random.Range(-intensity, intensity) * .2f);



            Camera.main.transform.localRotation = q;
        }

        if (decay)
            intensity -= decayPerSec * Time.deltaTime;

        duration -= Time.deltaTime;

        yield return null;
    }
}
