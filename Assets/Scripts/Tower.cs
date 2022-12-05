using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] int price;             // 구매 가격.

    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;   // 총알 프리팹.
    [SerializeField] float bulletSpeed;     // 총알 속도.

    [Header("Attack")]
    [SerializeField] float attackRange;     // 공격 범위.
    [SerializeField] float attackRate;      // 공격 속도.
    [SerializeField] int attackPower;       // 공격력.
    [SerializeField] LayerMask attackMask;  // 공격 대상 마스크.

    private float delayTime;                // 공격 대기 시간.
    private Enemy target;                   // 공격 대상.

    public int Price => price;              // 가격을 읽기 전용 프로퍼티로 외부 공개.

    private void Start()
    {
        delayTime = attackRate;
    }
    private void Update()
    {
        SearchToTarget();
        LookAtTarget();
        AttackToTarget();
    }

    private void SearchToTarget()
    {
        // 공격할 대상이 없다면.
        if (target == null)
        {
            // 원형 레이를 사용해 범위 내에 있는 모든 적을 탐색한다.
            // 이후 가장 가까운 거리의 적을 대상으로 잡는다.
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
            if (targets.Length > 0)
            {
                Collider2D find = targets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).First();
                target = find.GetComponent<Enemy>();
            }
        }
        else
        {
            // 공격 대상이 존재한다면 나와의 거리를 측정해 공격범위를 벗어났는지 체크한다.
            if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
                target = null;
        }
    }
    private void LookAtTarget()
    {
        // MoveToward(A, B, T) : A가 B값으로 T만큼 변한 값 (등속)
        // Slerp(A, B, F) : A가 B값으로 T의 비율로 변한 값 (곡선)

        Quaternion lookRotation;
        if(target != null)
        {
            // FromToRotation(축 방향, 실제 바라 볼 방향)
            // = 축 방향을 실제 바라 볼 방향으로 향하는 회전 값을 반환.
            Vector3 dir = (target.transform.position - transform.position);
            lookRotation = Quaternion.FromToRotation(Vector2.right, dir);            
        }
        else
        {
            // Euler(Vector3) : Vector3(오일러 각도)를 사원수(Quaternion)로 바꿔주는 함수.
            lookRotation = Quaternion.Euler(Vector2.right);
        }

        // 현재 내 회전 값에서 목표 방향까지 부드럽게(Smooth) 돌린다.
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 15 * Time.deltaTime);
    }
    private void AttackToTarget()
    {
        // 공격.
        delayTime = Mathf.Clamp(delayTime - Time.deltaTime, 0.0f, attackRate);  // 대기 시간 감소.
        if (delayTime <= 0.0f && target != null)                                // 대기 시간이 끝났다면..
        {
            delayTime = attackRate;                                             // 공격 대기 시간 갱신.

            // 총알 프리팹을 클론으로 생성해 정보 대입.
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Setup(target, bulletSpeed, attackPower);
        }
    }


    private void OnDrawGizmos()
    {
        // 공격 범위 기즈모.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 공격 대상 기즈모.
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
