using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam3Follow : MonoBehaviour
{

    [SerializeField] public Transform _player;
    private Vector3 _camPosition;

    // Start is called before the first frame update
    void Start()
    {
        _camPosition = _player.position;
        _camPosition.z = transform.position.z;
        transform.position = _camPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _camPosition = _player.position;
        _camPosition.z = transform.position.z;
        transform.position = _camPosition;
    }
}
