using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // 싱글톤.
    // = 어디서든 접근이 용이한 디자인 패턴.
    public static GameUI Instance { get; private set; }     

    [SerializeField] HpBar HpBarPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public HpBar GetHpBar()
    {
        return Instantiate(HpBarPrefab, transform);
    }
}
