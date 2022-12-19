using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float moveSpeed;


    Transform player;

    private void Start()
    {
        player = Movement2D.Instance.transform;
    }

    private void Update()
    {
        Vector3 dir = (player.position - transform.position).normalized * moveSpeed;
        rigid.velocity = dir;
    }

}
