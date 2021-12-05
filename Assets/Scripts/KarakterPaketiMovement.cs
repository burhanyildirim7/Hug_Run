using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Update()
    {
        if (GameManager.instance.isContinue)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }      
    }
}
