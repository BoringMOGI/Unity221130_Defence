using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpawner : MonoBehaviour
{
    private Tower previewTower;     // 설치되기 전 예비 타워.

    // 에디터 상의 공간 (Space)
    // - Local : 상대적 좌표.
    // - World : 절대적 좌표.

    // World space   : 씬 상의 공간 (월드 그 자체 공간)
    // Screen space  : 화면 상의 공간 (카메라가 비추는 공간)

    // 마우스 왼쪽 버튼을 눌렀을 때.
    // Input.GetMouseButton : 마우스 버튼을 누르고 있는 동안 계속
    // Input.GetMouseButtonUp : 마우스 버튼을 땠을 때.
    // Input.GetMouseButtonDown : 마우스 버튼을 눌렀을 때.
    // 매개변수 button > 0:왼쪽, 1:오른쪽, 2:휠, 3:추가버튼1, 4:추가버튼2

    public void OnCreateTower(Tower towerPrefab)
    {
        // 타워를 설치 중이라면 무시한다.
        if (previewTower != null)
            return;

        // !(NOT) 연산자
        // EnoughGold는 요구치만큼 골드가 있는지 true, false로 대답한다.
        // 충분하다면 그 값을 반대로 돌려서 if문을 실행하지 못하게 하고
        // 불충분하다면 역시나 반대로 돌려서 if문을 실행하게 만든다.
        if(!GameManager.Instance.EnoughGold(towerPrefab.Price))
        {
            Debug.Log("소지 골드가 부족합니다!!");
            return;
        }

        // 임시 타워를 생성한다.
        previewTower = Instantiate(towerPrefab, transform);
        TowerControlUI.Instance.Close();
    }   

    void Update()
    {
        if (previewTower != null)
            UpdateNewTower();
        else
            UpdateTowerUI();
    }

    private void UpdateNewTower()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // 월드상 마우스 위치.
        RaycastHit hit;
        TileWall tileWall = null;

        // point로부터 정면으로 레이를 발사해 충돌하는 물체를 hit에 담는다.
        // hit한 물체로부터 TileWall을 검색한다.
        if (Physics.Raycast(point, Vector3.forward, out hit))
            tileWall = hit.collider.GetComponent<TileWall>();

        // 마우스 위치에 타일이 존재하고 타일에 타워가 설치되어있지 않을 경우.
        if (tileWall != null && !tileWall.isTower)
        {
            previewTower.transform.position = tileWall.transform.position;

            if (Input.GetMouseButtonUp(0))                               // 마우스 왼쪽 해제 (= 타워 설치)
            {
                GameManager.Instance.UseGold(previewTower.Price);       // 타워의 가격만큼 골드를 소비한다.
                tileWall.SetTower(previewTower);                        // 타일 Cell에 타워를 설치한다.
                previewTower.Activate();                                // 타워를 활성화시킨다.
                previewTower = null;                                    // 임시 타워를 null로 만들어 Update를 종료시킨다.
                return;
            }
        }
        // 마우스 위치에 타일이 없거나 타일에 이미 타워가 설치된 경우.
        else
        {
            point.z = 0f;
            previewTower.transform.position = point;
        }

        // 타워 설치 취소.
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(previewTower.gameObject);       // 인스턴스된 임시 타워를 제거한다.
            previewTower = null;                    // 참조 값을 null로 만들어 Update를 종료시킨다.
        }
    }
    private void UpdateTowerUI()
    {
        if(Input.GetMouseButtonUp(0))
        {            
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(worldPosition, Vector3.forward, out hit))
            {
                TileWall tile = hit.collider.GetComponent<TileWall>();
                if (tile.isTower)
                    tile.OpenControlUI();
                else
                    TowerControlUI.Instance.Close();
            }
            else
            {
                TowerControlUI.Instance.Close();
            }
        }
    }

    private void OnGUI()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject; 
        string name = (current != null) ? current.name : "Empty";

        GUI.Label(new Rect(50, 50, 200, 50), $"Current Select : {name}", new GUIStyle() { fontSize = 50 });
    }

}
