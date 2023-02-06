using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRolling : MonoBehaviour
{

    public float moveForce;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.right * moveForce);
        }
    }
}
