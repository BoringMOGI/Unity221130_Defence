using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : Tower
{
    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;   // 총알 프리팹.
    [SerializeField] float bulletSpeed;     // 총알 속도.

    protected override void Attack()
    {
        // 총알 프리팹을 클론으로 생성해 정보 대입.
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.Setup(target, bulletSpeed, attackPower);
    }
}
