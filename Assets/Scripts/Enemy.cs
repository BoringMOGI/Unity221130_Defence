using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Queue<Transform> point; // ���� ������ ��ġ ���� ť.
    float rotate;

    public void Setup(Transform[] points)
    {
        // �迭�� ť�� �ݷ����� �����̱� ������ ���� ������ �ʱ�ȭ�� �����ϴ�.
        point = new Queue<Transform>(points);
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
    }

    private void OnGoal()
    {
        Destroy(gameObject);
    }
}
