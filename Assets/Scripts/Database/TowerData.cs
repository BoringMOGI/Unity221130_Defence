using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum TOWER
{
    Normal,
    Explode,
    Electric,
}

[System.Serializable]
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
    public Debuff debuff;
    public Sprite towerSprite;
    public Tower prefab;
    public ParticleSystem fx;

    public TowerInfo(string csv, DebuffData debuffData)
    {
        string[] datas = csv.Split(',');
        name = datas[0];
        type = (TOWER)System.Enum.Parse(typeof(TOWER), datas[1]);
        level = int.Parse(datas[2]);
        price = int.Parse(datas[3]);
        attackRange = float.Parse(datas[4]);
        attackRate = float.Parse(datas[5]);
        attackPower = int.Parse(datas[6]);
        explodeRange = float.Parse(datas[7]);
        debuff = debuffData.GetDebuff(int.Parse(datas[8]));
        prefab = Resources.Load<Tower>($"Tower/{datas[9]}");
        towerSprite = Resources.Load<Sprite>($"Tower/{datas[10]}");
        fx = Resources.Load<ParticleSystem>($"Fx/{datas[11].Trim()}");
    }
}

public class TowerData : MonoBehaviour
{
    [System.Serializable]
    class TowerGroup
    {
        public TOWER type;              // 그룹의 타입.
        public List<TowerInfo> infos;   // 그룹의 레벨별 타워 정보.
        
        public TowerGroup(TOWER type)
        {
            this.type = type;
            infos = new List<TowerInfo>();
        }

        public TowerInfo GetTowerInfo(int level)
        {
            // 인포 리스트에서 level에 해당하는 info정보를 리턴한다.
            var find = infos.Where(info => info.level == level);
            return find.Count() <= 0 ? null : find.First();
        }
    }

    // 데이터가 대입된 타워 프리팹을 관리하는 클래스.

    [SerializeField] TextAsset dataAsset;
    [SerializeField] DebuffData debuffData;
    [SerializeField] List<TowerGroup> towerGroups;

    private void Start()
    {
        debuffData.Setup();

        string csv = dataAsset.text.Trim();
        string[] lines = csv.Split('\n');
        for (int index = 1; index < lines.Length; index++)
        {
            string line = lines[index];                         // i번째 열 데이터. (가로 한 줄)
            TowerInfo info = new TowerInfo(line, debuffData);   // csv 열 데이터를 넘겨서 값을 설정한다.
            TowerGroup group = GetTowerGroup(info.type);        // info의 type에 해당하는 그룹을 찾는다.
            group.infos.Add(info);
        }
    }
    private TowerGroup GetTowerGroup(TOWER type)
    {
        // info가 들어갈 그룹을 찾는다.
        TowerGroup group = null;
        var find = towerGroups.Where(g => g.type == type);      // 열거자 중에서 type에 해당하는 그룹을 찾는다.

        if (find.Count() <= 0)                      // 개수가 0이하라면 그룹이 없다는 말이기 때문에
        {
            group = new TowerGroup(type);           // 새로운 그룹을 생성하고
            towerGroups.Add(group);                 // List에 대입한다.
        }
        else
        {
            group = find.First();                   // 개수가 1이상이라면 그대로 반환한다.
        }

        return group;
    }


    public TowerInfo GetTowerInfo(TOWER type, int level)
    {
        var find = towerGroups.Where(g => g.type == type);
        if(find.Count () > 0)
        {
            TowerGroup group = find.First();                    // 그룹을 찾는다.
            TowerInfo towerInfo = group.GetTowerInfo(level);    // 그룹중 level에 해당하는 info를 찾는다.
            return towerInfo;                                   // 원하는 타워 정보를 전달한다.
        }

        return null;
    }
}
