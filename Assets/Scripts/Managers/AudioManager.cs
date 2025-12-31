using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource clickButtonStart;
    [SerializeField] private AudioSource clickButtonTutorial;
    [SerializeField] private AudioSource selectButton;
    [SerializeField] private AudioSource openCloseScroll;
    [SerializeField] private AudioSource selectButtonTutorial;

    [SerializeField] private GameObject firstButton;
    private string _selectedButton;
    
    public void PlayClickButtonTutorial()
    {
        clickButtonTutorial.Play();
    }
    
    public void PlayClickButtonStart()
    {
        clickButtonStart.Play();
    }
    
    public void PlayOpenCloseScroll()
    {
        openCloseScroll.Play();
    }

    void Start()
    {
        _selectedButton = firstButton.name;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name != _selectedButton && EventSystem.current.currentSelectedGameObject.name.StartsWith("@"))
            {
                selectButton.Play();
                _selectedButton = EventSystem.current.currentSelectedGameObject.name;
            }
        }
        
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name != _selectedButton && EventSystem.current.currentSelectedGameObject.name.StartsWith("#"))
            {
                selectButtonTutorial.Play();
                _selectedButton = EventSystem.current.currentSelectedGameObject.name;
            }
        }
    }
}
