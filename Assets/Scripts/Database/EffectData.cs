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
            string fxName = datas[1].Trim();        // 이펙트의 이름.

            ParticleSystem fx = null;
            foreach(ParticleSystem prefab in effectPrefabs)
            {
                // i번째 데이터 속 이펙트의 이름과 실제 프리팹 배열의 이름이 같은가?
                // 문자열 비교시 중요하지 않다면 두 대상을 전부 소문자로 만들어서 비교.
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
        // 예외) 저장소 안에 id에 해당하는 키 값이 없기 때문에 null을 반환한다.
        if (storage.ContainsKey(id) == false)
            return null;

        return storage[id];
    }
}
