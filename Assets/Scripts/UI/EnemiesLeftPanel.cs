using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesLeftPanel : MonoBehaviour
{
    private Text _enemiesLeftText;
    private void Awake()
    {
        _enemiesLeftText = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateEnemiesLeftText();
    }

    private void OnEnable()
    {
        KilledEnemiesCounter.enemyKilled += UpdateEnemiesLeftText;
    }

    private void OnDisable()
    {
        KilledEnemiesCounter.enemyKilled -= UpdateEnemiesLeftText;
    }

    private void UpdateEnemiesLeftText()
    {
        _enemiesLeftText.text = KilledEnemiesCounter.enemiesLeft.ToString();
    }
}
