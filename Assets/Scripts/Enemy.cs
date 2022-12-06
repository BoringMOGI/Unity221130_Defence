using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int hp;
    [SerializeField] int gold;
    [SerializeField] Vector3 hpBarOffset;

    Queue<Transform> point; // ���� ������ ��ġ ���� ť.
    float rotate;
    HpBar hpBar;
    int maxHp;

    public void Setup(Transform[] points)
    {
        // �迭�� ť�� �ݷ����� �����̱� ������ ���� ������ �ʱ�ȭ�� �����ϴ�.
        point = new Queue<Transform>(points);
        hpBar = GameUI.Instance.GetHpBar();         // GamUI���Լ� ü�¹� �ϳ��� �޾ƿ´�.
        maxHp = hp;                                 // �� �ִ� ü���� ���� ü������ �����Ѵ�.

        // 1������ ���� ������ �޾ƿ��� ���� ��ġ�� �������ش�.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    void Update()
    {
        // point�� ������ 0�̶�� ���� �� �̻� �� �������� ���ٴ� ���̴�.
        // �� �� �����ߴ�.
        if(point.Count <= 0)
        {
            OnGoal();
            return;
        }

        // MoveTowords(Vector3, Vector3, float) : Vector3
        // => A�������� B�������� float�ӵ��� �������� ���� ��ġ�� �����Ѵ�.
        Vector3 destination = point.Peek().position;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // ���� �� ��ġ�� �������� �Ÿ��� 0.0f���� �۴ٴ� ���� ������ ��ġ�� �ִٴ� ���̴�.
        // ���� Queue�� Dequeue�� ȣ���� ���� �������� ����Ű�� ����.
        if (Vector3.Distance(transform.position, destination) <= 0.0f)
            point.Dequeue();

        // ���Ϸ� ������ �����ؼ� �����(Quaternion)������ �ٲ�޶�� �Լ���.
        // 1�� �� 90���� ���ƶ�.
        transform.rotation = Quaternion.Euler(0, 0, rotate += -360 * Time.deltaTime);

        // ü�¹��� ��ġ�� �����Ѵ�.
        // ������ �� ��ġ�� �� �߽��̱� ������ �������� �������ش�.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    public void OnDamaged(int power)
    {
        hp = Mathf.Clamp(hp - power, 0, 999);
        hpBar.SetHp(hp, maxHp);
        if(hp <= 0)
        {
            GameManager.Instance.AddGold(gold);     // ���� ������ �޾� �׾��� �� ��� ȹ��.
            OnDeadEnemy();                          // �� ����.
        }
    }
    private void OnGoal()
    {
        GameManager.Instance.OnGoalEnemy();         // GM���� ���� �����ߴٰ� �˸�.
        OnDeadEnemy();                              // �� ����.
    }
    private void OnDeadEnemy()
    {
        EnemySpanwer.Instance.OnDestroyEnemy();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

    // OnDrawGizmos���� ������
    // => ������Ʈ�� �������� ���� �׸���.
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + hpBarOffset, 0.05f);
    }
}
