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
    private Coroutine hitEffect;

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
}
