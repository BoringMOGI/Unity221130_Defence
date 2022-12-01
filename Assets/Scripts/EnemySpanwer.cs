using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;     // �� ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.
    [SerializeField] int spawnCount;        // ������ ����.
    [SerializeField] Transform[] points;    // ���� �̵��� ��ġ ��ǥ �迭.

    private void Start()
    {
        /*
        Gacha gacha = new Gacha();

        #region ������ �ʱ�ȭ.
        gacha.AddItem("5�� ��Ƽ", 0.6f);
        gacha.AddItem("5�� ��", 0.6f);
        gacha.AddItem("5�� ���̷�ũ", 0.6f);
        gacha.AddItem("5�� ��û", 0.6f);
        gacha.AddItem("5�� ��", 0.6f);

        gacha.AddItem("4�� �߿�", 5.1f);
        gacha.AddItem("4�� �ǽ�", 5.1f);
        gacha.AddItem("4�� ����", 5.1f);
        gacha.AddItem("4�� ����", 5.1f);
        gacha.AddItem("4�� ������", 5.1f);
        gacha.AddItem("4�� ����", 5.1f);
        gacha.AddItem("4�� �⸪", 5.1f);
        gacha.AddItem("4�� ����", 5.1f);
        gacha.AddItem("4�� ����", 5.1f);

        gacha.AddItem("3�� A", 94.3f);
        gacha.AddItem("3�� B", 94.3f);
        gacha.AddItem("3�� C", 94.3f);
        gacha.AddItem("3�� D", 94.3f);
        gacha.AddItem("3�� E", 94.3f);
        gacha.AddItem("3�� F", 94.3f);
        gacha.AddItem("3�� G", 94.3f);
        gacha.AddItem("3�� H", 94.3f);
        gacha.AddItem("3�� I", 94.3f);
        gacha.AddItem("3�� J", 94.3f);
        gacha.AddItem("3�� K", 94.3f);
        #endregion

        BuyGacha(gacha, 10);
        */
    }
    private void BuyGacha(Gacha gacha, int count)
    {
        for (int i = 0; i < count; i++)
            Debug.Log(gacha.GetGacha());
    }

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
