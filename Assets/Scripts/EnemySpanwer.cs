using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;     // 적 프리팹.
    [SerializeField] float spawnRate;       // 생성 주기.
    [SerializeField] int spawnCount;        // 생성할 개수.
    [SerializeField] Transform[] points;    // 적이 이동할 위치 좌표 배열.

    private void Start()
    {
        /*
        Gacha gacha = new Gacha();

        #region 데이터 초기화.
        gacha.AddItem("5성 벤티", 0.6f);
        gacha.AddItem("5성 진", 0.6f);
        gacha.AddItem("5성 다이루크", 0.6f);
        gacha.AddItem("5성 각청", 0.6f);
        gacha.AddItem("5성 모나", 0.6f);

        gacha.AddItem("4성 중운", 5.1f);
        gacha.AddItem("4성 피슬", 5.1f);
        gacha.AddItem("4성 행추", 5.1f);
        gacha.AddItem("4성 리사", 5.1f);
        gacha.AddItem("4성 레이저", 5.1f);
        gacha.AddItem("4성 응광", 5.1f);
        gacha.AddItem("4성 향릉", 5.1f);
        gacha.AddItem("4성 베넷", 5.1f);
        gacha.AddItem("4성 설탕", 5.1f);

        gacha.AddItem("3성 A", 94.3f);
        gacha.AddItem("3성 B", 94.3f);
        gacha.AddItem("3성 C", 94.3f);
        gacha.AddItem("3성 D", 94.3f);
        gacha.AddItem("3성 E", 94.3f);
        gacha.AddItem("3성 F", 94.3f);
        gacha.AddItem("3성 G", 94.3f);
        gacha.AddItem("3성 H", 94.3f);
        gacha.AddItem("3성 I", 94.3f);
        gacha.AddItem("3성 J", 94.3f);
        gacha.AddItem("3성 K", 94.3f);
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
