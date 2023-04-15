using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWetherKeyisOnDoor : MonoBehaviour
{
    public GameObject _door;
    public GameObject _key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyisOnDoor())
        {
            Destroy(_door.transform.parent.gameObject);
            Destroy(_door);
            Destroy(_key);
        }
    }

    public bool KeyisOnDoor()
    {
        switch (CameraController.instance._camPos)
        {
            case 1:
                //TODO:查询斜上45度视图时各向量的关系并计算门和钥匙的距离
                break;
            case 2:

                Vector3 keyPos1 = new Vector3(_door.transform.position.x, _key.transform.position.y, _key.transform.position.z);
                Debug.Log(Vector3.Distance(keyPos1, _door.transform.position));
                if (Vector3.Distance(keyPos1, _door.transform.position) <= 0.2f)
                    return true;

                break;
            case 3:

                Vector3 keyPos2 = new Vector3(_key.transform.position.x, _key.transform.position.y, _door.transform.position.z);
                if (Vector3.Distance(keyPos2, _door.transform.position) <= 0.2f)
                    return true;

                break;
        }
        return false;
    }
}
