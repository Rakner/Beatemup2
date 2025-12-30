using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(zOrder))]
public class Item : MonoBehaviour, IHittableGameObjectByPlayer
{
    protected string itemName;
    
    [SerializeField] private bool expiresInmediately;
    
    [SerializeField]private float expireTimes;
    
    [SerializeField] private AudioSource audioSource;
    
    protected CharacterBeatController _player;

    private float _counterTime;
    private bool _enterActionDone = false; 
    
    
    public virtual void HitByPlayer(float damage, CharacterBeatController player)
    {
        _player = player;
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        _counterTime = 0;
        ExecuteAction();
        _enterActionDone = true;
    }
    
    public virtual void ExecuteAction()
    {
        if (expiresInmediately)
        {
            ExitAction();
        }
    }
    
    public virtual void ExitAction()
    {
        _enterActionDone = false;
    }

    private void Update()
    {
        if (_enterActionDone && !expiresInmediately && expireTimes > 0)
        {
            _counterTime += Time.deltaTime;

            if (_counterTime < expireTimes)
            {
                ExecuteAction();
            }
            else
            {
                ExitAction();
            }
        }
    }
}
