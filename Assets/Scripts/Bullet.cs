using UnityEngine;

public class Bullet : MonoBehaviour
{
    Enemy target;
    float moveSpeed;
    int power;

    // 총알은 최초 세팅 시 타겟을 받는다.
    public void Setup(Enemy target, float moveSpeed, int power)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.power = power;
    }
    private void Update()
    {
        // 타겟을 향해 날아가는 도중 타겟이 삭제된다면.. 총알도 사라진다.
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        // MoveTowords(A, B, T)
        // => A지점에서 B지점으로 향할 때 T만큼 움직인 위치 값을 반환한다.
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        // 나와 타겟의 거리가 0.0f 이하 즉, 동일하다면 데미지를 전달하고 삭제된다.
        if(Vector3.Distance(transform.position, target.transform.position) <= 0.0f)
        {
            target.OnDamaged(power);
            Destroy(gameObject);
        }

    }
}
