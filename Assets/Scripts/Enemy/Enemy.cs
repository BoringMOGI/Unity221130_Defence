using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;     // ���� ������.
    [SerializeField] float moveSpeed;                   // �⺻ �̵��ӵ�.
    [SerializeField] int hp;                            // �⺻ ü��.
    [SerializeField] int gold;                          // �⺻ ����.
    [SerializeField] Vector3 hpBarOffset;               // ü�¹� ��ġ Offset.

    List<Debuff> debuffList = new List<Debuff>();       // ������ �ִ� ������� ����Ʈ.

    Queue<Transform> point;     // ���� ������ ��ġ ���� ť.
    HpBar hpBar;
    float rotate;
    int maxHp;

    float currentMoveSpeed;     // ���� �̵� �ӵ�.

    public void Setup(EnemyInfo info, Transform[] points)
    {
        // ���� ���� �⺻ ���� ����.
        spriteRenderer.sprite = info.sprite;
        hp = maxHp = info.hp;
        moveSpeed = info.speed;
        gold = info.gold;

        // �迭�� ť�� �ݷ����� �����̱� ������ ���� ������ �ʱ�ȭ�� �����ϴ�.
        point = new Queue<Transform>(points);
        hpBar = GameUI.Instance.GetHpBar();         // GamUI���Լ� ü�¹� �ϳ��� �޾ƿ´�.

        // 1������ ���� ������ �޾ƿ��� ���� ��ġ�� �������ش�.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    void Update()
    {
        UpdateDebuff();     // ����� ������Ʈ.
        Movement();         // ĳ���� �̵�.
    }

    // ����� ����.
    public void AddDebuff(Debuff debuff)
    {
        // ������ type, priory�� ���� ������� �ִٸ� �ð��� ����.
        var find = debuffList.Where(v => (v.type == debuff.type) && (v.priority == debuff.priority));
        if(find.Count() > 0)
        {
            find.First().continueTime = debuff.continueTime;
        }
        else
        {
            debuffList.Add(debuff);
        }
    }
    private void UpdateDebuff()
    {
        currentMoveSpeed = moveSpeed;
        spriteRenderer.color = Color.white;

        // ���� ȿ�� ����.
        for(DEBUFF type = 0; type < DEBUFF.Count; type++)
        {
            // ����Ʈ���� ������ Ÿ���� ���� ����� �׷��� �����.
            // ������ 0����� ���� ��ŵ.
            var typeGroup = debuffList.Where(v => v.type == type);
            if (typeGroup.Count() <= 0)
                continue;

            // type ����� �׷��� �������� ���� �� ù��° �� ����.
            Debuff debuff = typeGroup.OrderByDescending(v => v.priority).First();
            switch(type)
            {
                case DEBUFF.Slow:
                    currentMoveSpeed = moveSpeed * debuff.value;
                    spriteRenderer.color = Color.blue;
                    break;
            }
        }

        // Where(����)
        // ����Ʈ�� ��ȸ�ϸ鼭 �ش� ����� ���ӽð��� ���������� 0���� Ŭ ��츸 �����´�.
        debuffList = debuffList.Where(debuff => (debuff.continueTime -= GameManager.DeltaTime) > 0.0f).ToList();
    }

    // �̵� ����
    private void Movement()
    {
        // point�� ������ 0�̶�� ���� �� �̻� �� �������� ���ٴ� ���̴�.
        // �� �� �����ߴ�.
        if (point.Count <= 0)
        {
            OnGoal();
            return;
        }

        // MoveTowords(Vector3, Vector3, float) : Vector3
        // => A�������� B�������� float�ӵ��� �������� ���� ��ġ�� �����Ѵ�.
        Vector3 destination = point.Peek().position;
        transform.position = Vector3.MoveTowards(transform.position, destination, currentMoveSpeed * GameManager.DeltaTime);

        // ���� �� ��ġ�� �������� �Ÿ��� 0.0f���� �۴ٴ� ���� ������ ��ġ�� �ִٴ� ���̴�.
        // ���� Queue�� Dequeue�� ȣ���� ���� �������� ����Ű�� ����.
        if (Vector3.Distance(transform.position, destination) <= 0.0f)
            point.Dequeue();

        // ���Ϸ� ������ �����ؼ� �����(Quaternion)������ �ٲ�޶�� �Լ���.
        // 1�� �� 90���� ���ƶ�.
        transform.rotation = Quaternion.Euler(0, 0, rotate += -360 * GameManager.DeltaTime);

        // ü�¹��� ��ġ�� �����Ѵ�.
        // ������ �� ��ġ�� �� �߽��̱� ������ �������� �������ش�.
        hpBar.SetPosition(transform.position + hpBarOffset);
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

    // ���� ����.
    public void OnDamaged(int power)
    {
        hp = Mathf.Clamp(hp - power, 0, 999);
        hpBar.SetHp(hp, maxHp);
        if (hp <= 0)
        {
            GameManager.Instance.AddGold(gold);     // ���� ������ �޾� �׾��� �� ��� ȹ��.
            OnDeadEnemy();                          // �� ����.
        }
    }

    // OnDrawGizmos���� ������
    // => ������Ʈ�� �������� ���� �׸���.
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + hpBarOffset, 0.05f);
    }
}
