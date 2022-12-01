using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;     // 적 프리팹.
    [SerializeField] float spawnRate;       // 생성 주기.
    [SerializeField] int spawnCount;        // 생성할 개수.
    [SerializeField] Transform[] points;    // 적이 이동할 위치 좌표 배열.

    [ContextMenu("Spawn Enemy")]
    public void StartSpawn()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        int remaining = spawnCount;     // 남은 개수.
        while(remaining > 0)            // 남은 개수가 0보다 클 때.
        {
            // points의 최초 위치에 적을 만든다.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(points);
            remaining -= 1;
            yield return new WaitForSeconds(spawnRate);
        }

        Debug.Log("적 생성 완료");
    }
    
}
