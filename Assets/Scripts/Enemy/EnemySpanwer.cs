using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : Singleton<EnemySpanwer>
{
    [SerializeField] StageData stageData;   // 
    [SerializeField] Enemy enemyPrefab;     // 적 프리팹.
    [SerializeField] float spawnRate;       // 생성 주기.
    [SerializeField] Transform[] points;    // 적이 이동할 위치 좌표 배열.

    StageInfo stage;
    int wave;
    int maxWave;
    int aliveEnemyCount;    // 살아있는 적의 개수.

    private void Start()
    {
        stageData.Setup();
        stage = stageData.GetStageInfo(1);      // 1스테이지에 대한 웨이브 정보.
        wave = 1;                               // 현재 웨이브.
        maxWave = stage.enemyInfoList.Count;    // 최종 웨이브.
    }

    public void StartWave()
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
        EnemyInfo waveInfo = stage.enemyInfoList[wave -1];
        aliveEnemyCount = waveInfo.count;                   // 생존 적의 개수를 생성 개수로 대입.
        for(int i = 0; i< waveInfo.count; i++)              // 이번 웨이브의 적 생성 개수만큼 반복.
        { 
            // points의 최초 위치에 적을 만든다.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(waveInfo, points);

            // WaitForSecond를 이용하면 시간 배율이 바뀌었을 때.. 바뀌기 전 값으로 초를 세고 있기 때문에
            // 간격을 맞추기 위해서 직접 초를 센다.
            float spawnTime = 0.0f;
            while ((spawnTime += GameManager.DeltaTime) < spawnRate)
                yield return null;
        }
    }
    
}
