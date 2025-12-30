using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//REQUERIMIENTOS
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerBeatInput))]
[RequireComponent(typeof(CharacterBeatView))]
public class PlayerBeatControler : CharacterBeatController, IHittableGameObjectByEnemy
{
    //DECLARACIÓN DE VARIABLES
    private enum CharacterState { IDLE, WALK, JUMP, ATTACK, FALL, HURT, DIE }
    private CharacterState _characterState;
    
    private CharacterBeatView _characterBeatView;
    private Rigidbody2D _rb2D;


    private Vector2 _movementVector;
    private Vector3 _velocity;
    private float _floorLvl;
    private Vector3 _jumpVelocity;
    private bool _isJumping;
    private bool isAttacked = false;

    [SerializeField] private float maxSpeedX;
    [SerializeField] private float maxSpeedY;
    //[SerializeField] private float jumpHeight;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxAirAcceleration;
    
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _characterBeatView = GetComponent<CharacterBeatView>();
        _floorLvl = float.MinValue;
        _rb2D.gravityScale = 0;
        _characterState =  CharacterState.IDLE;
    }

    void Update()
    {
        if (_characterState == CharacterState.FALL)
        {
            if (transform.position.y <= _floorLvl)
            {
                SetInGround();
            }
        }else if (_characterState == CharacterState.JUMP)
        {
            if (_rb2D.velocity.y < 0)
            {
                _characterState = CharacterState.FALL;
            }
        }
    }
    
    void FixedUpdate()
    {
        if (_characterState == CharacterState.IDLE || _characterState == CharacterState.WALK || _characterState == CharacterState.JUMP)
        {
            if ((bottomAnchor.position.x < leftLimit.position.x && _movementVector.x < 0) || (bottomAnchor.position.x > rightLimit.position.x && _movementVector.x > 0))
            {
                _movementVector.x = 0;
            }
            
            if ((bottomAnchor.position.y > topLimit.position.y && _movementVector.y > 0) || (bottomAnchor.position.y < bottomLimit.position.y && _movementVector.y < 0))
            {
                _movementVector.y = 0;
            }
            
            _velocity =  _rb2D.velocity;
            
            float acceleration = (_characterState !=  CharacterState.JUMP) ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;
            
            _velocity.x = Mathf.MoveTowards(_velocity.x, _movementVector.x, maxSpeedChange);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _movementVector.y, maxSpeedChange);

            if (_isJumping)
            {
                _isJumping =  false;
                Jump();
            }
            
            _rb2D.velocity = _velocity;
        }
        
    }

    public void MoveAction(Vector2 movementVector)
    {
        _movementVector =  Vector2.ClampMagnitude(movementVector, 1f);
        _movementVector.x *= maxSpeedX;
        _movementVector.y *= maxSpeedY;
        
        Debug.Log(_movementVector + "__" + Vector2.zero + "__" + _characterState);

        if (_characterState == CharacterState.IDLE || _characterState == CharacterState.WALK)
        {
            if (movementVector.x < 0 && transform.rotation.y != -180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -180, transform.rotation.z);
            }
            else if (movementVector.x > 0 && transform.rotation.y != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            }
            
            //--------------ANIMACIONES--------------//

            //if (_characterState != CharacterState.IDLE)
            if (movementVector == Vector2.zero && _characterState != CharacterState.IDLE)
            {
                _characterState = CharacterState.IDLE;
                _characterBeatView.ChangeAnimatorState("moving", 0);
            }
            else if (movementVector != Vector2.zero && _characterState != CharacterState.WALK)
            {
                _characterState = CharacterState.WALK;
                _characterBeatView.ChangeAnimatorState("moving", 1);
            }
        }
    }

    public void AttackAction()
    {
        if (_characterState == CharacterState.IDLE || _characterState == CharacterState.WALK)
        {
            _characterState = CharacterState.ATTACK;
            _rb2D.velocity = Vector2.zero;
            _characterBeatView.ChangeAnimatorState("attack", 1);

            StartCoroutine(FinishAttackAnimationState());
        }
    }

    [SerializeField] private float timeFinishAttackAnimation;
    private IEnumerator FinishAttackAnimationState()
    {
        yield return new WaitForSeconds(timeFinishAttackAnimation);

        
    }
    
    public void TestAnimationEvent()
    {
        Debug.Log("TestAnimationEvent");
        Collider2D[] objectsHit = Physics2D.OverlapBoxAll(hitAnchor.position, hitSize, 0);

        foreach (var objectHit in objectsHit)
        {
            if (objectHit.GetComponent<IHittableGameObjectByPlayer>() != null)
            {
                objectHit.GetComponent<IHittableGameObjectByPlayer>().HitByPlayer(damagePerHit, this);
            }
        }
        
        _characterState =  CharacterState.IDLE;
        _characterBeatView.ChangeAnimatorState("attack", 0);
        _characterBeatView.ChangeAnimatorState("moving", 0);
    }

    private void Jump()
    {
        _characterState = CharacterState.JUMP;
        _characterBeatView.ChangeAnimatorState("jump", 1);
        _rb2D.gravityScale = 1;
        _velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight); 
        _floorLvl = transform.position.y - 0.00001f;
    }
    
    private void SetInGround()
    {
        _characterState = CharacterState.IDLE;
        _characterBeatView.ChangeAnimatorState("moving", 0);
        _characterBeatView.ChangeAnimatorState("jump", 0);
        _rb2D.gravityScale = 0;
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
        transform.position = new Vector2(transform.position.x, _floorLvl);
        _floorLvl = float.MinValue;
    }

    public void JumpAction()
    {
        if (_characterState == CharacterState.IDLE || _characterState == CharacterState.WALK)
        {
            _isJumping = true;
        }
    }

    public void HitByEnemy(float damage, CharacterBeatController player)
    {
        if (_characterState == CharacterState.ATTACK)
        {
            //COSA DE PARRY MUY A LO MEJOR
            return;
        }

        if (!isAttacked)
        {
            isAttacked = true;
            currentHealth -= (int)damage;
            HealthUpdate();

            if (currentHealth <= 0)
            {
                //ESTO NECESITA AJUSTES CON LAS ANIMACIONES DE MUERTE PARA CUANDO TENGAMOS LAS FINALES
                _characterBeatView.ChangeAnimatorState("hurt", 2);
                _characterState = CharacterState.DIE;
                _rb2D.velocity = Vector2.zero;
                GameManager.Instance.GameOver();
                StopAllCoroutines();
            }
            else
            {
                _characterBeatView.ChangeAnimatorState("hurt", 1);
                _characterState = CharacterState.HURT;
                _rb2D.velocity = Vector2.zero;
            }
        }
        
    }
    
    private void BecomeHitteableAgain()
    {
        //PLAY SOUND
        isAttacked = false;
        _characterState = CharacterState.IDLE;
        _characterBeatView.ChangeAnimatorState("hurt", 0);
    }

    private void FinishAnimations()
    {
        _characterBeatView.animator.speed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ITriggerObject>() != null)
        {
            other.GetComponent<ITriggerObject>().TriggerByPlayer(this);
        }
    }
}
