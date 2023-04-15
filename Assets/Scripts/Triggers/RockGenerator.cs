using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    private float time;

    public Rigidbody rock;
    public float force;
    public float generatePace;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= generatePace)
        {
            GenerateRock();
            time = 0;
        }
    }


    void GenerateRock()
    {
        Rigidbody rockRb;
        rockRb = Instantiate(rock,transform.position,Quaternion.identity) as Rigidbody;
        rockRb.AddForce(Vector3.right * force);

    }
}
