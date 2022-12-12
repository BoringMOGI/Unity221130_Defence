using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TOWER
{
    Normal,
    Explode,
    Electric,
}
public class TowerInfo
{
    public string name;
    public TOWER type;
    public int level;
    public int price;
    public float attackRange;
    public float attackRate;
    public int attackPower;
    public float explodeRange;
    public ParticleSystem fx;
    public Debuff debuff;

    public TowerInfo GetCopy()
    {
        TowerInfo copy = new TowerInfo();
        copy.name = name;
        copy.type = type;
        copy.level = level;
        copy.price = price;
        copy.attackRange = attackRange;
        copy.attackRate = attackRate;
        copy.attackPower = attackPower;
        copy.explodeRange = explodeRange;
        copy.fx = fx;                       // fx는 프리팹이기 때문에 상관없다. (나중에 인스턴스하기 때문)
        copy.debuff = debuff.GetCopy();     // 참조 값인 debuff를 그냥 대입하면 복사가 아니다.
        return copy;
    }
}

public class TowerData : MonoBehaviour
{
    [System.Serializable]
    struct TowerGroup
    {
        public TOWER type;
        public Tower[] prefabs;
    }

    // 데이터가 대입된 타워 프리팹을 관리하는 클래스.

    [SerializeField] string fileName;
    [SerializeField] EffectData effectData;
    [SerializeField] DebuffData debuffData;
    [SerializeField] TowerGroup[] towerGroups;

    private void Start()
    {
        effectData.Setup();
        debuffData.Setup();

        TextAsset towerFile = Resources.Load<TextAsset>(fileName);

        string tsv = towerFile.text.Trim();
        string[] lines = tsv.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            TowerInfo info = new TowerInfo();

            #region TSV파싱
            string[] datas = lines[i].Split('\t');
            info.name = datas[0];
            info.type = (TOWER)System.Enum.Parse(typeof(TOWER), datas[1]);
            info.level = int.Parse(datas[2]);
            info.price = int.Parse(datas[3]);
            info.attackRange = float.Parse(datas[4]);
            info.attackRate = float.Parse(datas[5]);
            info.attackPower = int.Parse(datas[6]);
            info.explodeRange = float.Parse(datas[7]);
            info.fx = effectData.GetEffect(int.Parse(datas[8]));
            info.debuff = debuffData.GetDebuff(int.Parse(datas[9]));
            #endregion

            // 타워 프리팹 배열에서 info값에 해당하는 프리팹을 찾아라.
            // 해당 프리팹에게 info데이터를 세팅한다.
            Tower[] prefabs = towerGroups.Where(g => g.type == info.type).First().prefabs;
            if (info.level <= prefabs.Length)
                prefabs[info.level - 1].Setup(info);
        }
    }


    public Tower GetTowerPrefab(TOWER type, int level)
    {
        // 배열에서 type에 해당하는 그룹을 찾고 level - 1번째 타워 프리팹을 전달한다.
        return towerGroups.Where(t => t.type == type).First().prefabs[level - 1];
    }

}
