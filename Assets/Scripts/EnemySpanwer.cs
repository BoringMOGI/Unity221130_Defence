using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;     // �� ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.
    [SerializeField] int spawnCount;        // ������ ����.
    [SerializeField] Transform[] points;    // ���� �̵��� ��ġ ��ǥ �迭.

    [ContextMenu("Spawn Enemy")]
    public void StartSpawn()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        int remaining = spawnCount;     // ���� ����.
        while(remaining > 0)            // ���� ������ 0���� Ŭ ��.
        {
            // points�� ���� ��ġ�� ���� �����.
            Enemy newEnemy = Instantiate(enemyPrefab, points[0].position, Quaternion.identity, transform);
            newEnemy.Setup(points);
            remaining -= 1;
            yield return new WaitForSeconds(spawnRate);
        }

        Debug.Log("�� ���� �Ϸ�");
    }
    
}
