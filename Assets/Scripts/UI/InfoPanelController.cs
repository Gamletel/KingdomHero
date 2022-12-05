using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    [SerializeField] private Text _enemiesLeftText;
    [SerializeField] private Text _curWaveText;

    private void Start()
    {
        KilledEnemiesCounter.enemyKilled += UpdateEnemiesLeftText;
        EnemySpawner.newWave += UpdateWaveText;
    }

    private void OnDestroy()
    {
        KilledEnemiesCounter.enemyKilled -= UpdateEnemiesLeftText;
        EnemySpawner.newWave -= UpdateWaveText;
    }

    private void UpdateEnemiesLeftText()
    {
        _enemiesLeftText.text = KilledEnemiesCounter.enemiesLeft.ToString();
    }

    private void UpdateWaveText(int wave)
    {
        _curWaveText.text = wave.ToString();
    }
}
