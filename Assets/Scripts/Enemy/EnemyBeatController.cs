using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CharacterBeatView))]
[RequireComponent(typeof(AudioSource))]

public class EnemyBeatController : CharacterBeatController, IHittableGameObjectByPlayer
{
    private enum CharacterState { CHASE, ATTACK, HURT, WAIT_TO_ATTACK, DIE }

    [SerializeField] private float timeBeforeAttack;
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float maxSpeedX;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float maxSpeedChange;
    [SerializeField] private AudioSource sfxHit;
    
    private Rigidbody2D _rb2D;
    private CharacterBeatView _characterBeatView;
    private bool _isDead = false;
    private Vector2 _movementVector;
    private Vector3 _velocity;
    private CharacterState _enemyState;
    private GameObject _target;
    private bool isAttacked = false;
    private float _counterWaitingAttack;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _characterBeatView = GetComponent<CharacterBeatView>();
        _enemyState = CharacterState.CHASE;
        _characterBeatView.ChangeAnimatorState("moving", 1);
        
        GameManager.Instance.AddEnemy(this);
        _target = GameManager.Instance.player;
    }

    void Update()
    {
        LookToPlayer();
        if (_enemyState == CharacterState.CHASE)
        {
            if (Vector3.Distance(_target.transform.position, transform.position) > distanceToAttack)
            {
                Debug.Log("ME MUEVO");
                Vector2 direction = _target.transform.position - transform.position;
                MoveAction(new Vector2(direction.normalized.x, direction.normalized.y));
            }
            else
            {
                MoveAction(new Vector2(0,0));
                _enemyState = CharacterState.ATTACK;
                _characterBeatView.ChangeAnimatorState("moving", 0);
                _characterBeatView.ChangeAnimatorState("attack", 1);
            }
        }
    }

    
    private void FixedUpdate()
    {
        if (_enemyState == CharacterState.CHASE)
        {
            if ((bottomAnchor.position.x < leftLimit.position.x && _movementVector.x < 0) || (bottomAnchor.position.x > rightLimit.position.x && _movementVector.x > 0))
            {
                Debug.Log("NO ME MUEVO");
                _movementVector.x = 0;
            }
            
            if ((bottomAnchor.position.y > leftLimit.position.y && _movementVector.y > 0) || (bottomAnchor.position.y < rightLimit.position.y && _movementVector.y < 0))
            {
                _movementVector.y = 0;
            }
            _velocity =  _rb2D.velocity;
            
        }
        else if (_enemyState == CharacterState.WAIT_TO_ATTACK)
        {
            _velocity =  _rb2D.velocity;
            _counterWaitingAttack += Time.deltaTime;

            if (_counterWaitingAttack < timeBeforeAttack * 0.5f)
            {
                _movementVector.x = -1;
            }
            else if (_counterWaitingAttack > timeBeforeAttack * 0.5f)
            {
                Debug.Log("RESET MOVEMENT VECTOR");
                _movementVector.x = 1;
            }
            else
            {
                //_movementVector.x = 1;
                _counterWaitingAttack = 0;
                MoveAction(new Vector2(0,0));
                _enemyState = CharacterState.CHASE;
                //_velocity = Vector3.zero;
            }
        }
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _movementVector.x, maxSpeedChange);
        _velocity.y = Mathf.MoveTowards(_velocity.y, _movementVector.y, maxSpeedChange);
        _rb2D.velocity = _velocity;
    }

    public void HitByPlayer(float damage, CharacterBeatController player)
    {
        currentHealth -= (int)damage;
        float normalizedHealth = (float)currentHealth / maxHealth;
        Debug.Log("VIDA ACTUAL" + currentHealth + "VIDA DE LA UI" + normalizedHealth + "___ DAÑO RECIBIDO" + damage);
        GameManager.Instance.uiController.SetEnableEnemyElements(true);
        GameManager.Instance.EnemyHitted(_characterBeatView.faceSprite, _characterBeatView.characterName, normalizedHealth);

        if (currentHealth <= 0)
        {
            _characterBeatView.ChangeAnimatorState("hurt", 2);
            gameObject.GetComponent<Collider2D>().enabled = false;
            _enemyState = CharacterState.DIE;
            _rb2D.velocity = Vector2.zero;
            _velocity =  Vector3.zero;
            StopAllCoroutines();
            
            GameManager.Instance.RemoveEnemies(this);
            GameManager.Instance.uiController.SetEnableEnemyElements(false);
            Destroy(this.gameObject, 2);
        }
        else
        {
            if (!isAttacked)
            {
                isAttacked = true;
                _characterBeatView.ChangeAnimatorState("hurt", 1);
                _enemyState = CharacterState.HURT;
                _rb2D.velocity = Vector2.zero;
            }
            
        }
    }

    private void FinishHurtAnimationState()
    {
        _characterBeatView.ChangeAnimatorState("hurt", 0);
        

        if (sfxHit)
        {
            sfxHit.Play();
        }
    }

    private void BecomeHitteableAgain()
    {
        isAttacked = false;
        _enemyState = CharacterState.CHASE;
    }

    private void MoveAction(Vector2 movementVector)
    {
        _movementVector = Vector2.ClampMagnitude(movementVector, 1f);
        _movementVector.x = movementVector.x * maxSpeedX;
        _movementVector.y = movementVector.y * maxSpeedY;
        LookToPlayer();
    }

    private void LookToPlayer()
    {
        if (_target.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    private void Attack()
    {
        if (!_isDead && _enemyState != CharacterState.HURT)
        {
            _rb2D.velocity = Vector2.zero;
            Collider2D[] objects = Physics2D.OverlapBoxAll(hitAnchor.position, hitSize, 0);

            foreach (Collider2D obj in objects)
            {
                if (obj.GetComponent<IHittableGameObjectByEnemy>() != null)
                {
                    obj.GetComponent<IHittableGameObjectByEnemy>().HitByEnemy(damagePerHit, this);
                }
            }
        }
        
        _enemyState =  CharacterState.WAIT_TO_ATTACK;
        _characterBeatView.ChangeAnimatorState("moving", 1);
        _characterBeatView.ChangeAnimatorState("attack", 0);
    }
    
}
