using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // �̱���.
    // = ��𼭵� ������ ������ ������ ����.
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
