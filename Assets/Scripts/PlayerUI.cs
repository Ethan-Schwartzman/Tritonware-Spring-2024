using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance;
    [SerializeField] TMP_Text health, maxHealth, speed,
        missileWarning,
        powerup, popupText;
    [SerializeField] ProgressBar powerupDuration, playerProgress, enemyProgress;
    [SerializeField] Image segment;


    Coroutine currentPopup;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        speed.text = Mathf.RoundToInt(PlayerShip.Instance.GetVelocity().magnitude).ToString();
        playerProgress.SetLevel(Mathf.Clamp01(ThreatController.Instance.GetPlayerProgress() / Settings.Instance.sectorDistance));
        enemyProgress.SetLevel(Mathf.Clamp01(ThreatController.Instance.GetEnemyProgress() / Settings.Instance.sectorDistance));

        Powerup pow = PlayerShip.Instance.currentPowerup;
        if (pow != null)
        {
            float percent = ((pow.GetDuration() - pow.activatedDuration) / pow.GetDuration()) / (float)pow.maxCharges
                + (float)(pow.charges-1) / (float)pow.maxCharges;
            powerupDuration.SetLevel(percent);
        }
        else powerupDuration.SetLevel(0);
        
    }

    public void UpdateUI()
    {
        health.text = PlayerShip.Instance.GetHealth().ToString();
        maxHealth.text = PlayerShip.Instance.GetMaxHealth().ToString();
        if (PlayerShip.Instance.currentPowerup == null) powerup.text = "";
        else powerup.text = PlayerShip.Instance.currentPowerup.GetName();
    }

    public void SetPowerupCharges(int charges)
    {
        powerupDuration.SetSegments(charges);
    }

    public IEnumerator MissileWarning(float height)
    {
        missileWarning.rectTransform.anchoredPosition = new Vector2(0, height * 5);
        for (int i = 0; i < 5; i++) 
        {
            missileWarning.enabled = true;
            yield return new WaitForSeconds(0.2f);
            missileWarning.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }


    public void PopupText(string text)
    {
        if (currentPopup != null) StopCoroutine(currentPopup);
        currentPopup = StartCoroutine(PopupTextC(text));
    }
    private IEnumerator PopupTextC(string text)
    {
        popupText.text = text;
        popupText.enabled = true;
        yield return new WaitForSeconds(0.4f);
        popupText.enabled = false;
        yield return new WaitForSeconds(0.4f);
        popupText.enabled = true;
        yield return new WaitForSeconds(0.4f);
        popupText.enabled = false;
    }

}
