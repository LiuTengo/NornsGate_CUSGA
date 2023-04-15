using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_2_0 : MonoBehaviour
{
    public bool IsVertical;
    public GameObject _revolution;
    public Transform _revolutionPoint;
    public float RotateSpeed;

    public GameObject _gameObject;
    public GameObject _gameObject2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsVertical) {
            Revolution();
        }
    }

    void Revolution() {

        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
        float angleDiff = Quaternion.Angle(_revolution.transform.rotation, targetRotation);

        if (angleDiff > .1f)
        {
            Quaternion newRotation = Quaternion.RotateTowards(_revolution.transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
            _revolution.transform.RotateAround(_revolutionPoint.position, -Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else {
            _revolution.transform.rotation = targetRotation;
        }

        
        
    }
}
