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
            //FIXME:�����ں�����������ʧ��Ч��
            Destroy(gameObject); 
            Destroy(_keyObj);
        }

    }

    bool isOnPosition()
    {
        Plane doorPlane = new Plane(_camNormal, transform.position);
        Vector3 keyPos = doorPlane.ClosestPointOnPlane(_keyObj.transform.position);

        //Notice��1.2��һ����β��Գ�����ֵ��
        //�������ù����Ͽ������������������޷���ȥ����Ľ����
        //FIXME��������֮��Ľ�������Ҫ������һ�б𷽷�����Ϊ�����λ�ò���������������Ŀ�������ϡ�
        if (Vector3.Distance(keyPos, transform.position) <= 1.2)
            return true;

        return false;
    } 

}
