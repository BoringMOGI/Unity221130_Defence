using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ϲ�ȭ �ڷ��� T�� ���ѻ����� class(���� ����)�̴�.
public class Singleton<T> : MonoBehaviour
    where T : class
{
    public static T Instance { get; private set; }
    private void Awake()
    {
        // this�� ���� ����Ų��. �̰��� T�ڷ������� ��ȯ��Ų��.
        Instance = this as T;
    }
}
