using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    
    [SerializeField] private Transform leftUpLimit;
    [SerializeField] private Transform rightBottomLimit;
    
    private Vector3 _targetPos;

    void Start()
    {
        _targetPos = transform.position;
    }

    void Update()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position + offset;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            float interpVelocity = targetDirection.magnitude * speed;
            _targetPos = (transform.position) + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            Vector3 cameraPosition = Vector3.Lerp(transform.position, _targetPos, 0.25f);
            
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, leftUpLimit.position.x, rightBottomLimit.position.x);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, rightBottomLimit.position.y, leftUpLimit.position.y);
            
            transform.position = cameraPosition;
        }
    }
}
