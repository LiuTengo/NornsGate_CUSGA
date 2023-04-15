using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{

    public GameObject _targetCube;
    public Transform player;
    public float rotateSpeed;
    public Vector3 targetRotation;


    private bool _isRotate;

    // Start is called before the first frame update
    void Start()
    {
        _isRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 3f) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _isRotate= true;
               
            }
        }
        ToRotateCube();

    }

    void ToRotateCube()
    {
        if(_isRotate)
            _targetCube.transform.rotation = Quaternion.Lerp(_targetCube.transform.rotation, Quaternion.Euler(targetRotation),rotateSpeed * Time.deltaTime);

    }

}
