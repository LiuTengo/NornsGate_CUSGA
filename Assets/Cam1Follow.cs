using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam1Follow : MonoBehaviour
{
    [SerializeField] public Transform _player;
    private Vector3 _camPositionLerp;

    // Start is called before the first frame update
    void Start()
    {
        _camPositionLerp = transform.position - _player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, _player.position + _camPositionLerp, 0.5f);
    }
}
