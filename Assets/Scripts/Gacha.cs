using UnityEngine;
using System.Collections.Generic;

class Gacha
{
    class Item
    {
        public string name;
        public float weight;
        public Item(string name, float weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }
    List<Item> itemList;        // 아이템 리스트 (물품)
    float totalWeight;            // 가중치 총합.
    public Gacha()
    {
        itemList = new List<Item>();
        totalWeight = 0;
    }
    public void AddItem(string name, float weight)
    {
        itemList.Add(new Item(name, weight));
        totalWeight += weight;
    }
    public string GetGacha()
    {
        float pick = totalWeight * Random.value;
        float next = 0;
        foreach(Item item in itemList)
        {
            next += item.weight;        // 다음 비교 값을 계산한다.
            if (pick < next)            // 랜덤하게 찍힌 지점이 비교 값보다 작다면
                return item.name;       // 해당 아이템의 이름을 반환한다.
        }

        return string.Empty;
    }
}
