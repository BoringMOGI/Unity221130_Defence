using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWall : MonoBehaviour
{
    Tower onTower;      // Ÿ�� ���� �����ϴ� Ÿ���� �����Ѵ�.

    public bool isTower => (onTower != null);     // Ÿ���� �����ϴ°�?
    
    public void SetTower(Tower tower)
    {
        if (isTower)
            return;

        // Ÿ���� ���� ��ġ�� �ű�� ������Ų��.
        onTower = tower;
        tower.transform.position = transform.position;
    }
    
    public void OnMouseOver()
    {
        if (isTower)
            onTower.SwitchAttackRange(true);
    }
    public void OnMouseExit()
    {
        if (isTower)
            onTower.SwitchAttackRange(false);
    }
    public void OnMouseUpAsButton()
    {
        if (isTower)
            TowerControlUI.Instance.Open(onTower, transform.position);
    }
}
