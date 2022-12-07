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
        // �� �ֺ� ���� ���� ������ ������� �Ǵ�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
        foreach(Collider2D collider in colliders)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                // ���� Ÿ���� ������ �ִ� ������� ���纻�� �ѱ��.
                // ���纻�� �ѱ��� �ʰ� ������ �ѱ�� ���� �ּҸ� �����ϰ� �ֱ� ������
                // enemy ���ο��� ����� Ŭ������ ���� �����ϸ� ���� Ÿ���� ����� ���� ����ȴ�.
                enemy.AddDebuff(debuff.GetCopy());
            }
        }

        Instantiate(fx, transform.position, Quaternion.identity);
    }
}
