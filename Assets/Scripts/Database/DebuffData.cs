using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public enum DEBUFF
{
    Slow,   // ���ο�.
    Etc2,   // �ӽ� 1
    Etc3,   // �ӽ� 2

    Count,
}

[System.Serializable]
public class Debuff
{
    public string name;         // ����� �̸�.
    public DEBUFF type;         // ����� ����.
    public int priority;        // �켱 ����.
    public float continueTime;  // ���� �ð�.
    public float value;         // ������� ���� Ư�� ��.

    public Debuff GetCopy()
    {
        Debuff copy = new Debuff();

        copy.name = name;
        copy.type = type;
        copy.priority = priority;
        copy.continueTime = continueTime;
        copy.value = value;

        return copy;
    }
}


public class DebuffData : MonoBehaviour
{
    [SerializeField] string fileName;

    Dictionary<int, Debuff> storage;

    public void Setup()
    {
        storage = new Dictionary<int, Debuff>();

        TextAsset debuffFile= Resources.Load<TextAsset>(fileName);

        string tsv = debuffFile.text.Trim();
        string[] lines = tsv.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split('\t');
            Debuff debuff = new Debuff();

            int id = int.Parse(datas[0]);
            debuff.name = datas[1];
            debuff.type = (DEBUFF)System.Enum.Parse(typeof(DEBUFF), datas[2]);
            debuff.priority = int.Parse(datas[3]);
            debuff.continueTime = float.Parse(datas[4]);
            debuff.value = float.Parse(datas[5]);

            storage.Add(id, debuff);
        }
    }

    public Debuff GetDebuff(int id)
    {
        if (storage.ContainsKey(id) == false)
            return null;

        return storage[id];
    }

}
