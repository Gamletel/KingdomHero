using UnityEngine;


public class Wall : MonoBehaviour, IDamageable
{
    public static Transform wallTransform { get; private set; }
    [SerializeField] private float _maxHP;
    private float _curHP;

    private void Awake()
    {
        _curHP = _maxHP;
        wallTransform = transform;
        WallHPController.OnHPChanged(_maxHP, _curHP);
    }

    private void OnEnable()
    {
        LoseController.playerLose += WallIsCrushing;
    }

    private void OnDisable()
    {
        LoseController.playerLose -= WallIsCrushing;
    }

    private void WallIsCrushing()
    {
        GetComponent<Animator>().SetTrigger("PlayerLose");
    }

    public void GetDamage(int damage)
    {
        _curHP -= damage;
        Debug.Log($"Получен урон! ХП: {_curHP}");
        if (_curHP <= 0)
            LoseController.OnPlayerLose();
        WallHPController.OnHPChanged(_maxHP, _curHP);
    }
}