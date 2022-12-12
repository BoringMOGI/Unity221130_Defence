using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tower : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] LineRenderer lineRenderer;       // 라인 렌더러.

    // 타워의 기본 정보.
    protected int price;             // 구매 가격.
    protected float attackRange;     // 공격 범위.
    protected float attackRate;      // 공격 속도.
    protected int attackPower;       // 공격력.
    protected ParticleSystem fx;     // 타워의 이펙트.

    // 내부 멤버 변수.
    private float delayTime;                // 공격 대기 시간.
    private bool isActive;                  // 활성화가 되었는가?
    protected Enemy target;                 // 공격 대상.
    protected int targetLayer;              // 공격 대상 레이어.

    public int Price => price;              // 가격을 읽기 전용 프로퍼티로 외부 공개.

    private void Start()
    {
        delayTime = attackRate;

        // LayerMask.NameToLayer(string)가 주는 값은 몇 번째 레이어(index)인지를 값으로 반환한다.
        // 하지만 실제로 Raycast에서 받는 Mask값은 비트 열거값이기 때문에 index를 해당 값으로 바꿀 필요가 있다.
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");

        // 공격 범위를 그린다.
        CalculateLine();
        SwitchAttackRange(true);
    }
    private void Update()
    {
        if (isActive)
        {
            SearchToTarget();
            LookAtTarget();
            AttackToTarget();
        }
    }

    public virtual void Setup(TowerInfo info)
    {
        price = info.price;
        attackRange = info.attackRange;
        attackRate = info.attackRate;
        attackPower = info.attackPower;
        fx = info.fx;
    }

    private void SearchToTarget()
    {
        // 공격할 대상이 없다면.
        if (target == null)
        {
            // 원형 레이를 사용해 범위 내에 있는 모든 적을 탐색한다.
            // 이후 가장 가까운 거리의 적을 대상으로 잡는다.
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);
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

        Quaternion lookRotation = Quaternion.identity;
        if(target != null)
        {
            // FromToRotation(축 방향, 실제 바라 볼 방향)
            // = 축 방향을 실제 바라 볼 방향으로 향하는 회전 값을 반환.
            Vector3 dir = (target.transform.position - transform.position);
            lookRotation = Quaternion.FromToRotation(Vector2.right, dir);

            // 현재 내 회전 값에서 목표 방향까지 부드럽게(Smooth) 돌린다.
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 15 * GameManager.DeltaTime);
        }
        /*else
        {
            // Euler(Vector3) : Vector3(오일러 각도)를 사원수(Quaternion)로 바꿔주는 함수.
            lookRotation = Quaternion.Euler(Vector2.right);
        }*/
                
    }
    private void AttackToTarget()
    {
        // 공격.
        delayTime = Mathf.Clamp(delayTime - GameManager.DeltaTime, 0.0f, attackRate);   // 대기 시간 감소.
        if (delayTime <= 0.0f && target != null)                                        // 대기 시간이 끝났다면..
        {
            delayTime = attackRate;                                                     // 공격 대기 시간 갱신.
            Attack();
        }
    }

    public void Activate()
    {
        isActive = true;
        SwitchAttackRange(false);
    }

    private void CalculateLine()
    {
        // 공격 범위만큼 원의 정점을 세팅한다.
        var segments = 360;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * attackRange, Mathf.Cos(rad) * attackRange, -1f);
        }

        lineRenderer.SetPositions(points);
    }
    public void SwitchAttackRange(bool isOn)
    {
        lineRenderer.enabled = isOn;
    }

    // abstract 함수는 선언만 할 수 있다.
    // 클래스를 상속받는 자식 클래스는 무조건 상속해야한다.
    protected abstract void Attack();
}
