using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;    // �� ������.
    [SerializeField] BoxCollider2D boundary;    // �� ���.
    [SerializeField] float makeRate;            // ���� �ֱ�.
    [SerializeField] int createCount;           // ���� ����.
    [SerializeField] float minRange;            // �ּ� �Ÿ�.
    [SerializeField] float maxRange;            // �ִ� �Ÿ�.
    [SerializeField] Transform player;          // �÷��̾� ��ġ.

    float timer = 0.0f;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= makeRate)
        {
            timer = 0.0f;
            StartCoroutine(MakeEnemy());
        }
    }

    private IEnumerator MakeEnemy()
    {
        Vector3 offset = Random.insideUnitCircle.normalized * Random.Range(minRange, maxRange);
        Vector3 createPoint = boundary.bounds.ClosestPoint(player.position + offset);

        for(int i = 0; i< createCount; i++)
        {
            Vector3 point = boundary.bounds.ClosestPoint(createPoint + (Vector3)Random.insideUnitCircle);
            Instantiate(enemyPrefab, point, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    private void OnDrawGizmos()
    {
        if(player != null)
        {
            Gizmos.DrawWireSphere(player.position, minRange);
            Gizmos.DrawWireSphere(player.position, maxRange);
        }
    }
}
