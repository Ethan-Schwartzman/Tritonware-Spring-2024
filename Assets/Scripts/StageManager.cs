using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
 
    public ParticleSystem HyperspaceParticles;

    private int stage;

    void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogWarning("Tried to create more than one instance of StageManager");
            Destroy(this);
        }

        stage = 1;
    }

    public void AdvanceStage() {
        stage++;
        StartCoroutine(AdvanceStageCoroutine());
    }

    private IEnumerator AdvanceStageCoroutine() {
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(EffectController.Instance.Hyperspace(HyperspaceParticles));
        ScoreManager.Instance.NextStage();
    }
}
