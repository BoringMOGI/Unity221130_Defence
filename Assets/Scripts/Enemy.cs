using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Queue<Transform> point; // 적이 움직일 위치 지점 큐.
    float rotate;

    public void Setup(Transform[] points)
    {
        // 배열도 큐도 콜렉션의 일종이기 때문에 같은 원리로 초기화가 가능하다.
        point = new Queue<Transform>(points);
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
    }

    private void OnGoal()
    {
        Destroy(gameObject);
    }
}
