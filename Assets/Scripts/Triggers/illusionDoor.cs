using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illusionDoor : MonoBehaviour
{

    public Transform _camPos, _player;
    public GameObject _keyObj;
    private Vector3 _camNormal; 

    // Update is called once per frame
    void Update()
    {

        _camNormal = Vector3.Normalize(_camPos.position - _player.position);

        Plane doorPlane = new Plane(_camNormal, transform.position);
        Vector3 keyPos = doorPlane.ClosestPointOnPlane(_keyObj.transform.position);


        if (isOnPosition() )//&& CameraController.instance._camPos ==1)
        {
            //FIXME:建议在后期做出逐渐消失的效果
            Destroy(gameObject); 
            Destroy(_keyObj);
        }

    }

    bool isOnPosition()
    {
        Plane doorPlane = new Plane(_camNormal, transform.position);
        Vector3 keyPos = doorPlane.ClosestPointOnPlane(_keyObj.transform.position);

        //Notice：1.2是一个多次测试出的数值。
        //不能设置过于严苛的条件，否则会造成无法消去方块的结果。
        //FIXME：可能在之后的进程中需要完善这一判别方法。因为相机的位置并非如此理想的落在目标坐标上。
        if (Vector3.Distance(keyPos, transform.position) <= 1.2)
            return true;

        return false;
    } 

}
