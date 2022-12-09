using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] HpBar HpBarPrefab;
    [SerializeField] Text goldText;     // 소지 골드 텍스트.
    [SerializeField] Text lifeText;     // 생명력 텍스트.
    [SerializeField] Text waveText;     // 웨이브 텍스트.
    [SerializeField] Text speedText;    // 속도 텍스트.

    [Header("Wave Control")]
    [SerializeField] GameObject waveControlPanel;
    [SerializeField] Text waveControlText;

    [Header("Result")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    public HpBar GetHpBar()
    {
        return Instantiate(HpBarPrefab, transform);
    }

    public void UpdateGoldText(int gold)
    {
        // 골드를 쉼표 형식으로 대입.
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
    public void UpdateSpeedText(float speed)
    {
        speedText.text = $"x{speed.ToString("#.0")}";
    }
    public void UpdateWaveControl(float readyTime)
    {
        waveControlPanel.SetActive(readyTime > 0.0f);   // 대기 시간이 있다면 컨트롤 패널을 켠다.

        // 우리는 초 단위로 출력하려고 한다.
        // 컴퓨터는 1초가 남았을때 0.N초를 값으로 가지기 때문에 올림한다.
        // Ceil:올림, Round:반올림, Floor:내림.
        int time = Mathf.CeilToInt(readyTime);
        waveControlText.text = $"다음 웨이브까지 {time}초";
    }

    public void OnShowResult(bool isGameClear)
    {
        resultPanel.SetActive(true);
        resultText.text = isGameClear ? "GAME CLEAR" : "GAME OVER";
    }

}
