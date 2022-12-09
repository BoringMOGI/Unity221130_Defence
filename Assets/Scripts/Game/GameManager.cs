using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static float GameSpeed { get; private set; }
    public static float DeltaTime => Time.deltaTime * GameSpeed;

    private enum STATE
    {
        Ready,      // 준비 상태 : 유저가 정비를 하는 시간.
        Wave,       // 전투 상태 : 적이 생성되고 타워가 공격하는 시간.
        Result,     // 결과 상태 : 게임이 클리어,오버되어 끝난 시간.
    }

    [SerializeField] STATE state;           // GM의 상태.
    [SerializeField] int gold;              // 소지 골드.
    [SerializeField] int hp;                // 현재 체력.
    [SerializeField] int wave;              // 현재 웨이브.
    [SerializeField] int maxWave;           // 최대 웨이브.
    [SerializeField] float waveReadyTime;   // 웨이브 대기 시간.

    float readyTime;    // 대기 시간.

    private void Start()
    {
        GameSpeed = 1.0f;

        GameUI.Instance.UpdateGoldText(gold);
        GameUI.Instance.UpdateHpText(hp);
        GameUI.Instance.UpdateWaveText(wave, maxWave);
        GameUI.Instance.UpdateSpeedText(GameSpeed);

        readyTime = waveReadyTime;      // 대기 시간의 초기 값을 대입.
    }
    private void Update()
    {
        // 상태 머신 : 특정한 상태에 따라 분기 후 각각 다른 처리를 할 수 있다.
        switch (state)
        {
            case STATE.Ready:
                UpdateReady();
                break;
            case STATE.Wave:
                UpdateWave();
                break;
            case STATE.Result:
                UpdateResult();
                break;
        }
    }

    private void UpdateReady()
    {
        readyTime = Mathf.Clamp(readyTime - GameManager.DeltaTime, 0.0f, int.MaxValue);
        GameUI.Instance.UpdateWaveControl(readyTime);

        if(readyTime <= 0.0f)
        {
            EnemySpanwer.Instance.StartSpawn();     // 적 생성 시작.
            readyTime = waveReadyTime;              // 대기 시간 초기화.
            state = STATE.Wave;                     // 상태 변경.
        }
    }
    private void UpdateWave()
    {
        // 웨이브가 끝이 났는지 판단.
    }
    private void UpdateResult()
    {

    }
    

    // 유저가 시작 버튼을 눌렀다.
    public void OnPlayButton()
    {
        readyTime = 0.0f;
    }
    public void OnEndWave()
    {
        // 웨이브가 끝났기 때문에 다음 웨이브를 진행할지 판단한다.
        if(wave >= maxWave)
        {
            // 더 이상 진행할 웨이브가 없기 때문에 게임 클리어.
            GameClear();
            state = STATE.Result;   // 상태를 결과로 변경.
        }
        else
        {
            wave += 1;              // 웨이브 1 증가.
            GameUI.Instance.UpdateWaveText(wave, maxWave);

            state = STATE.Ready;    // 상태를 준비 단계로 변경.
        }
    }
    public void OnSpeedUp()
    {
        // 게임 속도를 1.0추가 시키는데 그 값이 최대 속도인 3보다 높아졌다면 1로 초기화.
        GameSpeed += 1.0f;
        if (GameSpeed > 3.0f)
            GameSpeed = 0.0f;

        //Time.timeScale = GameSpeed;                   // 게임의 시간 배율(기본:1.0)
        GameUI.Instance.UpdateSpeedText(GameSpeed);     // GameUI에게 시간 텍스트를 업데이트 시킨다.
    }

    public void AddGold(int amount)
    {
        gold += amount;
        GameUI.Instance.UpdateGoldText(gold);
    }
    public bool UseGold(int amount)
    {
        // 골드가 요구치보다 부족하면 false반환.
        if (gold < amount)
            return false;

        // 골드가 요구치만큼 충분하면 true반환.
        gold -= amount;
        GameUI.Instance.UpdateGoldText(gold);
        return true;
    }
    public bool EnoughGold(int amount)
    {
        // 소지 골드가 충분한가요?
        return gold >= amount;
    }

    public void OnGoalEnemy()
    {
        hp -= 1;
        GameUI.Instance.UpdateHpText(hp);
        if (hp <= 0)
            GameOver();
    }

    private void GameClear()
    {
        GameUI.Instance.OnShowResult(true);
    }
    private void GameOver()
    {
        GameUI.Instance.OnShowResult(false);
    }
}
