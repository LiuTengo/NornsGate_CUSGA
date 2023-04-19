using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool _isMain = false;
    public bool _isFront = false;
    public bool _isLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isMain = true;
            _isFront = false;
            _isLeft = false;
        }
        else if (Input.GetKeyDown(KeyCode.O)) 
        { 
            _isMain = false;
            _isFront = true;
            _isLeft = false;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _isMain = false;
            _isFront = false;
            _isLeft = true;
        }
    }
}
