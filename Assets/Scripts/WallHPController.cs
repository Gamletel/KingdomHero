using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHPController : MonoBehaviour
{
    public delegate void HPChanged(float hpRatio);
    public static event HPChanged hpChanged;

    public static void OnHPChanged(float maxHP, float curHP)
    {
        hpChanged?.Invoke(curHP/maxHP);
    }
}
