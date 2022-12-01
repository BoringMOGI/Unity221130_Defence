using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        transform.position = new Vector3(-12, 0, 0);
        transform.Translate(Vector3.right * 2);                // (현재 위치에서) 오른쪽으로 2만큼 이동해라.
        // = transform.position += Vector3.right * 2;

        transform.rotation = Quaternion.Euler(0, 0, 90);        // z축으로 90도를 향해라.
        transform.Rotate(Vector3.forward * 30f);                // 현재 각도에서 z축으로 30만큼 돌아라.
    }
    private void Update()
    {
        // 아래는 forward를 정면으로 삼는 3D에서 사용하는 방식.
        // 2D는 정면이 보통 up이다.
        // transform.LookAt(target.position);      // LookAt(Transform) : 해당 지점을 바라봐라.

        // Quaternion.FromToRotation(Vector3 a, Vector3 b) : Quaternion
        // = a축을 정면으로 삼고 b방향으로 바라봐라.
        transform.rotation = Quaternion.FromToRotation(Vector3.up, target.position - transform.position);
    }
}
