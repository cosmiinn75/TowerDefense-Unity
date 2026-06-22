using UnityEngine;

public class UIButtonEffects : MonoBehaviour
{
    [Header("Transition settings")]
    public float animationDuration = 0.2f;
    private Vector3 targetScale;
    private float elapsedTime = 0f;

    private bool isOpening = true;
    private bool isAnimating = false;

    [Header("Pulse settings")]
    public bool enablePulse = true;    
    public float pulseSpeed = 3f;       
    public float scaleAmount = 0.12f;    

    private void Awake()
    {
        targetScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        elapsedTime = 0f;
        isOpening = true;
        isAnimating = true;
    }

    private void Update()
    {
      
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;
            float percentage = Mathf.Clamp01(elapsedTime / animationDuration);

            if (isOpening)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, percentage);

                if (percentage >= 1f)
                {
                    isAnimating = false;
                }
            }
            else
            {
                transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, percentage);

                if (percentage >= 1f)
                {
                    isAnimating = false;
                    gameObject.SetActive(false);
                }
            }
        }

        else if (enablePulse && isOpening)
        {
            float pulseFactor = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) / 2f;
            float currentScale = 1f + (pulseFactor * scaleAmount);

            transform.localScale = new Vector3(
                targetScale.x * currentScale,
                targetScale.y * currentScale,
                targetScale.z
            );
        }
    }

    public void CloseMenu()
    {
        elapsedTime = 0f;
        isOpening = false;
        isAnimating = true;
    }
}