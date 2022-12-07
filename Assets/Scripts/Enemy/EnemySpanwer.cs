using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : Singleton<EnemySpanwer>
{
    [SerializeField] Enemy enemyPrefab;     // 적 프리팹.
    [SerializeField] float spawnRate;       // 생성 주기.
    [SerializeField] int spawnCount;        // 생성할 개수.
    [SerializeField] Transform[] points;    // 적이 이동할 위치 좌표 배열.

    int aliveEnemyCount;    // 살아있는 적의 개수.

    public void StartSpawn()
    {
        StartCoroutine(Spawn());
    }

    // 생성된 적이 파괴 당했을 때 호출되는 함수.
    public void OnDestroyEnemy()
    {
        aliveEnemyCount -= 1;       // 살아있는 적의 개수 감소.
        if(aliveEnemyCount <= 0)    // 더 이상 살아있는 적이 없다면..
        {
            GameManager.Instance.OnEndWave();   // GM에게 웨이브가 끝났음을 알린다.
        }
    }

    private IEnumerator Spawn()
    {
        aliveEnemyCount = spawnCount;   // 생존 적의 개수를 생성 개수로 대입.
        int remaining = spawnCount;     // 남은 개수.
        while(remaining > 0)            // 남은 개수가 0보다 클 때.
        {
            // points의 최초 위치에 적을 만든다.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(points);
            remaining -= 1;
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
}
