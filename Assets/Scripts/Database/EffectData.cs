using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EffectData : MonoBehaviour
{
    struct Data
    {
        public int id;
        public ParticleSystem effect;
    }

    [SerializeField] string fileName;                   // ����Ʈ TSV ����.
    [SerializeField] ParticleSystem[] effectPrefabs;    // ����Ʈ ������ �迭.

    Dictionary<int, ParticleSystem> storage;            // �����.

    public void Setup()
    {
        storage = new Dictionary<int, ParticleSystem>();
        TextAsset effectFile = Resources.Load<TextAsset>(fileName);

        string tsv = effectFile.text.Trim();    // tsv������ �� �� ������ ������ �� ����.
        string[] lines = tsv.Split('\n');       // �ٶ��(\n)�������� �ڸ���.
        for(int i = 1; i < lines.Length; i++)   // 0��° ������ Ű ���̱� ������ �����Ѵ�.
        {
            string[] datas = lines[i].Split('\t');  // �� ������ ��(Tab)�������� �ڸ���.
            int id = int.Parse(datas[0]);           // ����Ʈ�� ID ��.
            string fxName = datas[1].Trim();        // ����Ʈ�� �̸�.

            ParticleSystem fx = null;
            foreach(ParticleSystem prefab in effectPrefabs)
            {
                // i��° ������ �� ����Ʈ�� �̸��� ���� ������ �迭�� �̸��� ������?
                // ���ڿ� �񱳽� �߿����� �ʴٸ� �� ����� ���� �ҹ��ڷ� ���� ��.
                if(prefab.name.ToLower().Contains(fxName.ToLower()))
                {
                    fx = prefab;
                    break;
                }
            }
            storage.Add(id, fx);
        }
    }

    public ParticleSystem GetEffect(int id)
    {
        // ����) ����� �ȿ� id�� �ش��ϴ� Ű ���� ���� ������ null�� ��ȯ�Ѵ�.
        if (storage.ContainsKey(id) == false)
            return null;

        return storage[id];
    }
}
