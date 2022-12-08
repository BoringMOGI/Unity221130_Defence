using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerControlUI : Singleton<TowerControlUI>
{
    [SerializeField] GameObject panel;      // 실제 UI의 패널.
    [SerializeField] Text sellText;         // 판매 가격 텍스트.
    [SerializeField] Text upgradeText;      // 업그레이드 가격 텍스트.

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Open(Tower tower, Vector3 worldPosition)
    {
        Vector3 position = Camera.main.WorldToScreenPoint(worldPosition);
        transform.position = position;

        sellText.text = Mathf.RoundToInt(tower.Price * 0.5f).ToString("#,##0");
        upgradeText.text = "9,999";

        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    
    
}
