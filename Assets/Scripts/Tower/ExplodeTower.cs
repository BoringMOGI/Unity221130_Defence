using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTower : Tower
{
    [Header("Explode")]
    [SerializeField] float explodeRange;        // ���� ����.
    [SerializeField] ParticleSystem explodeFx;  // ���� ����Ʈ.

    // 1.�Ϲ� Ÿ���� ���� ���� �ֱⰡ �ְ� �ð��� �Ǿ��� �� ����� �����Ѵ�.
    // 2.�̶� ���� ������ �ִ� �����Ե� �������� �ش�.
    protected override void Attack()
    {
        // ���� ����� �������� explodeRange �������� ������ ���� ���̸� ���� ��� �ݶ��̴� �˻�.
        Collider2D[] targets = Physics2D.OverlapCircleAll(target.transform.position, explodeRange, attackMask);
        foreach(Collider2D target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();     // �˻��� �ݶ��̴����Լ� Enemy ������Ʈ �˻�.
            if (enemy == null)                              // Enemy ������Ʈ�� ������.
                continue;                                   // �ش� ���� ����.

            enemy.OnDamaged(attackPower);
        }

        Instantiate(explodeFx, target.transform.position, Quaternion.identity);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();    // �θ��� �Լ��� ȣ��.

        // ���� ������ ����Ѵ�.
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.transform.position, explodeRange);
        }
    }
}
