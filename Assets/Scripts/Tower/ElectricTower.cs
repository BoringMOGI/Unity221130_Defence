using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTower : Tower
{
    [Header("Debuff")]
    [SerializeField] Debuff debuff;
    [SerializeField] ParticleSystem fx;

    protected override void Attack()
    {
        // 내 주변 일정 범위 적에게 디버프를 건다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
        foreach(Collider2D collider in colliders)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                // 마비 타워가 가지고 있는 디버프의 복사본을 넘긴다.
                // 복사본을 넘기지 않고 원본을 넘기면 같은 주소를 참조하고 있기 때문에
                // enemy 내부에서 디버프 클래스의 값을 변경하면 마비 타워의 디버프 값도 변경된다.
                enemy.AddDebuff(debuff.GetCopy());
            }
        }

        Instantiate(fx, transform.position, Quaternion.identity);
    }
}
