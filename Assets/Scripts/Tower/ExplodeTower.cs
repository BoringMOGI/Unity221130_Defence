using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTower : Tower
{
    [Header("Explode")]
    [SerializeField] float explodeRange;        // 폭발 범위.
    [SerializeField] ParticleSystem explodeFx;  // 폭발 이펙트.

    // 1.일반 타워와 같이 공격 주기가 있고 시간이 되었을 때 대상을 공격한다.
    // 2.이때 일정 범위에 있는 적에게도 데미지를 준다.
    protected override void Attack()
    {
        // 공격 대상을 기준으로 explodeRange 반지름을 가지는 원형 레이를 통해 모든 콜라이더 검색.
        Collider2D[] targets = Physics2D.OverlapCircleAll(target.transform.position, explodeRange, attackMask);
        foreach(Collider2D target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();     // 검색된 콜라이더에게서 Enemy 컴포넌트 검색.
            if (enemy == null)                              // Enemy 컴포넌트가 없으면.
                continue;                                   // 해당 루프 생략.

            enemy.OnDamaged(attackPower);
        }

        Instantiate(explodeFx, target.transform.position, Quaternion.identity);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();    // 부모의 함수를 호출.

        // 폭발 범위를 출력한다.
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.transform.position, explodeRange);
        }
    }
}
