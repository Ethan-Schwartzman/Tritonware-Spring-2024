using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance;
    [SerializeField] TMP_Text health, maxHealth, speed,
        playerProgress, enemyProgress, missileWarning,
        powerup;
    [SerializeField] ProgressBar powerupDuration;
    


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        speed.text = Mathf.RoundToInt(PlayerShip.Instance.GetVelocity().magnitude).ToString();
        playerProgress.text = ThreatController.Instance.GetPlayerProgress().ToString();
        enemyProgress.text = ThreatController.Instance.GetEnemyProgress().ToString();

        Powerup pow = PlayerShip.Instance.currentPowerup;
        if (pow != null)
        {
            powerupDuration.SetLevel((pow.GetDuration() - pow.activatedDuration) / pow.GetDuration());
        }
        
    }

    public void UpdateUI()
    {
        health.text = PlayerShip.Instance.GetHealth().ToString();
        maxHealth.text = PlayerShip.Instance.GetMaxHealth().ToString();
        if (PlayerShip.Instance.currentPowerup == null) powerup.text = "";
        else powerup.text = PlayerShip.Instance.currentPowerup.GetName();
    }

    public IEnumerator MissileWarning()
    {
        missileWarning.enabled = true;
        yield return new WaitForSeconds(0.2f);
        missileWarning.enabled = false;
        yield return new WaitForSeconds(0.2f);
        missileWarning.enabled = true;
        yield return new WaitForSeconds(0.2f);
        missileWarning.enabled = false;
        yield return new WaitForSeconds(0.2f);
        missileWarning.enabled = true;
        yield return new WaitForSeconds(0.2f);
        missileWarning.enabled = false;
    }

}
