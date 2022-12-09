using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileWall : MonoBehaviour
{
    Tower onTower;      // Ÿ�� ���� �����ϴ� Ÿ���� �����Ѵ�.
    bool isActive;      // Ÿ�� ��ġ �� �����ð��� ���� �Ŀ� Ÿ�Ͽ� ���õ� ����� ����ϰ� �ϰ� �ʹ�.
    
    // Ÿ���� �����ϴ°�?
    public bool isTower => (onTower != null);

    // Ÿ�Ͽ� Ÿ���� ��ġ�ϰڴ�.
    public void SetTower(Tower tower)
    {
        // �̹� Ÿ���� ������ ���� ��� �����Ѵ�.
        if (isTower)
            return;

        // Ÿ���� ���� ��ġ�� �ű�� ������Ų��.
        onTower = tower;
        tower.SwitchAttackRange(false);
        tower.transform.position = transform.position;

        StartCoroutine(ActiveTile());
    }
    private IEnumerator ActiveTile()
    {
        yield return new WaitForSeconds(0.5f);
        isActive = true;
    }

    public void OpenControlUI()
    {
        if (isTower)
        {
            TowerControlUI.Instance.RegestedEvent(OnSellTower, OnUpgradeTower);
            TowerControlUI.Instance.Open(this, onTower);
        }
    }
    private void OnSellTower()
    {
        // Ÿ�� ������ ���ݸ�ŭ GM�� ��带 �߰���Ű�� ��ġ�Ǿ��ִ� Ÿ���� �����Ѵ�.
        int sellPrice = Mathf.RoundToInt(onTower.Price * 0.5f);
        GameManager.Instance.AddGold(sellPrice);
        Destroy(onTower.gameObject);
        onTower = null;
        isActive = false;
    }
    private void OnUpgradeTower()
    {

    }
    
    public void OnMouseOver()
    {
        if (isTower && isActive)
            onTower.SwitchAttackRange(true);
    }
    public void OnMouseExit()
    {
        if (isTower && isActive)
        {
            onTower.SwitchAttackRange(false);
        }
    }  
}
