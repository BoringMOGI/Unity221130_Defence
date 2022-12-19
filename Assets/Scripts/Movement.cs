using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        #region �̵�
        // transform.position : ���� ��ǥ�󿡼��� ��ġ.
        // transform.localPosition : ��� ��ǥ�󿡼��� ��ġ. �θ� ������Ʈ�� ������ ���� ��ǥ��.

        // transform.position += Vector3 : ���� ��ġ ��ǥ�� �̵����� ���ؼ� �����δ�.
        // transform.Translate(Vector3) : ���� ��ǥ�� �������� ������� ��ǥ�� �̵��Ѵ�.

        // Vector3.MoveTowards(A, B, t) : A��ġ���� B��ġ�� t�� �ӵ��� ������������ ��ǥ�� ��ȯ.
        // Vector3.Lerp(A, B, t) : A��ġ���� B��ġ�� t�� ������ ������������ ��ǥ�� ��ȯ.
        #endregion

        // ���� �������� �������� ��� ���ϴ°�?
        // Vector3.up       : (0, 1, 0)
        // Vector3.down     : (0, -1, 0)
        // Vector3.left     : (-1, 0, 0)
        // Vector3.right    : (1, 0, 0)
        // Vector3.forward  : (0, 0, 1)
        // Vector3.back     : (0, 0, -1)
        // Vector3.zero     : (0, 0, 0)

        // transform.Vector : �� ���� �������� ���� ��ǥ�� ��� ���ϴ°�?

        // Quaternion.AngleAxis(angle, axis)
        // => Ư�� ��(axis)�� �������� ����(angle)��ŭ ���� ȸ����.
        
        transform.LookAt(target);                                   // Ÿ��(Transform)�� �ٶ󺸰� �϶�.

        // �� ��ġ���� �������� ���� ���� �� y������ ������ x,z�࿡ ���� ȸ������ 0���� �����.
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        transform.forward = dir;   // ���� ���� ���� ������ ��븦 ���ϴ� �������� �����϶�.

        // ���Ϸ� ������ ���ʹϾ����� ��ȯ�����ִ� �Լ�.
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.rotation *= Quaternion.Euler(0, 90, 0);

        // Target���� ���ϴ� ������ ���� ������ �ٶ󺸾��� ���� ȭ����.
        // LookAt�̶� ���� ����.
        transform.rotation = Quaternion.LookRotation(new Vector3(1, 1, 0), Vector3.up);

        // A���⿡�� B�������� ���ϴ� ȸ����.
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right);

        // �̵������� MoveToward�� ����. ȸ�����̴�.
        // �̵��� : ��� ��ġ - �� ��ġ (����X)
        // ���� ���� : �� ��ġ���� ��� ��ġ�� �ٶ󺸴� ȸ������ ���� �� ������� ȸ����Ų��.
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 45f * Time.deltaTime);
    }


    // ���� : float�ڷ��� time ������ ���� �ʴ� 1�� �ӵ��� �������� ������.
    //  �� ��Ʈ1 : Time.deltaTime�� ������ �ǹ��ϴ� ���ΰ�?
    //  �� ��Ʈ2 : Update�Լ��� �����ΰ�?

    // ���� : ���������� �ʴ� 3M�� �����̴� �˰����� ������.

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

        // ����1 : �ʴ� 90���� ȸ����Ű�� �����?
        // �� ��Ʈ : �� ������ �ʴ� 90�� �����ؾ� �Ѵ�.
        // angle += Time.deltaTime * (360 / time);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // �������� �������� distance��ŭ �̵��� ��ǥ�� ������.
        //distance += Time.deltaTime;
        //transform.position = Vector3.forward * distance;

        // ���� ���� ��ġ���� �������� Time.deltaTime��ŭ ������.
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
