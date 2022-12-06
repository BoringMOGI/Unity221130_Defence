using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int hp;
    [SerializeField] int gold;
    [SerializeField] Vector3 hpBarOffset;

    Queue<Transform> point; // 적이 움직일 위치 지점 큐.
    float rotate;
    HpBar hpBar;
    int maxHp;

    public void Setup(Transform[] points)
    {
        // 배열도 큐도 콜렉션의 일종이기 때문에 같은 원리로 초기화가 가능하다.
        point = new Queue<Transform>(points);
        hpBar = GameUI.Instance.GetHpBar();         // GamUI에게서 체력바 하나를 받아온다.
        maxHp = hp;                                 // 내 최대 체력을 현재 체력으로 대입한다.

        // 1프레임 오차 때문에 받아오자 마자 위치를 갱신해준다.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    void Update()
    {
        // point의 개수가 0이라는 말은 더 이상 갈 목적지가 없다는 말이다.
        // 즉 골에 도착했다.
        if(point.Count <= 0)
        {
            OnGoal();
            return;
        }

        // MoveTowords(Vector3, Vector3, float) : Vector3
        // => A지점에서 B지점까지 float속도로 움직였을 때의 위치를 리턴한다.
        Vector3 destination = point.Peek().position;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // 현재 내 위치와 목적지의 거리가 0.0f보다 작다는 말은 동일한 위치에 있다는 말이다.
        // 따라서 Queue의 Dequeue를 호출해 다음 목적지를 가리키게 하자.
        if (Vector3.Distance(transform.position, destination) <= 0.0f)
            point.Dequeue();

        // 오일러 각도로 선언해서 사원수(Quaternion)각도로 바꿔달라는 함수다.
        // 1초 당 90도씩 돌아라.
        transform.rotation = Quaternion.Euler(0, 0, rotate += -360 * Time.deltaTime);

        // 체력바의 위치를 갱신한다.
        // 하지만 내 위치는 몸 중심이기 때문에 오차값을 수정해준다.
        hpBar.SetPosition(transform.position + hpBarOffset);
    }

    public void OnDamaged(int power)
    {
        hp = Mathf.Clamp(hp - power, 0, 999);
        hpBar.SetHp(hp, maxHp);
        if(hp <= 0)
        {
            GameManager.Instance.AddGold(gold);     // 적이 공격을 받아 죽었을 때 골드 획득.
            OnDeadEnemy();                          // 적 삭제.
        }
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

    // OnDrawGizmos와의 차이점
    // => 오브젝트를 선택했을 때만 그린다.
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + hpBarOffset, 0.05f);
    }
}
