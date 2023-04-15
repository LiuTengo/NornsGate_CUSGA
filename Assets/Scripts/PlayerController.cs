using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    private CharacterController chaCon;
    private Vector3 moveDirection = Vector3.zero;

    const float gravity = 13.5f;
    public float moveSpeed, jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        chaCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {

        if (chaCon.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpForce;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        chaCon.Move(moveDirection * Time.deltaTime);
    }
}
