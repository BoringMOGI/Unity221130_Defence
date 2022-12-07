using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일반화 자료형 T의 제한사항은 class(참조 형식)이다.
public class Singleton<T> : MonoBehaviour
    where T : class
{
    public static T Instance { get; private set; }
    private void Awake()
    {
        // this는 나를 가리킨다. 이것을 T자료형으로 변환시킨다.
        Instance = this as T;
    }
}
