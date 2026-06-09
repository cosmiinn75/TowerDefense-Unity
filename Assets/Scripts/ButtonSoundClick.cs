using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundClick : MonoBehaviour
{
    public int levelIndex;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySoundClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }
    }
    void PlayLockedClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLockedClick();
        }
    }
    void PlaySound()
    {
        if (gameObject.CompareTag("isLevelButton")){
            int maxLevelReached = MainGameManager.Instance.maxLevelReached;
            
            if(levelIndex > maxLevelReached)
            {
                PlayLockedClick();
            }else
            {
                PlaySoundClick();
            }

        }else
        {
            PlaySoundClick();
        }


    }
}
