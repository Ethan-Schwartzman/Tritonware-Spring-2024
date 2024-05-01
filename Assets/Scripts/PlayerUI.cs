using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance;
    [SerializeField] TMP_Text
        missileWarning,
        popupText;
    [SerializeField] ProgressBar powerupDuration, healthBar;
    [SerializeField] Image segment, powerupIcon, powerupActivateIndicator;
    public Slider PlayerProgress;
    public Slider EnemyProgress;

    Coroutine currentPopup;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        PlayerProgress.value = (Mathf.Clamp01(ThreatController.Instance.GetPlayerProgress() / Settings.Instance.sectorDistance));
        EnemyProgress.value = (Mathf.Clamp01(ThreatController.Instance.GetEnemyProgress() / Settings.Instance.sectorDistance));

        Powerup pow = PlayerShip.Instance.currentPowerup;
        if (pow != null)
        {
            float percent = ((pow.GetDuration() - pow.activatedDuration) / pow.GetDuration()) / (float)pow.maxCharges
                + (float)(pow.charges - 1) / (float)pow.maxCharges;
            powerupDuration.SetLevel(percent);
            if (pow.isActive) powerupActivateIndicator.color = Color.yellow;
            else powerupActivateIndicator.color = Color.black;
        }
        else
        {
            powerupDuration.SetLevel(0);
            powerupActivateIndicator.color = Color.black;
        }
        
    }

    public void UpdateUI()
    {
        healthBar.SetLevel((float)PlayerShip.Instance.GetHealth() / (float)PlayerShip.Instance.GetMaxHealth());
        if (PlayerShip.Instance.currentPowerup == null) powerupIcon.enabled = false;
        else
        {
            powerupIcon.enabled = true;
            powerupIcon.sprite = PlayerShip.Instance.currentPowerup.iconSprite;
        }
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
