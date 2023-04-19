using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public Transform _target;
    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = _target.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
