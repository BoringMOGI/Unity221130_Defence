using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : Singleton<EnemySpanwer>
{
    [SerializeField] StageData stageData;   // 
    [SerializeField] Enemy enemyPrefab;     // �� ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.
    [SerializeField] Transform[] points;    // ���� �̵��� ��ġ ��ǥ �迭.

    StageInfo stage;
    int wave;
    int maxWave;
    int aliveEnemyCount;    // ����ִ� ���� ����.

    private void Start()
    {
        stageData.Setup();
        stage = stageData.GetStageInfo(1);      // 1���������� ���� ���̺� ����.
        wave = 1;                               // ���� ���̺�.
        maxWave = stage.enemyInfoList.Count;    // ���� ���̺�.
    }

    public void StartWave()
    {
        StartCoroutine(Spawn());
    }

    // ������ ���� �ı� ������ �� ȣ��Ǵ� �Լ�.
    public void OnDestroyEnemy()
    {
        aliveEnemyCount -= 1;       // ����ִ� ���� ���� ����.
        if(aliveEnemyCount <= 0)    // �� �̻� ����ִ� ���� ���ٸ�..
        {
            GameManager.Instance.OnEndWave();   // GM���� ���̺갡 �������� �˸���.
        }
    }

    private IEnumerator Spawn()
    {
        EnemyInfo waveInfo = stage.enemyInfoList[wave -1];
        aliveEnemyCount = waveInfo.count;                   // ���� ���� ������ ���� ������ ����.
        for(int i = 0; i< waveInfo.count; i++)              // �̹� ���̺��� �� ���� ������ŭ �ݺ�.
        { 
            // points�� ���� ��ġ�� ���� �����.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(waveInfo, points);

            // WaitForSecond�� �̿��ϸ� �ð� ������ �ٲ���� ��.. �ٲ�� �� ������ �ʸ� ���� �ֱ� ������
            // ������ ���߱� ���ؼ� ���� �ʸ� ����.
            float spawnTime = 0.0f;
            while ((spawnTime += GameManager.DeltaTime) < spawnRate)
                yield return null;
        }
    }
    
}
