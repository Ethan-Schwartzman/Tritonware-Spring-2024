using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance;
    [SerializeField] TMP_Text health, maxHealth, speed,
        playerProgress, enemyProgress;

    


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        speed.text = Mathf.RoundToInt(PlayerShip.Instance.GetVelocity().magnitude).ToString();
        playerProgress.text = ThreatController.Instance.GetPlayerProgress().ToString();
        enemyProgress.text = ThreatController.Instance.GetEnemyProgress().ToString();
    }

    public void UpdateUI()
    {
        health.text = PlayerShip.Instance.GetHealth().ToString();
        maxHealth.text = PlayerShip.Instance.GetMaxHealth().ToString();
    }
}
