using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tower : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] LineRenderer lineRenderer;       // ���� ������.

    // Ÿ���� �⺻ ����.
    protected int price;             // ���� ����.
    protected float attackRange;     // ���� ����.
    protected float attackRate;      // ���� �ӵ�.
    protected int attackPower;       // ���ݷ�.
    protected ParticleSystem fx;     // Ÿ���� ����Ʈ.

    // ���� ��� ����.
    private float delayTime;                // ���� ��� �ð�.
    private bool isActive;                  // Ȱ��ȭ�� �Ǿ��°�?
    protected Enemy target;                 // ���� ���.
    protected int targetLayer;              // ���� ��� ���̾�.

    public int Price => price;              // ������ �б� ���� ������Ƽ�� �ܺ� ����.

    private void Start()
    {
        delayTime = attackRate;

        // LayerMask.NameToLayer(string)�� �ִ� ���� �� ��° ���̾�(index)������ ������ ��ȯ�Ѵ�.
        // ������ ������ Raycast���� �޴� Mask���� ��Ʈ ���Ű��̱� ������ index�� �ش� ������ �ٲ� �ʿ䰡 �ִ�.
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");

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
        // ������ ����� ���ٸ�.
        if (target == null)
        {
            // ���� ���̸� ����� ���� ���� �ִ� ��� ���� Ž���Ѵ�.
            // ���� ���� ����� �Ÿ��� ���� ������� ��´�.
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);
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
}
