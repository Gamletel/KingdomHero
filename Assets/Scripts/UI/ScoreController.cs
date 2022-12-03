using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _scoreMultiplierText;
    [SerializeField] private Image _multiplierCountdownImg;
    private float _score;
    private int _scoreMultiplier = 1;
    private const float _timeToResetMultiplier = 5f;
    private const float _countdown = .1f;
    private float _curTime;

    private void Awake()
    {
        _scoreMultiplierText.text = _scoreMultiplier.ToString();
        _scoreText.text = _score.ToString();
    }

    private void Start()
    {
        KilledEnemiesCounter.enemyKilled += AddScore;
    }

    private void OnDestroy()
    {
        KilledEnemiesCounter.enemyKilled -= AddScore;
    }

    private void AddScore()
    {
        CalculateScore();
        StartCoroutine(MultiplierTimer());
    }

    private IEnumerator MultiplierTimer()
    {
        _curTime = _timeToResetMultiplier;
        while (_curTime >= 0)
        {
            yield return new WaitForSeconds(_countdown * _scoreMultiplier);
            _curTime -= _countdown;
            _multiplierCountdownImg.fillAmount = _curTime / _timeToResetMultiplier;
        }
        _scoreMultiplier = 1;
        _scoreMultiplierText.text = _scoreMultiplier.ToString();
    }

    private void CalculateScore()
    {
        if (_curTime >= 0)
            _scoreMultiplier++;
        else
            _scoreMultiplier = 1;

        _score += 1 * _scoreMultiplier;

        _scoreMultiplierText.text = _scoreMultiplier.ToString();
        _scoreText.text = _score.ToString();
    }
}
