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
        copy.fx = fx;                       // fx�� �������̱� ������ �������. (���߿� �ν��Ͻ��ϱ� ����)
        copy.debuff = debuff.GetCopy();     // ���� ���� debuff�� �׳� �����ϸ� ���簡 �ƴϴ�.
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

    // �����Ͱ� ���Ե� Ÿ�� �������� �����ϴ� Ŭ����.

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

            #region TSV�Ľ�
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

            // Ÿ�� ������ �迭���� info���� �ش��ϴ� �������� ã�ƶ�.
            // �ش� �����տ��� info�����͸� �����Ѵ�.
            Tower[] prefabs = towerGroups.Where(g => g.type == info.type).First().prefabs;
            if (info.level <= prefabs.Length)
                prefabs[info.level - 1].Setup(info);
        }
    }


    public Tower GetTowerPrefab(TOWER type, int level)
    {
        // �迭���� type�� �ش��ϴ� �׷��� ã�� level - 1��° Ÿ�� �������� �����Ѵ�.
        return towerGroups.Where(t => t.type == type).First().prefabs[level - 1];
    }

}
