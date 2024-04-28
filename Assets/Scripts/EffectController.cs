using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    public static Color damageColor = Color.red;

    public static IEnumerator DamageEffect(SpriteRenderer sr)
    {
        if (sr.color != damageColor)
        {
            Color originalColor = sr.color;
            sr.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            if (sr != null) sr.color = originalColor;
        }
    }

    public void SpawnParticles(ParticleSystem particles, Transform t) {
        ParticleSystem ps = Instantiate(particles);
        ps.transform.position = t.position;
        ps.transform.localScale = t.localScale;
        StartCoroutine(this.ParticleCoroutine(ps));
    }

    public IEnumerator Hyperspace(ParticleSystem ps) {
        ps.Play();
        ShaderManager.Instance.HyperspaceEffect(ps.main.startLifetime.constantMax);
        yield return new WaitForSeconds(ps.main.startLifetime.constantMax);
        //Debug.Log("done");
    }

    private IEnumerator ParticleCoroutine(ParticleSystem ps) {
        ps.Play();
        yield return new WaitForSeconds(ps.main.startLifetime.constantMax);
        Destroy(ps.gameObject);
    }

    void Start()
    {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of EffectController");
            Destroy(this);
        }
    }
}
