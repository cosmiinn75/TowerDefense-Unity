
using System.Collections.Generic;
using UnityEngine;


public class FollowPath : MonoBehaviour
{
    private int currentIndex = 0; // Current Point
    public List<Transform> pathwayPoints; //Pathway
    private EnemyStats stats; //Speed
    private void Start()
    {
        stats = GetComponent<EnemyStats>();
    }
    private void Update()
    {
        // If he reaches the final point
        if (currentIndex >= pathwayPoints.Count) {
            if (stats != null)
            {
                stats.reachedEnd = true;
                Destroy(gameObject);
                //Game Over         
                return;
            }
        }

   

            transform.position = Vector3.MoveTowards(transform.position, pathwayPoints[currentIndex].position, stats.currentSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathwayPoints[currentIndex].position) < 0.1f)
            {
                currentIndex++; // When he reaches the next checkpoint make it go towards the next
            }
        
       
       
    }
}
