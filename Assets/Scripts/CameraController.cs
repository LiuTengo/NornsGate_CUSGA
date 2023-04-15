using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public int _sightNum; //视角数目
    public int _camPos; //相机位置

    public GameObject mainCamera;
    public GameObject rightCamera;
    public GameObject frontCamera;

    public GameObject _frontColider;
    public GameObject _rightColider;

    public bool isThree;

    //1——45度斜上视角
    //2——左视图
    //3——正视图
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        rightCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        frontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        _camPos = 1; //初始设置相机位置在1号位
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCameraPosition();
        CameraPosition();
    }

    void SwitchCameraPosition()
    {
        //按下Q键切换视角
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(_camPos < _sightNum)
            {
                _camPos += 1;
            }
            else if(_camPos == _sightNum)
            {
                _camPos = 1;
            }
        }
    }

    void CameraPosition()
    {

        switch(_camPos ) 
        {

            case 1:
                mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                rightCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                frontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                _frontColider.SetActive(false);
                _rightColider.SetActive(false);
                break;

            case 2:
                mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                rightCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                frontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                _frontColider.SetActive(true);
                _rightColider.SetActive(false);
                break;
            
            case 3:
                if (isThree) {
                    mainCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                    rightCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
                    frontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                    _rightColider.SetActive(true);
                    _frontColider.SetActive(false);
                }

                break;
            
        }


    }

}
