using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] int price;             // ���� ����.

    [Header("Attack")]
    [SerializeField] float attackRange;     // ���� ����.
    [SerializeField] float attackRate;      // ���� �ӵ�.
    [SerializeField] int attackPower;       // ���ݷ�.
    [SerializeField] LayerMask attackMask;  // ���� ��� ����ũ.

    private float delayTime;                // ���� ��� �ð�.
    protected Enemy target;                 // ���� ���.
    protected LayerMask EnemyMask => attackMask;

    public int Price => price;              // ������ �б� ���� ������Ƽ�� �ܺ� ����.

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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 15 * Time.deltaTime);
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
        delayTime = Mathf.Clamp(delayTime - Time.deltaTime, 0.0f, attackRate);  // ��� �ð� ����.
        if (delayTime <= 0.0f && target != null)                                // ��� �ð��� �����ٸ�..
        {
            delayTime = attackRate;                                             // ���� ��� �ð� ����.
            AttackToTarget(target, attackPower);
        }
    }

    // abstract �Լ��� ���� �� �� �ִ�.
    // Ŭ������ ��ӹ޴� �ڽ� Ŭ������ ������ ����ؾ��Ѵ�.
    protected abstract void AttackToTarget(Enemy target, int attackPower);


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
