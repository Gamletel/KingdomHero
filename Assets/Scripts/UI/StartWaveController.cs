using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveController : MonoBehaviour
{
    public delegate void WaveStarted();
    public static event WaveStarted waveStarted;

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gamePanel;

    private void Start()
    {
        _gamePanel.SetActive(false);
    }

    public void StartWave()
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(true);
        waveStarted?.Invoke();
    }
}
