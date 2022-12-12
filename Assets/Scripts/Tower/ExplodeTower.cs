using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTower : Tower
{
    private float explodeRange;        // ���� ����.

    public override void Setup(TowerInfo info)
    {
        base.Setup(info);
        explodeRange = info.explodeRange;
    }

    // 1.�Ϲ� Ÿ���� ���� ���� �ֱⰡ �ְ� �ð��� �Ǿ��� �� ����� �����Ѵ�.
    // 2.�̶� ���� ������ �ִ� �����Ե� �������� �ش�.
    protected override void Attack()
    {
        // ���� ����� �������� explodeRange �������� ������ ���� ���̸� ���� ��� �ݶ��̴� �˻�.
        int layer = 1 << LayerMask.NameToLayer("Enemy");
        Collider2D[] targets = Physics2D.OverlapCircleAll(target.transform.position, explodeRange, layer);
        foreach(Collider2D target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();     // �˻��� �ݶ��̴����Լ� Enemy ������Ʈ �˻�.
            if (enemy == null)                              // Enemy ������Ʈ�� ������.
                continue;                                   // �ش� ���� ����.

            enemy.OnDamaged(attackPower);
        }

        Instantiate(fx, target.transform.position, Quaternion.identity);
    }
}
