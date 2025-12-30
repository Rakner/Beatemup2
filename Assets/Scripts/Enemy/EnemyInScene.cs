using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInScene
{
    public GameObject enemyGameObject;
    public float timeToInitBehaviour = 0;
    public bool isHiddenAtStart = false;

    private float _currentTime;
    private EnemyGroup _myGroup;
    private bool _isStarted = false;

    public void SetMyGroup(EnemyGroup group)
    {
        _myGroup = group;

        if (isHiddenAtStart)
        {
            enemyGameObject.SetActive(false);
            enemyGameObject.GetComponent<EnemyBeatController>().enabled = false;
        }
    }

    public void AddTime(float timeDelta)
    {
        _currentTime += timeDelta;

        if (_currentTime >= timeToInitBehaviour && !_isStarted)
        {
            _isStarted = true;
            enemyGameObject.GetComponent<EnemyBeatController>().enabled = true;
            _myGroup.InitEnemy(enemyGameObject.GetComponent<EnemyBeatController>());
        }
    }
}
