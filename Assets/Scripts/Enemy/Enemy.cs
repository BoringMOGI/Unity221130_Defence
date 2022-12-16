using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;     // 적의 렌더러.
    [SerializeField] float moveSpeed;                   // 기본 이동속도.
    [SerializeField] int hp;                            // 기본 체력.
    [SerializeField] int gold;                          // 기본 가격.
    [SerializeField] Vector3 hpBarOffset;               // 체력바 위치 Offset.

    List<Debuff> debuffList = new List<Debuff>();       // 가지고 있는 디버프의 리스트.

    Queue<Transform> point;     // 적이 움직일 위치 지점 큐.
    HpBar hpBar;
    float rotate;
    int maxHp;

    float currentMoveSpeed;     // 현재 이동 속도.

    public void Setup(EnemyInfo info, Transform[] points)
    {
        // 적에 대한 기본 정보 세팅.
        spriteRenderer.sprite = info.sprite;
        hp = maxHp = info.hp;
        moveSpeed = info.speed;
        gold = info.gold;

        // 배열도 큐도 콜렉션의 일종이기 때문에 같은 원리로 초기화가 가능하다.
        point = new Queue<Transform>(points);
        hpBar = GameUI.Instance.GetHpBar();         // GamUI에게서 체력바 하나를 받아온다.

        // 1프레임 오차 때문에 받아오자 마자 위치를 갱신해준다.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    void Update()
    {
        UpdateDebuff();     // 디버프 업데이트.
        Movement();         // 캐릭터 이동.
    }

    // 디버프 관련.
    public void AddDebuff(Debuff debuff)
    {
        // 동일한 type, priory를 가진 디버프가 있다면 시간을 갱신.
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

        // 실제 효과 적용.
        for(DEBUFF type = 0; type < DEBUFF.Count; type++)
        {
            // 리스트에서 동일한 타입을 가진 디버프 그룹을 만든다.
            // 개수가 0개라면 루프 스킵.
            var typeGroup = debuffList.Where(v => v.type == type);
            if (typeGroup.Count() <= 0)
                continue;

            // type 디버프 그룹을 내림차순 정렬 후 첫번째 값 대입.
            Debuff debuff = typeGroup.OrderByDescending(v => v.priority).First();
            switch(type)
            {
                case DEBUFF.Slow:
                    currentMoveSpeed = moveSpeed * debuff.value;
                    spriteRenderer.color = Color.blue;
                    break;
            }
        }

        // Where(필터)
        // 리스트를 순회하면서 해당 요소의 지속시간을 감소했을때 0보다 클 경우만 가져온다.
        debuffList = debuffList.Where(debuff => (debuff.continueTime -= GameManager.DeltaTime) > 0.0f).ToList();
    }

    // 이동 관련
    private void Movement()
    {
        // point의 개수가 0이라는 말은 더 이상 갈 목적지가 없다는 말이다.
        // 즉 골에 도착했다.
        if (point.Count <= 0)
        {
            OnGoal();
            return;
        }

        // MoveTowords(Vector3, Vector3, float) : Vector3
        // => A지점에서 B지점까지 float속도로 움직였을 때의 위치를 리턴한다.
        Vector3 destination = point.Peek().position;
        transform.position = Vector3.MoveTowards(transform.position, destination, currentMoveSpeed * GameManager.DeltaTime);

        // 현재 내 위치와 목적지의 거리가 0.0f보다 작다는 말은 동일한 위치에 있다는 말이다.
        // 따라서 Queue의 Dequeue를 호출해 다음 목적지를 가리키게 하자.
        if (Vector3.Distance(transform.position, destination) <= 0.0f)
            point.Dequeue();

        // 오일러 각도로 선언해서 사원수(Quaternion)각도로 바꿔달라는 함수다.
        // 1초 당 90도씩 돌아라.
        transform.rotation = Quaternion.Euler(0, 0, rotate += -360 * GameManager.DeltaTime);

        // 체력바의 위치를 갱신한다.
        // 하지만 내 위치는 몸 중심이기 때문에 오차값을 수정해준다.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }    
    private void OnGoal()
    {
        GameManager.Instance.OnGoalEnemy();         // GM에게 적이 도착했다고 알림.
        OnDeadEnemy();                              // 적 삭제.
    }
    private void OnDeadEnemy()
    {
        EnemySpanwer.Instance.OnDestroyEnemy();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

    // 공격 관련.
    public void OnDamaged(int power)
    {
        hp = Mathf.Clamp(hp - power, 0, 999);
        hpBar.SetHp(hp, maxHp);
        if (hp <= 0)
        {
            GameManager.Instance.AddGold(gold);     // 적이 공격을 받아 죽었을 때 골드 획득.
            OnDeadEnemy();                          // 적 삭제.
        }
    }

    // OnDrawGizmos와의 차이점
    // => 오브젝트를 선택했을 때만 그린다.
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + hpBarOffset, 0.05f);
    }
}
