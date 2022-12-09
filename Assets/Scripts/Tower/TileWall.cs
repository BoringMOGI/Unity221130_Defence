using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileWall : MonoBehaviour
{
    Tower onTower;      // 타일 위에 존재하는 타워를 참조한다.
    bool isActive;      // 타워 설치 후 일정시간이 지난 후에 타일에 관련된 기능을 사용하게 하고 싶다.
    
    // 타워가 존재하는가?
    public bool isTower => (onTower != null);

    // 타일에 타워를 설치하겠다.
    public void SetTower(Tower tower)
    {
        // 이미 타워를 가지고 있을 경우 무시한다.
        if (isTower)
            return;

        // 타워를 나의 위치로 옮기고 참조시킨다.
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
        // 타워 가격의 절반만큼 GM의 골드를 추가시키고 설치되어있던 타워는 제거한다.
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
