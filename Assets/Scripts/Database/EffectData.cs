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

    [SerializeField] string fileName;                   // 이펙트 TSV 파일.
    [SerializeField] ParticleSystem[] effectPrefabs;    // 이펙트 프리팹 배열.

    Dictionary<int, ParticleSystem> storage;            // 저장소.

    public void Setup()
    {
        storage = new Dictionary<int, ParticleSystem>();
        TextAsset effectFile = Resources.Load<TextAsset>(fileName);

        string tsv = effectFile.text.Trim();    // tsv파일을 앞 뒤 공백을 제거한 후 대입.
        string[] lines = tsv.Split('\n');       // 줄띄움(\n)기준으로 자른다.
        for(int i = 1; i < lines.Length; i++)   // 0번째 라인은 키 값이기 때문에 무시한다.
        {
            string[] datas = lines[i].Split('\t');  // 한 라인을 탭(Tab)기준으로 자른다.
            int id = int.Parse(datas[0]);           // 이펙트의 ID 값.
            string fxName = datas[1];               // 이펙트의 이름.

            // 파티클 배열에서 이름이 같은 이펙트 하나를 찾는다.
            ParticleSystem fx = effectPrefabs.Where(fx => fx.name.Equals(fxName)).First();
            storage.Add(id, fx);
        }
    }

    public ParticleSystem GetEffect(int id)
    {
        // 예외) 저장소 안에 id에 해당하는 키 값이 없기 때문에 null을 반환한다.
        if (storage.ContainsKey(id) == false)
            return null;

        return storage[id];
    }
}
