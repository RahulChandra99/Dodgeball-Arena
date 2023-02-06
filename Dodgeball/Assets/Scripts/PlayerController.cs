using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public FixedJoystick joystick;
    public Animator anim;
    
    public float moveSpeed;
    public float runSpeed;

    
    public Transform ballPosiPlayer;

    public bool hasBomb;
    public float throwForce;

    public bool isCollided;
    
    
    

    private void FixedUpdate()
    {
        if (GameManager.Instance.isTimerStarted)
        {
            if (hasBomb)
            {
                rb.velocity = new Vector3(-joystick.Horizontal * runSpeed * Time.fixedDeltaTime,rb.velocity.y,-joystick.Vertical * runSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = new Vector3(-joystick.Horizontal * moveSpeed * Time.fixedDeltaTime,rb.velocity.y,-joystick.Vertical * moveSpeed * Time.fixedDeltaTime);
            }
        
        
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            
                if (hasBomb)
                {
                    anim.SetBool("RunWithBomb",true);
                }
                else 
                {
                    anim.SetBool("Run",true);
                    anim.SetBool("RunWithBomb",false);
                }
            }
            else
            {
                if (hasBomb)
                {
                    Debug.Log("running bomb false");
                    anim.SetBool("RunWithBomb",false);
                    anim.SetBool("Run",false);
                }
                else
                {
                    anim.SetBool("Run",false);
                }
            }
        }
        
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball") )
        {
            Debug.Log("collided with " + this.name);
            
            // GameManager.Instance.ballonGround = false;
            
            GameManager.Instance.loserPlayer = this.gameObject;
            
            hasBomb = true;
            // isCollided = true;
            Destroy(GameManager.Instance.ballGO.GetComponent<Rigidbody>());
            GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = false;
            
            GameManager.Instance.ballGO.transform.SetParent(ballPosiPlayer.gameObject.transform);
            GameManager.Instance.ballGO.transform.position = ballPosiPlayer.transform.position;
            GameManager.Instance.ballGO.transform.rotation = ballPosiPlayer.transform.rotation;

            


        }
    }
    
    public void DiveDodge()
    {
        if (GameManager.Instance.isTimerStarted)
        {
            if (!hasBomb)
            {
                anim.Play("DiveIdle");
                moveSpeed = 80;
                Invoke("Run",1.3f);
            } 
        }
        
        
    }
    

    public void throwBall()
    {
        if (hasBomb)
        {
           
            anim.Play("Throw2");
            hasBomb = false;
            
            Invoke("RunAgain",0.4f);
        }
    }

    void Run()
    {
        anim.Play("Run");
        moveSpeed = 60;
    }

    void RunAgain()
    {

        GameManager.Instance.ballGO.GetComponent<TrailRenderer>().enabled = true;
        GameManager.Instance.ballGO.transform.SetParent(null);
        GameManager.Instance.ballGO.AddComponent<Rigidbody>();
        GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = true;
        GameManager.Instance.ballGO.GetComponent<Rigidbody>().collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            GameManager.Instance.ballGO.GetComponent<Rigidbody>().AddForce( rb.velocity * throwForce); 
        }
        else
        {
            GameManager.Instance.ballGO.GetComponent<Rigidbody>().AddForce( -ballPosiPlayer.transform.up * throwForce); 
        }

        StartCoroutine(ForThrow());
        
        anim.Play("Run");
        // GameManager.Instance.ballonGround = true;

    }

    IEnumerator ForThrow()
    {
        
        yield return new WaitForSeconds(1f);
        isCollided = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            throwBall();
            
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DiveDodge();
            
        }

        if (hasBomb)
        {
            GameManager.Instance.ballGO.GetComponent<TrailRenderer>().enabled = false;
            GameManager.Instance.throw_dodgeTXT.text = " THROW ";
        }
        else
        {
            GameManager.Instance.throw_dodgeTXT.text = " DODGE ";
        }
    }


    
}
