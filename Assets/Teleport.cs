using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public Vector3 destination;
    public GameObject anotherTeleport;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) {
            return;
        }
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            anotherTeleport.SetActive(false);
            other.transform.position += destination;

            cc.enabled = true;
            anotherTeleport.SetActive(true);
        }
        else
        {
            anotherTeleport.SetActive(false);
            other.transform.position += destination;
            anotherTeleport.SetActive(true);
        }
    }
}
