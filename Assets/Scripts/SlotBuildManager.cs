using UnityEngine;

public class SlotBuildManager : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    public int selectedTower = 0;


    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTower];
    }
}
