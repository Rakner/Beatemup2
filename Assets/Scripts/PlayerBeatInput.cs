using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatInput : MonoBehaviour
{
    //DECLARACIÓN DE VARIABLES
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode attackKey;
    
    
    private PlayerBeatControler _playerBeatControler;
    private Vector2 _movementVector;
    
    void Start()
    {
        _playerBeatControler = GetComponent<PlayerBeatControler>();
        _movementVector = new Vector2();
    }
    
    void Update()
    {
        _movementVector.x = Input.GetAxisRaw("Horizontal");
        _movementVector.y = Input.GetAxisRaw("Vertical");
        
        _playerBeatControler.MoveAction(_movementVector);

        if (Input.GetKeyDown(jumpKey))
        {
            _playerBeatControler.JumpAction();
        }
        
        if (Input.GetKeyDown(attackKey))
        {
            _playerBeatControler.AttackAction();
        }
    }
}
