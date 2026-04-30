using UnityEngine;

public class MenuFadeAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 0.2f;
    private Vector3 targetScale;
    private float elapsedTime = 0f;


    private bool isOpening = true;
    private bool isAnimating = false;

    private void Awake()
    {
     
        targetScale = transform.localScale;


        isAnimating = true;
        isOpening = true;
        elapsedTime = 0f;
        enabled = true;
    }

    private void Update()
    {
        if (!isAnimating) return;

        elapsedTime += Time.deltaTime;
        float percentage = Mathf.Clamp01(elapsedTime / animationDuration);

        if (isOpening)
        {
       
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, percentage);

            if (percentage >= 1f)
            {
                isAnimating = false;
                enabled = false;
            }
        }
        else
        {
            enabled = true;
            transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, percentage);

            if (percentage >= 1f)
            {
                isAnimating = false;
                enabled = false;
                Destroy(gameObject); 
            }
        }
    }


    public void CloseMenu()
    {
 
        elapsedTime = 0f;
        isOpening = false;
        isAnimating = true;
        enabled = true;
    }
}