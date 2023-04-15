using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    //
    [Header("Basic Settings")]
    public bool isMain, isRight, isFront;
    [Header("Portal To Which Cube")]
    public Transform _targetCube;
    //public Transform _invisibleCube;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (isMain && CameraController.instance._camPos == 1)
            {
                Vector3 offset = _targetCube.position - transform.position;
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
                other.transform.position += offset;
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
            }
            else if (isRight && CameraController.instance._camPos == 2)
            {
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
                other.transform.position = new Vector3(_targetCube.transform.position.x, other.transform.position.y, other.transform.position.z);
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
            }
            else if (isFront && CameraController.instance._camPos == 3)
            {
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, _targetCube.transform.position.z);
                other.GetComponent<CharacterController>().enabled = !other.GetComponent<CharacterController>().enabled;
            }
        }
        
    }
}
