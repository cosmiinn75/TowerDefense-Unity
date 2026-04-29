using UnityEngine;

namespace Assets.FantasyTowerDefense.Scripts.Scene
{
    public class GridSnap : MonoBehaviour
    {
        public void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            var grid = GetComponentInParent<Grid>();

            foreach (Transform child in transform)
            {
                var cell = grid.WorldToCell(child.position);

                child.position = grid.GetCellCenterWorld(cell);
            }
        }
    }
}