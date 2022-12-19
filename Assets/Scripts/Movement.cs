using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        #region 이동
        // transform.position : 절대 좌표상에서의 위치.
        // transform.localPosition : 상대 좌표상에서의 위치. 부모 오브젝트가 없으면 절대 좌표상.

        // transform.position += Vector3 : 현재 위치 좌표에 이동량을 더해서 움직인다.
        // transform.Translate(Vector3) : 현재 좌표를 기준으로 상대적인 좌표로 이동한다.

        // Vector3.MoveTowards(A, B, t) : A위치에서 B위치로 t의 속도로 움직였을때의 좌표를 반환.
        // Vector3.Lerp(A, B, t) : A위치에서 B위치로 t의 비율로 움직였을때의 좌표를 반환.
        #endregion

        // 월드 기준으로 오른쪽이 어디를 향하는가?
        // Vector3.up       : (0, 1, 0)
        // Vector3.down     : (0, -1, 0)
        // Vector3.left     : (-1, 0, 0)
        // Vector3.right    : (1, 0, 0)
        // Vector3.forward  : (0, 0, 1)
        // Vector3.back     : (0, 0, -1)
        // Vector3.zero     : (0, 0, 0)

        // transform.Vector : 내 기준 오른쪽이 절대 좌표상 어디를 향하는가?

        // Quaternion.AngleAxis(angle, axis)
        // => 특정 축(axis)를 기준으로 각도(angle)만큼 도는 회전량.
        
        transform.LookAt(target);                                   // 타겟(Transform)을 바라보게 하라.

        // 내 위치에서 상대까지의 방향 벡터 중 y성분을 지워서 x,z축에 대한 회전량을 0으로 만든다.
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        transform.forward = dir;   // 나의 정면 벡터 방향을 상대를 향하는 방향으로 대입하라.

        // 오일러 각도를 쿼터니언으로 변환시켜주는 함수.
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.rotation *= Quaternion.Euler(0, 90, 0);

        // Target으로 향하는 방향을 정면 방향이 바라보았을 때의 화전량.
        // LookAt이랑 거의 같다.
        transform.rotation = Quaternion.LookRotation(new Vector3(1, 1, 0), Vector3.up);

        // A방향에서 B방향으로 향하는 회전량.
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right);

        // 이동에서의 MoveToward랑 같다. 회전판이다.
        // 이동량 : 상대 위치 - 내 위치 (방향X)
        // 정면 기준 : 내 위치에서 상대 위치를 바라보는 회전량을 구한 뒤 등속으로 회전시킨다.
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 45f * Time.deltaTime);
    }


    // 문제 : float자료형 time 변수의 값이 초당 1의 속도로 오르도록 만들자.
    //  ㄴ 힌트1 : Time.deltaTime은 무엇을 의미하는 값인가?
    //  ㄴ 힌트2 : Update함수는 무엇인가?

    // 문제 : 오른쪽으로 초당 3M씩 움직이는 알고리즘을 만들어라.

    [SerializeField] float time;
    
    //[SerializeField] float angle;
    [SerializeField] float distance;


    Vector3 mouseWorldPosition;
    Vector3 dir;
    void Update()
    {
        //Vector3 movement = target.position - transform.position;
        //float x = movement.x;
        //float y = movement.y;

        //angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90;
        //Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = angleAxis;

        // 문제1 : 초당 90도씩 회전시키는 방법은?
        // ㄴ 힌트 : 내 각도가 초당 90씩 증가해야 한다.
        // angle += Time.deltaTime * (360 / time);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 원점에서 정면으로 distance만큼 이동한 좌표로 가세요.
        //distance += Time.deltaTime;
        //transform.position = Vector3.forward * distance;

        // 지금 너의 위치에서 정면으로 Time.deltaTime만큼 가세요.
        //transform.position += Vector3.forward * Time.deltaTime;

        // transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * (360 / time), Vector3.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mouseWorldPosition, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, dir * 10f);
    }
}
