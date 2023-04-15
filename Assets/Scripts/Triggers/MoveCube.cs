using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveCube : MonoBehaviour
{

    private Vector3 _Pos1, _Pos2, targetPos;
    private float time1, time2;

    [Header("Basic Settings")]
    public float _moveStep;
    public float _characterMoveSpeed;
    public float _lerpSpeed;
    public Transform _target1, _target2;


    // Start is called before the first frame update
    void Start()
    {
        _Pos1 = _target1.position;
        _Pos2 = _target2.position;
        targetPos = _Pos1;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCubes();
    }

    void MoveCubes() 
    {
        //确定平台移动方向 
        if (transform.position ==_Pos1)
        {
            //time1 =0;
            //time1 += Time.deltaTime;
            // if(time1 >= 1.5f)
                targetPos = _Pos2;
        }
        else if (transform.position == _Pos2)
        {
            //time2 = 0;
            //time2 += Time.deltaTime;
            //if(time2 >= 1.5f)
                targetPos = _Pos1;
        }
        //移动平台
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveStep * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, _characterMoveSpeed * Time.deltaTime);
            other.gameObject.GetComponent<CharacterController>().enabled = true;
        }
           
    }


}
