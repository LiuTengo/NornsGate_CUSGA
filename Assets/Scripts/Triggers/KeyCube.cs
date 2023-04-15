using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCube : MonoBehaviour
{

    public GameObject _doorCube;

    private bool _isOpen;
    private Renderer _door;
    private float _disappearTime;
    // Start is called before the first frame update
    void Start()
    {
        //disappearTime = 0;
        _isOpen = false;
        //door = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isOpen = true;
        }

        if (_isOpen)
        {
            DoorOpen();
        }

    }

    /*private void OnCollisionStay(Collision collision)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
        }

    }*/

    void DoorOpen()
    {
        _doorCube.SetActive(false);

        /*if(disappearTime < 1)
        {
            disappearTime += 1;
        }
        if(door.material.color.a >= 0)
        {
            door.material.color = new Color(door.material.color.r,
                                            door.material.color.g,
                                            door.material.color.b,
                                            door.material.color.a - disappearTime/50);
        }
        */
    }
}
