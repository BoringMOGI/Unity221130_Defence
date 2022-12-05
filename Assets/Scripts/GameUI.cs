using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] HpBar HpBarPrefab;
    [SerializeField] Text goldText;     // ���� ��� �ؽ�Ʈ.
    [SerializeField] Text lifeText;     // ������ �ؽ�Ʈ.
    [SerializeField] Text waveText;     // ���̺� �ؽ�Ʈ.

    [Header("Wave Control")]
    [SerializeField] GameObject waveControlPanel;
    [SerializeField] Text waveControlText;

    public HpBar GetHpBar()
    {
        return Instantiate(HpBarPrefab, transform);
    }

    public void UpdateGoldText(int gold)
    {
        // ��带 ��ǥ �������� ����.
        goldText.text = gold.ToString("#,##0");
    }
    public void UpdateHpText(int hp)
    {
        lifeText.text = hp.ToString();
    }
    public void UpdateWaveText(int wave, int maxWave)
    {
        waveText.text = $"{wave}/{maxWave}";
    }

    public void UpdateWaveControl(float readyTime)
    {
        waveControlPanel.SetActive(readyTime > 0.0f);   // ��� �ð��� �ִٸ� ��Ʈ�� �г��� �Ҵ�.

        // �츮�� �� ������ ����Ϸ��� �Ѵ�.
        // ��ǻ�ʹ� 1�ʰ� �������� 0.N�ʸ� ������ ������ ������ �ø��Ѵ�.
        // Ceil:�ø�, Round:�ݿø�, Floor:����.
        int time = Mathf.CeilToInt(readyTime);
        waveControlText.text = $"���� ���̺���� {time}��";
    }
}