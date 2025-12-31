using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private VFXManager vfxManager;
    
    public Image playerFace;
    public Image playerHealth;
    public TextMeshProUGUI playerName;

    public Image enemyFace;
    public Image enemyHealth;
    public TextMeshProUGUI enemyName;

    public GameObject startMenu;
    private bool _startMenuActive = true;
    
    public GameObject tutorialMenu;
    public GameObject tutorialText;
    public GameObject tutorialText2;
    public Animator tutorialAnimator;
    
    public Button tutorialButton;
    public Button tutorialButtonMainMenu;
    
    public GameObject gameOver;

    public void EnableStartMenu(bool enable)
    {
        audioManager.PlayClickButtonStart();
        vfxManager.PlayVfxStartButton();
        _startMenuActive = enable;
        StartCoroutine(StartMenu());
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(0.7f);
        startMenu.SetActive(_startMenuActive);
    }
    
    public void EnableTutorial(bool enableStart)
    {
        audioManager.PlayClickButtonTutorial();
        vfxManager.PlayVfxTutorialButton();
        _startMenuActive = enableStart;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(0.7f);
        startMenu.SetActive(_startMenuActive);
        tutorialMenu.SetActive(true);
        StartCoroutine(ShowTutorialText());
    }

    IEnumerator ShowTutorialText()
    {
        audioManager.PlayOpenCloseScroll();
        yield return new WaitForSeconds(0.7f);
        tutorialText.SetActive(true);
        tutorialButton.Select();
    }

    public void ExitTutorial()
    {
        audioManager.PlayOpenCloseScroll();
        StartCoroutine(FinishExitTutorial());
    }
    
    IEnumerator FinishExitTutorial()
    {
        vfxManager.StopVfxTutorialButton();
        tutorialAnimator.Play("CloseScroll");
        tutorialText.SetActive(false);
        tutorialText2.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        tutorialButtonMainMenu.Select();
        tutorialMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void StartTutorial2()
    {
        audioManager.PlayOpenCloseScroll();
        StartCoroutine(ShowTutorial2());
    }
    
    IEnumerator ShowTutorial2()
    {
        tutorialAnimator.Play("CloseScroll");
        tutorialText.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        tutorialAnimator.Play("TutorialStart");
        tutorialButtonMainMenu.Select();
        
        
        yield return new WaitForSeconds(1f);
        tutorialText2.SetActive(true);
    }
    
    
    
    
    //IN GAME
    
    public void SetPlayerFace(Sprite face)
    {
        playerFace.sprite = face;
    }

    public void SetPlayerHealth(float healthNormalized)
    {
        playerHealth.fillAmount = healthNormalized;
    }

    public void SetPlayerName(string namePlayer)
    {
        playerName.text = namePlayer;
    }
    
    public void SetEnemyFace(Sprite face)
    {
        enemyFace.sprite = face;
    }

    public void SetEnemyHealth(float healthNormalized)
    {
        enemyHealth.fillAmount = healthNormalized;
    }

    public void SetEnemyName(string nameEnemy)
    {
        enemyName.text = nameEnemy;
    }

    public void SetEnableEnemyElements(bool enable)
    {
        enemyFace.enabled = enable;
        enemyHealth.enabled = enable;
        enemyName.enabled = enable;
    }

    public bool IsEnableEnemyElements()
    {
        return enemyFace.gameObject.activeSelf;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}
