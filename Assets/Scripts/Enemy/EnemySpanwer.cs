using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : Singleton<EnemySpanwer>
{
    [SerializeField] Enemy enemyPrefab;     // �� ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.
    [SerializeField] int spawnCount;        // ������ ����.
    [SerializeField] Transform[] points;    // ���� �̵��� ��ġ ��ǥ �迭.

    int aliveEnemyCount;    // ����ִ� ���� ����.

    public void StartSpawn()
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
        aliveEnemyCount = spawnCount;   // ���� ���� ������ ���� ������ ����.
        int remaining = spawnCount;     // ���� ����.
        while(remaining > 0)            // ���� ������ 0���� Ŭ ��.
        {
            // points�� ���� ��ġ�� ���� �����.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(points);
            remaining -= 1;
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
}
