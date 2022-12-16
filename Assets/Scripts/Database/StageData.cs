using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public int stage;
    public int wave;
    public string name;
    public Sprite sprite;
    public int count;
    public int hp;
    public float speed;
    public int gold;

    public EnemyInfo(string row)
    {
        string[] datas = row.Trim().Split(',');

        stage = int.Parse(datas[0]);
        wave = int.Parse(datas[1]);
        name = datas[2];
        sprite = Resources.Load<Sprite>($"Enemy/{datas[3]}");
        count = int.Parse(datas[4]);
        hp = int.Parse(datas[5]);
        speed = float.Parse(datas[6]);
        gold = int.Parse(datas[7]);
    }
}

[System.Serializable]
public class StageInfo
{
    public int stage;
    public List<EnemyInfo> enemyInfoList;
    public StageInfo(int stage)
    {
        this.stage = stage;
        enemyInfoList = new List<EnemyInfo>();
    }
}

public class StageData : MonoBehaviour
{
    [SerializeField] TextAsset csvFile;
    [SerializeField] List<StageInfo> stageList = new List<StageInfo>();

    public void Setup()
    {
        string[] csvs = csvFile.text.Trim().Split('\n');
        for(int i = 1; i < csvs.Length; i++)
        {
            string csv = csvs[i];
            EnemyInfo enemy = new EnemyInfo(csv);
            StageInfo stage = GetStageInfo(enemy.stage);
            stage.enemyInfoList.Add(enemy);
        }
    }

    // stageList에서 index에 해당하는 stage데이터를 찾는다.
    public StageInfo GetStageInfo(int index)
    {
        var find = stageList.Where(s => s.stage == index);
        StageInfo stage = null;
        if(find.Count() <= 0)
        {
            stage = new StageInfo(index);
            stageList.Add(stage);
        }
        else
        {
            stage = find.First();
        }
        return stage;
    }
}
