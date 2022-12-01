using UnityEngine;

public class Bullet : MonoBehaviour
{
    Enemy target;
    float moveSpeed;
    int power;

    // �Ѿ��� ���� ���� �� Ÿ���� �޴´�.
    public void Setup(Enemy target, float moveSpeed, int power)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.power = power;
    }
    private void Update()
    {
        // Ÿ���� ���� ���ư��� ���� Ÿ���� �����ȴٸ�.. �Ѿ˵� �������.
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        // MoveTowords(A, B, T)
        // => A�������� B�������� ���� �� T��ŭ ������ ��ġ ���� ��ȯ�Ѵ�.
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        // ���� Ÿ���� �Ÿ��� 0.0f ���� ��, �����ϴٸ� �������� �����ϰ� �����ȴ�.
        if(Vector3.Distance(transform.position, target.transform.position) <= 0.0f)
        {
            target.OnDamaged(power);
            Destroy(gameObject);
        }

    }
}
