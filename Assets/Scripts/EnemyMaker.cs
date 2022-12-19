using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;    // 적 프리팹.
    [SerializeField] BoxCollider2D boundary;    // 맵 경계.
    [SerializeField] float makeRate;            // 생성 주기.
    [SerializeField] int createCount;           // 생성 개수.
    [SerializeField] float minRange;            // 최소 거리.
    [SerializeField] float maxRange;            // 최대 거리.
    [SerializeField] Transform player;          // 플레이어 위치.

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
