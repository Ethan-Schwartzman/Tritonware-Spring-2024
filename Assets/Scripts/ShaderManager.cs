using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public static ShaderManager Instance;

    public Material ChromaticAberration;
    private const float DEFAULT_ABERRATION = 0.001f;
    private const float MAX_ABERRATION = 0.0075f;
    private const float ABERRATION_RATE = 0.0005f;


    public Transform CameraTransform;
    private const float CAMERA_DAMPING = 1.0f;


    private Coroutine hitEffect;

    public Transform PlayerTransform;

    void Start()
    {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of ShaderManager");
            Destroy(this);
        }

        ChromaticAberration.SetFloat("_intensity", DEFAULT_ABERRATION);
    }

    public void HitEffect() {
        if(hitEffect != null) StopCoroutine(hitEffect);
        hitEffect = StartCoroutine(HitCoroutine()); 
    }

    private IEnumerator HitCoroutine() {
        float intensity = MAX_ABERRATION;

        while(intensity > 0) {
            intensity -= ABERRATION_RATE;
            if(intensity < DEFAULT_ABERRATION) intensity = DEFAULT_ABERRATION;
            ChromaticAberration.SetFloat("_intensity", intensity);
            yield return new WaitForSeconds(0.01f);
        }
        
        hitEffect = null;
    }

    public void HyperspaceEffect(float duration) {
        StartCoroutine(HyperspaceCoroutine(duration)); 
    }

    private IEnumerator HyperspaceCoroutine(float duration) {
        if(hitEffect != null) StopCoroutine(hitEffect);

        Vector3 initialPosition = new Vector3(0, 0, -10f);

        float intensity = 0;
        float startTime = Time.time;
        bool playerReset = false;

        while(Time.time - startTime < duration) {
            // Aberration
            float t = (Time.time - startTime) / duration;
            t = -1.0f + 2*t;
            intensity = 1.0f - Mathf.Pow(Mathf.Abs(Mathf.Sin(Mathf.PI * t / 2.0f)), 2.5f);
            ChromaticAberration.SetFloat("_intensity", intensity * MAX_ABERRATION);

            // Screen shake
            CameraTransform.localPosition = initialPosition + Random.insideUnitSphere * intensity;

            yield return null;

            if (Time.time - startTime > duration * 0.5f && !playerReset) {
                PlayerTransform.position = new Vector3(-80, 0, 0);
                ThreatController.Instance.ResetPursuit();
                playerReset = true;
                //Debug.Log("beepboop done");
            }
        }

        CameraTransform.localPosition = initialPosition;
        ChromaticAberration.SetFloat("_intensity", DEFAULT_ABERRATION);
    }
}
