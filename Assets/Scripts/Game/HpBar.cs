using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image hpImage;

    public void SetPosition(Vector3 position)
    {
        // 매개변수로 받는 position은 월드상 좌표(World Space)이다.
        // 하지만 UI인 HpBar는 화면상 좌표(Screen Space)이기 때문에 좌표 변환이 필요하다.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        transform.position = screenPosition;
    }
    public void SetHp(float hp, float maxHp)
    {
        // 전체 체력 대비 현재 체력이 얼마인지를 비율로 계산해서 전달.
        hpImage.fillAmount = hp / maxHp;
    }
}
