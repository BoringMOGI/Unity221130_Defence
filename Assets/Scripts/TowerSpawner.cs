using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    // ������ ���� ���� (Space)
    // - Local : ����� ��ǥ.
    // - World : ������ ��ǥ.

    // World space   : �� ���� ���� (���� �� ��ü ����)
    // Screen space  : ȭ�� ���� ���� (ī�޶� ���ߴ� ����)

    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ��ư�� ������ ��.
        if(Input.GetMouseButtonDown(0))
        {
            // ��ũ�� ��ǥ �� ���콺 �������� ��ġ�� ���� �� ��ǥ�� ��������ش�.
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;

            // point�κ��� �������� ���̸� �߻��� �浹�ϴ� ��ü�� hit�� ��´�.
            if (Physics.Raycast(point, Vector3.forward, out hit))
            {
                TileWall tileWall = hit.collider.GetComponent<TileWall>();
                SpawnTower(tileWall);
            }
        }
    }

    private void SpawnTower(TileWall wall)
    {
        // ���� wall�� ���ų� �Ǵ� wall�� �̹� Ÿ���� �ִ� ���.
        if (wall == null || wall.isTower)
            return;

        Tower tower = Instantiate(towerPrefab, transform);  // Ÿ���� ���� �ڽ����� �����Ѵ�.
        wall.SetTower(tower);                               // wall�� SetTower�� ȣ���ϸ鼭 tower�� �ѱ��.
    }
}
