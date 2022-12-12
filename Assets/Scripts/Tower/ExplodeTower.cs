using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTower : Tower
{
    private float explodeRange;        // 폭발 범위.

    public override void Setup(TowerInfo info)
    {
        base.Setup(info);
        explodeRange = info.explodeRange;
    }

    // 1.일반 타워와 같이 공격 주기가 있고 시간이 되었을 때 대상을 공격한다.
    // 2.이때 일정 범위에 있는 적에게도 데미지를 준다.
    protected override void Attack()
    {
        // 공격 대상을 기준으로 explodeRange 반지름을 가지는 원형 레이를 통해 모든 콜라이더 검색.
        int layer = 1 << LayerMask.NameToLayer("Enemy");
        Collider2D[] targets = Physics2D.OverlapCircleAll(target.transform.position, explodeRange, layer);
        foreach(Collider2D target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();     // 검색된 콜라이더에게서 Enemy 컴포넌트 검색.
            if (enemy == null)                              // Enemy 컴포넌트가 없으면.
                continue;                                   // 해당 루프 생략.

            enemy.OnDamaged(attackPower);
        }

        Instantiate(fx, target.transform.position, Quaternion.identity);
    }
}
