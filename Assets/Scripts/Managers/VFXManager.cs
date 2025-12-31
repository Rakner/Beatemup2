using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject vfxStartButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject selectedStartButtonAnimation;
    
    [SerializeField] private GameObject vfxTutorialButton;
    [SerializeField] private GameObject tutorialButton;
    [SerializeField] private GameObject selectedTutorialButtonAnimation;
    
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject selectedExitButtonAnimation;

    public void PlayVfxStartButton()
    {
        vfxStartButton.SetActive(true);
    }

    public void PlayVfxTutorialButton()
    {
        vfxTutorialButton.SetActive(true);
    }
    
    public void StopVfxTutorialButton()
    {
        vfxTutorialButton.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name == startButton.name)
            {
                selectedStartButtonAnimation.SetActive(true);
            }
            else
            {
                selectedStartButtonAnimation.SetActive(false);
            }
        
            if (EventSystem.current.currentSelectedGameObject.name == tutorialButton.name)
            {
                selectedTutorialButtonAnimation.SetActive(true);
            }
            else
            {
                selectedTutorialButtonAnimation.SetActive(false);
            }
        
            if (EventSystem.current.currentSelectedGameObject.name == exitButton.name)
            {
                selectedExitButtonAnimation.SetActive(true);
            }
            else
            {
                selectedExitButtonAnimation.SetActive(false);
            }
        }
    }
}
