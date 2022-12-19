using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] float readyTime;
    [SerializeField] float blinkTime;
    [SerializeField] SpriteRenderer pointSprite;
    [SerializeField] Enemy2 enemyPrefab;

    IEnumerator Start()
    {
        float time = readyTime;
        float blindTime = blinkTime;
        while ((time -= Time.deltaTime) > 0.0f)
        {
            blindTime -= Time.deltaTime;
            if (blindTime <= 0.0f)
            {
                pointSprite.enabled = !pointSprite.enabled;
                blindTime = blinkTime;
            }
            yield return null;
        }

        Instantiate(enemyPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
}
