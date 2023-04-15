using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToTranslateObject : MonoBehaviour
{

    public Transform _targetPos1;
    public int enableCamPos;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && CameraController.instance._camPos == enableCamPos)
            other.transform.position = new Vector3(_targetPos1.position.x, other.transform.position.y, other.transform.position.z);

    }
}
