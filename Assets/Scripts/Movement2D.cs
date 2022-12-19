using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public static Movement2D Instance { get; private set; }
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0).normalized;              // 방향 벡터 정규화.
        transform.position += dir * moveSpeed * Time.deltaTime;     // 방향 벡터 * 속도 * 델타 타임.
    }
}
