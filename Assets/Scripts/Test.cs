using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        transform.position = new Vector3(-12, 0, 0);
        transform.Translate(Vector3.right * 2);                // (���� ��ġ����) ���������� 2��ŭ �̵��ض�.
        // = transform.position += Vector3.right * 2;

        transform.rotation = Quaternion.Euler(0, 0, 90);        // z������ 90���� ���ض�.
        transform.Rotate(Vector3.forward * 30f);                // ���� �������� z������ 30��ŭ ���ƶ�.
    }
    private void Update()
    {
        // �Ʒ��� forward�� �������� ��� 3D���� ����ϴ� ���.
        // 2D�� ������ ���� up�̴�.
        // transform.LookAt(target.position);      // LookAt(Transform) : �ش� ������ �ٶ����.

        // Quaternion.FromToRotation(Vector3 a, Vector3 b) : Quaternion
        // = a���� �������� ��� b�������� �ٶ����.
        transform.rotation = Quaternion.FromToRotation(Vector3.up, target.position - transform.position);
    }
}
