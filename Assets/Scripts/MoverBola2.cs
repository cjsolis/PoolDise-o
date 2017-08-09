using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBola2 : MonoBehaviour {

    private Rigidbody rb;
    public int speed = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //bool moveHorizontal = Input.GetMouseButtonDown(0);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        /*if (moveHorizontal)
        {
            Vector3 movimiento = new Vector3(speed, 0, 0);
            rb.AddForce(movimiento);

        }*/

        Vector3 movimiento = new Vector3(moveHorizontal * speed, 0, moveVertical * speed);
        rb.AddForce(movimiento);
    }
}
