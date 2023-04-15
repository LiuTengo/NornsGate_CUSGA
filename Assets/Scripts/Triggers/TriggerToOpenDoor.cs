using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToOpenDoor : MonoBehaviour
{
    public GameObject Door;
    public GameObject obj;

    private void Start()
    {
        Door.SetActive(true);
        obj.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Door.SetActive(false);
        obj.SetActive(false);   
    }
}
