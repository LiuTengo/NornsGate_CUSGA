using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToSetCubeActive : MonoBehaviour
{

    public GameObject eraseableCube,invisibleCube;
    // Start is called before the first frame update
    void Start()
    {
        eraseableCube.SetActive(false);
        invisibleCube.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        eraseableCube.SetActive(true);
        invisibleCube.SetActive(false);
    }

}
