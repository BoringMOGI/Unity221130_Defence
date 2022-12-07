using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : Tower
{
    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;   // �Ѿ� ������.
    [SerializeField] float bulletSpeed;     // �Ѿ� �ӵ�.

    protected override void Attack()
    {
        // �Ѿ� �������� Ŭ������ ������ ���� ����.
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.Setup(target, bulletSpeed, attackPower);
    }
}
