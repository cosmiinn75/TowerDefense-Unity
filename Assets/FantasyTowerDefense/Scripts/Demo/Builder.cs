using System.Collections.Generic;
using UnityEngine;

namespace Assets.FantasyTowerDefense.Scripts.Demo
{
    public class Builder : MonoBehaviour
    {
        public List<GameObject> TowerPrefabs;
        public Transform TowersNode;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var tower = Instantiate(TowerPrefabs[Random.Range(0, TowerPrefabs.Count)], TowersNode);

                tower.transform.position = Cursor.Instance.GetPosition();
            }
        }
    }
}