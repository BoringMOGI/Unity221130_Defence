using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpawner : MonoBehaviour
{
    private Tower previewTower;     // ��ġ�Ǳ� �� ���� Ÿ��.

    // ������ ���� ���� (Space)
    // - Local : ����� ��ǥ.
    // - World : ������ ��ǥ.

    // World space   : �� ���� ���� (���� �� ��ü ����)
    // Screen space  : ȭ�� ���� ���� (ī�޶� ���ߴ� ����)

    // ���콺 ���� ��ư�� ������ ��.
    // Input.GetMouseButton : ���콺 ��ư�� ������ �ִ� ���� ���
    // Input.GetMouseButtonUp : ���콺 ��ư�� ���� ��.
    // Input.GetMouseButtonDown : ���콺 ��ư�� ������ ��.
    // �Ű����� button > 0:����, 1:������, 2:��, 3:�߰���ư1, 4:�߰���ư2

    public void OnCreateTower(Tower towerPrefab)
    {
        // Ÿ���� ��ġ ���̶�� �����Ѵ�.
        if (previewTower != null)
            return;

        // !(NOT) ������
        // EnoughGold�� �䱸ġ��ŭ ��尡 �ִ��� true, false�� ����Ѵ�.
        // ����ϴٸ� �� ���� �ݴ�� ������ if���� �������� ���ϰ� �ϰ�
        // ������ϴٸ� ���ó� �ݴ�� ������ if���� �����ϰ� �����.
        if(!GameManager.Instance.EnoughGold(towerPrefab.Price))
        {
            Debug.Log("���� ��尡 �����մϴ�!!");
            return;
        }

        // �ӽ� Ÿ���� �����Ѵ�.
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
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // ����� ���콺 ��ġ.
        RaycastHit hit;
        TileWall tileWall = null;

        // point�κ��� �������� ���̸� �߻��� �浹�ϴ� ��ü�� hit�� ��´�.
        // hit�� ��ü�κ��� TileWall�� �˻��Ѵ�.
        if (Physics.Raycast(point, Vector3.forward, out hit))
            tileWall = hit.collider.GetComponent<TileWall>();

        // ���콺 ��ġ�� Ÿ���� �����ϰ� Ÿ�Ͽ� Ÿ���� ��ġ�Ǿ����� ���� ���.
        if (tileWall != null && !tileWall.isTower)
        {
            previewTower.transform.position = tileWall.transform.position;

            if (Input.GetMouseButtonUp(0))                               // ���콺 ���� ���� (= Ÿ�� ��ġ)
            {
                GameManager.Instance.UseGold(previewTower.Price);       // Ÿ���� ���ݸ�ŭ ��带 �Һ��Ѵ�.
                tileWall.SetTower(previewTower);                        // Ÿ�� Cell�� Ÿ���� ��ġ�Ѵ�.
                previewTower.Activate();                                // Ÿ���� Ȱ��ȭ��Ų��.
                previewTower = null;                                    // �ӽ� Ÿ���� null�� ����� Update�� �����Ų��.
                return;
            }
        }
        // ���콺 ��ġ�� Ÿ���� ���ų� Ÿ�Ͽ� �̹� Ÿ���� ��ġ�� ���.
        else
        {
            point.z = 0f;
            previewTower.transform.position = point;
        }

        // Ÿ�� ��ġ ���.
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(previewTower.gameObject);       // �ν��Ͻ��� �ӽ� Ÿ���� �����Ѵ�.
            previewTower = null;                    // ���� ���� null�� ����� Update�� �����Ų��.
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
