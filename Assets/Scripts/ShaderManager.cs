using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public static ShaderManager Instance;

    public Material ChromaticAberration;
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

        ChromaticAberration.SetFloat("_intensity", 0);
    }

    public void HitEffect() {
        if(hitEffect != null) StopCoroutine(hitEffect);
        hitEffect = StartCoroutine(HitCoroutine()); 
    }

    private IEnumerator HitCoroutine() {
        float intensity = 0.005f;
        float rate = 0.0005f;

        while(intensity > 0) {
            intensity -= rate;
            if(intensity < 0) intensity = 0;
            ChromaticAberration.SetFloat("_intensity", intensity);
            yield return new WaitForSeconds(0.01f);
        }
        
        hitEffect = null;
    }
}
