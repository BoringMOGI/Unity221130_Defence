using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerControlUI : Singleton<TowerControlUI>
{
    [SerializeField] GameObject panel;      // ���� UI�� �г�.
    [SerializeField] Text sellText;         // �Ǹ� ���� �ؽ�Ʈ.
    [SerializeField] Text upgradeText;      // ���׷��̵� ���� �ؽ�Ʈ.

    TileWall tile = null;
    Action onSellTower;         // Ÿ�� �Ǹ� ��������Ʈ.
    Action onUpgradeTower;      // Ÿ�� ���׷��̵� ��������Ʈ.

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
        // ������ Ÿ�Ͽ� ���� UI�� Open ��û�Ǿ��� ��� ����.
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
