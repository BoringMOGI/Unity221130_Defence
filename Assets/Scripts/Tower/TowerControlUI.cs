using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerControlUI : Singleton<TowerControlUI>
{
    [SerializeField] GameObject panel;      // 실제 UI의 패널.
    [SerializeField] Text sellText;         // 판매 가격 텍스트.
    [SerializeField] Text upgradeText;      // 업그레이드 가격 텍스트.

    TileWall tile = null;
    Action onSellTower;         // 타워 판매 델리게이트.
    Action onUpgradeTower;      // 타워 업그레이드 델리게이트.

    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void RegestedEvent(Action onSellTower, Action onUpgradeTower)
    {
        this.onSellTower = onSellTower;
        this.onUpgradeTower = onUpgradeTower;
    }
    public void Open(TileWall tile, Tower onTower)
    {
        // 동일한 타일에 대한 UI가 Open 요청되었을 경우 무시.
        if (this.tile == tile)
            return;

        Vector3 position = Camera.main.WorldToScreenPoint(tile.transform.position);
        transform.position = position;

        sellText.text = Mathf.RoundToInt(onTower.Price * 0.5f).ToString("#,##0");
        upgradeText.text = "9,999";

        panel.SetActive(true);
    }

    public void OnSellTower()
    {
        onSellTower?.Invoke();
        Close();
    }
    public void OnUpgradeTower()
    {
        onUpgradeTower?.Invoke();
        Close();
    }

    public void Close()
    {
        tile = null;
        panel.SetActive(false);
    }
    
}
