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
        public TOWER type;              // �׷��� Ÿ��.
        public List<TowerInfo> infos;   // �׷��� ������ Ÿ�� ����.
        
        public TowerGroup(TOWER type)
        {
            this.type = type;
            infos = new List<TowerInfo>();
        }

        public TowerInfo GetTowerInfo(int level)
        {
            // ���� ����Ʈ���� level�� �ش��ϴ� info������ �����Ѵ�.
            var find = infos.Where(info => info.level == level);
            return find.Count() <= 0 ? null : find.First();
        }
    }

    // �����Ͱ� ���Ե� Ÿ�� �������� �����ϴ� Ŭ����.

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
            string line = lines[index];                         // i��° �� ������. (���� �� ��)
            TowerInfo info = new TowerInfo(line, debuffData);   // csv �� �����͸� �Ѱܼ� ���� �����Ѵ�.
            TowerGroup group = GetTowerGroup(info.type);        // info�� type�� �ش��ϴ� �׷��� ã�´�.
            group.infos.Add(info);
        }
    }
    private TowerGroup GetTowerGroup(TOWER type)
    {
        // info�� �� �׷��� ã�´�.
        TowerGroup group = null;
        var find = towerGroups.Where(g => g.type == type);      // ������ �߿��� type�� �ش��ϴ� �׷��� ã�´�.

        if (find.Count() <= 0)                      // ������ 0���϶�� �׷��� ���ٴ� ���̱� ������
        {
            group = new TowerGroup(type);           // ���ο� �׷��� �����ϰ�
            towerGroups.Add(group);                 // List�� �����Ѵ�.
        }
        else
        {
            group = find.First();                   // ������ 1�̻��̶�� �״�� ��ȯ�Ѵ�.
        }

        return group;
    }


    public TowerInfo GetTowerInfo(TOWER type, int level)
    {
        var find = towerGroups.Where(g => g.type == type);
        if(find.Count () > 0)
        {
            TowerGroup group = find.First();                    // �׷��� ã�´�.
            TowerInfo towerInfo = group.GetTowerInfo(level);    // �׷��� level�� �ش��ϴ� info�� ã�´�.
            return towerInfo;                                   // ���ϴ� Ÿ�� ������ �����Ѵ�.
        }

        return null;
    }
}
