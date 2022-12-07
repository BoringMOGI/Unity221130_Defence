using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWall : MonoBehaviour
{
    Tower onTower;      // 타일 위에 존재하는 타워를 참조한다.

    public bool isTower => (onTower != null);     // 타워가 존재하는가?
    
    public void SetTower(Tower tower)
    {
        if (isTower)
            return;

        // 타워를 나의 위치로 옮기고 참조시킨다.
        onTower = tower;
        tower.transform.position = transform.position;
    }
}
