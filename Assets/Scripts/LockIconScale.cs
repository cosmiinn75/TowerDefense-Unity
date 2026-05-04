using UnityEngine;

namespace Assets.FantasyTowerDefense.Scripts.Demo
{
    public class LockIconScale : MonoBehaviour
    {
        private void LateUpdate()
        {
           
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); 
            transform.localScale = scale;
        }
    }
}