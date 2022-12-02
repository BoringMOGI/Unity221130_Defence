using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image hpImage;

    public void SetPosition(Vector3 position)
    {
        // �Ű������� �޴� position�� ����� ��ǥ(World Space)�̴�.
        // ������ UI�� HpBar�� ȭ��� ��ǥ(Screen Space)�̱� ������ ��ǥ ��ȯ�� �ʿ��ϴ�.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        transform.position = screenPosition;
    }
    public void SetHp(float hp, float maxHp)
    {
        // ��ü ü�� ��� ���� ü���� �������� ������ ����ؼ� ����.
        hpImage.fillAmount = hp / maxHp;
    }
}
