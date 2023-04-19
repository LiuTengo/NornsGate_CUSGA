using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Begin : MonoBehaviour
{
    public CinemachineVirtualCamera _cinemachineVirtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedCameraPriority(5f, 8));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayedCameraPriority(float delayTime, int priority)
    {
        yield return new WaitForSeconds(delayTime);
        _cinemachineVirtualCamera.Priority = priority;
    }
}
