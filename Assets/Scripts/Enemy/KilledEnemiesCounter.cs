using UnityEngine;

public class KilledEnemiesCounter : MonoBehaviour
{
    public delegate void AllEnemiesKilled();
    public static event AllEnemiesKilled allEnemiesKilled;

    public delegate void EnemyKilled();
    public static event EnemyKilled enemyKilled;

    public static int enemiesAmount { get; private set; }
    public static int enemiesLeft { get; private set; }
    private static int _killedEnemies;

    public static void OnAllEnemiesKilled()
    {
        allEnemiesKilled?.Invoke();
        _killedEnemies = 0;
    }

    public static void SetEnemiesAmount(int amount)
    {
        enemiesAmount = amount;
        enemiesLeft = enemiesAmount;
    }

    public static void OnEnemyKilled()
    {
        _killedEnemies++;
        enemiesLeft--;
        enemyKilled?.Invoke();
        if (_killedEnemies == enemiesAmount)
            OnAllEnemiesKilled();
    }
}
