using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private enum STATE
    {
        Ready,      // �غ� ���� : ������ ���� �ϴ� �ð�.
        Wave,       // ���� ���� : ���� �����ǰ� Ÿ���� �����ϴ� �ð�.
        Result,     // ��� ���� : ������ Ŭ����,�����Ǿ� ���� �ð�.
    }

    [SerializeField] STATE state;           // GM�� ����.
    [SerializeField] int gold;              // ���� ���.
    [SerializeField] int hp;                // ���� ü��.
    [SerializeField] int wave;              // ���� ���̺�.
    [SerializeField] int maxWave;           // �ִ� ���̺�.
    [SerializeField] float waveReadyTime;   // ���̺� ��� �ð�.

    float readyTime;    // ��� �ð�.

    private void Start()
    {
        GameUI.Instance.UpdateGoldText(gold);
        GameUI.Instance.UpdateHpText(hp);
        GameUI.Instance.UpdateWaveText(wave, maxWave);

        readyTime = waveReadyTime;      // ��� �ð��� �ʱ� ���� ����.
    }
    private void Update()
    {
        // ���� �ӽ� : Ư���� ���¿� ���� �б� �� ���� �ٸ� ó���� �� �� �ִ�.
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
        readyTime = Mathf.Clamp(readyTime - Time.deltaTime, 0.0f, int.MaxValue);
        GameUI.Instance.UpdateWaveControl(readyTime);

        if(readyTime <= 0.0f)
        {
            EnemySpanwer.Instance.StartSpawn();     // �� ���� ����.
            readyTime = waveReadyTime;              // ��� �ð� �ʱ�ȭ.
            state = STATE.Wave;                     // ���� ����.
        }
    }
    private void UpdateWave()
    {
        // ���̺갡 ���� ������ �Ǵ�.
    }
    private void UpdateResult()
    {

    }
    

    // ������ ���� ��ư�� ������.
    public void OnPlayButton()
    {
        readyTime = 0.0f;
    }
    public void OnEndWave()
    {
        // ���̺갡 ������ ������ ���� ���̺긦 �������� �Ǵ��Ѵ�.
        if(wave >= maxWave)
        {
            // �� �̻� ������ ���̺갡 ���� ������ ���� Ŭ����.
            GameClear();
            state = STATE.Result;   // ���¸� ����� ����.
        }
        else
        {
            wave += 1;              // ���̺� 1 ����.
            GameUI.Instance.UpdateWaveText(wave, maxWave);

            state = STATE.Ready;    // ���¸� �غ� �ܰ�� ����.
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        GameUI.Instance.UpdateGoldText(gold);
    }
    public bool UseGold(int amount)
    {
        // ��尡 �䱸ġ���� �����ϸ� false��ȯ.
        if (gold < amount)
            return false;

        // ��尡 �䱸ġ��ŭ ����ϸ� true��ȯ.
        gold -= amount;
        GameUI.Instance.UpdateGoldText(gold);
        return true;
    }
    public bool EnoughGold(int amount)
    {
        // ���� ��尡 ����Ѱ���?
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
        Debug.Log("���� Ŭ����!!!");
    }
    private void GameOver()
    {
        Debug.Log("���� ����...");
    }
}
