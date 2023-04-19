using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnTurnAround : MonoBehaviour
{

    public GameObject _revolution;

    public Transform _revolutionPoint;
    public float RotateSpeed;

    public float _revlutionAngle = 0;
    bool _stayIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _stayIn)
        {
            _revlutionAngle += 90;
            _revlutionAngle %= 360;
        }
        Revolution();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        _stayIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        _stayIn = false;
    }

    void Revolution()
    {

        Quaternion targetRotation = Quaternion.Euler(0, _revlutionAngle, 0);
        float angleDiff = Quaternion.Angle(_revolution.transform.rotation, targetRotation);

        if (angleDiff > .1f)
        {
            Quaternion newRotation = Quaternion.RotateTowards(_revolution.transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
            _revolution.transform.RotateAround(_revolutionPoint.position, Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else
        {
            _revolution.transform.rotation = targetRotation;
        }



    }
}
