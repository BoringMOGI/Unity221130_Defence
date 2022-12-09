using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tower : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] protected int price;             // ���� ����.
    [SerializeField] LineRenderer lineRenderer;       // ���� ������.

    [Header("Attack")]
    [SerializeField] protected float attackRange;     // ���� ����.
    [SerializeField] protected float attackRate;      // ���� �ӵ�.
    [SerializeField] protected int attackPower;       // ���ݷ�.
    [SerializeField] protected LayerMask attackMask;  // ���� ��� ����ũ.

    private float delayTime;                // ���� ��� �ð�.
    private bool isActive;                  // Ȱ��ȭ�� �Ǿ��°�?
    protected Enemy target;                 // ���� ���.

    public int Price => price;              // ������ �б� ���� ������Ƽ�� �ܺ� ����.

    private void Start()
    {
        delayTime = attackRate;

        // ���� ������ �׸���.
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

    private void SearchToTarget()
    {
        // ������ ����� ���ٸ�.
        if (target == null)
        {
            // ���� ���̸� ����� ���� ���� �ִ� ��� ���� Ž���Ѵ�.
            // ���� ���� ����� �Ÿ��� ���� ������� ��´�.
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, attackMask);
            if (targets.Length > 0)
            {
                Collider2D find = targets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).First();
                target = find.GetComponent<Enemy>();
            }
        }
        else
        {
            // ���� ����� �����Ѵٸ� ������ �Ÿ��� ������ ���ݹ����� ������� üũ�Ѵ�.
            if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
                target = null;
        }
    }
    private void LookAtTarget()
    {
        // MoveToward(A, B, T) : A�� B������ T��ŭ ���� �� (���)
        // Slerp(A, B, F) : A�� B������ T�� ������ ���� �� (�)

        Quaternion lookRotation = Quaternion.identity;
        if(target != null)
        {
            // FromToRotation(�� ����, ���� �ٶ� �� ����)
            // = �� ������ ���� �ٶ� �� �������� ���ϴ� ȸ�� ���� ��ȯ.
            Vector3 dir = (target.transform.position - transform.position);
            lookRotation = Quaternion.FromToRotation(Vector2.right, dir);

            // ���� �� ȸ�� ������ ��ǥ ������� �ε巴��(Smooth) ������.
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 15 * GameManager.DeltaTime);
        }
        /*else
        {
            // Euler(Vector3) : Vector3(���Ϸ� ����)�� �����(Quaternion)�� �ٲ��ִ� �Լ�.
            lookRotation = Quaternion.Euler(Vector2.right);
        }*/
                
    }
    private void AttackToTarget()
    {
        // ����.
        delayTime = Mathf.Clamp(delayTime - GameManager.DeltaTime, 0.0f, attackRate);   // ��� �ð� ����.
        if (delayTime <= 0.0f && target != null)                                        // ��� �ð��� �����ٸ�..
        {
            delayTime = attackRate;                                                     // ���� ��� �ð� ����.
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
        // ���� ������ŭ ���� ������ �����Ѵ�.
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

    // abstract �Լ��� ���� �� �� �ִ�.
    // Ŭ������ ��ӹ޴� �ڽ� Ŭ������ ������ ����ؾ��Ѵ�.
    protected abstract void Attack();


    protected virtual void OnDrawGizmosSelected()
    {
        // ���� ���� �����.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ���� ��� �����.
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
