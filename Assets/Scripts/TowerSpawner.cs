using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    // 에디터 상의 공간 (Space)
    // - Local : 상대적 좌표.
    // - World : 절대적 좌표.

    // World space   : 씬 상의 공간 (월드 그 자체 공간)
    // Screen space  : 화면 상의 공간 (카메라가 비추는 공간)

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때.
        // Input.GetMouseButton : 마우스 버튼을 누르고 있는 동안 계속
        // Input.GetMouseButtonUp : 마우스 버튼을 땠을 때.
        // Input.GetMouseButtonDown : 마우스 버튼을 눌렀을 때.
        // 매개변수 button > 0:왼쪽, 1:오른쪽, 2:휠, 3:추가버튼1, 4:추가버튼2
        if(Input.GetMouseButtonDown(0))
        {
            // 스크린 좌표 상 마우스 포지션의 위치를 월드 상 좌표로 변경시켜준다.
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;

            // point로부터 정면으로 레이를 발사해 충돌하는 물체를 hit에 담는다.
            if (Physics.Raycast(point, Vector3.forward, out hit))
            {
                TileWall tileWall = hit.collider.GetComponent<TileWall>();
                SpawnTower(tileWall);
            }
        }
    }

    private void SpawnTower(TileWall wall)
    {
        // 만약 wall이 없거나 또는 wall에 이미 타워가 있는 경우.
        if (wall == null || wall.isTower)
            return;

        // 게임매니저를 통해 현재 설치하려는 타워의 가격을 성공적으로 소비했는지 체크.
        // 만약 성공적으로 소비했다면 타워를 설치한다.
        if (GameManager.Instance.UseGold(towerPrefab.Price))
        {
            Tower tower = Instantiate(towerPrefab, transform);  // 타워를 나의 자식으로 생성한다.
            wall.SetTower(tower);                               // wall의 SetTower를 호출하면서 tower를 넘긴다.
        }
    }
}
