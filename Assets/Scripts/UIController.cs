using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image playerFace;
    public Image playerHealth;
    public TextMeshProUGUI playerName;

    public Image enemyFace;
    public Image enemyHealth;
    public TextMeshProUGUI enemyName;

    public GameObject gameOver;

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
