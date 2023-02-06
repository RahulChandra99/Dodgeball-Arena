using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EightDirectionMovement : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator anim;
    
    public float speed;
    public float walkRadius;

    public bool hasBomb;

    public bool isCollided;
    public Transform ballPosiPlayer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (GameManager.Instance.isTimerStarted)
        {
            if (agent != null)
            {
                agent.speed = speed;
                agent.SetDestination(RandomNavMeshLocation());
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.isTimerStarted)
        {
            if (!hasBomb)
            {
                agent.speed = speed;
                if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
                {
                    transform.LookAt(GameManager.Instance.ballGO.transform);
                
                    if (hasBomb)
                    {
                        agent.speed = 0.5f;
                        anim.SetBool("RunWithBomb",true);
                    }
                    else 
                    {
                        anim.SetBool("Run",true);
                        anim.SetBool("RunWithBomb",false);
                    }
                    agent.SetDestination(RandomNavMeshLocation());
                
                }
            }
            else
            {
                anim.SetBool("Run",false);
                
                transform.LookAt(GameManager.Instance.allPlayers[tempPlayerToLookAt]);

                agent.speed = 0;

            }
            
        }

    }

    

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;

        }

        return finalPosition;
    }

    private int tempPlayerToLookAt;
    
    private void OnCollisionEnter(Collision other)
    {
        if (GameManager.Instance.isTimerStarted)
        {
            if (other.gameObject.CompareTag("Ball") )
            {
                // GameManager.Instance.ballonGround = false;
                Debug.Log("collided with " + this.name);
            
                GameManager.Instance.loserPlayer = this.gameObject;
            
                hasBomb = true;
                // isCollided = true;
                Destroy(GameManager.Instance.ballGO.GetComponent<Rigidbody>());
                GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = true;
            
                GameManager.Instance.ballGO.transform.SetParent(ballPosiPlayer.gameObject.transform);
                GameManager.Instance.ballGO.transform.position = ballPosiPlayer.transform.position;
                GameManager.Instance.ballGO.transform.rotation = ballPosiPlayer.transform.rotation;

                throwAI();
            }
        }
        
    }

    public void throwAI()
    {
        tempPlayerToLookAt = UnityEngine.Random.Range(0, 5);
            
            
            
            
        Invoke("WaitandShoot",1.5f);
    }

    void WaitandShoot()
    {
        anim.Play("Throw2");
        
        
        Invoke("WaitAndRun",0.4f);
        // GameManager.Instance.ballonGround = true;


       
    }
    
    void WaitAndRun()
    {
        GameManager.Instance.ballGO.transform.SetParent(null);
        GameManager.Instance.ballGO.AddComponent<Rigidbody>();
        GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = true;
        GameManager.Instance.ballGO.GetComponent<Rigidbody>().collisionDetectionMode =
            CollisionDetectionMode.Continuous;
        GameManager.Instance.ballGO.GetComponent<Rigidbody>().AddForce(-ballPosiPlayer.transform.up * 250);

        hasBomb = false;

        
        anim.Play("Run");
        anim.SetBool("Run",true);
    }
}

// public float minTime = 2;
    // public float maxTime = 5f;
    // public GameObject Ground;
    // private NavMeshAgent nma = null;
    // private Bounds bounds;

    // private void Start()
    // {
    //     nma = this.GetComponent<NavMeshAgent>();
    //     bounds = Ground.GetComponent<Renderer>().bounds;
    //     
    // }
    //
    // private void Update()
    // {
    //     if (nma.hasPath == false || nma.remainingDistance < 1f)
    //     {
    //         float wait = UnityEngine.Random.Range(minTime, maxTime);
    //         Invoke("PickRandomDest",wait);
    //     }
    // }
    //
    // void PickRandomDest()
    // {
    //     float rx = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
    //     float rz = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
    //     Vector3 rpo
    //          = new Vector3(rx,this.transform.position.y,rz);
    //     nma.SetDestination(rpo);
    // }

    //     public float velocity = 5;
    //     public float turnSpeed = 10;
    //
    //     Vector2 input;
    //     float angle;
    //
    //     Quaternion targetRotation;
    //     public Transform cam; //Transform cam;
    //
    //     FollowTarget ft;
    //
    //     void Start()
    //     {
    //         //cam = Camera.main.transform;
    //         if (cam.GetComponent<FollowTarget>())
    //         {
    //             ft = cam.GetComponent<FollowTarget>();
    //         }
    //
    //     }
    //
    //     void Update()
    //     {
    //         GetInput();
    //
    //         if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;
    //
    //         CalculateDirection();
    //         Rotate();
    //         Move();
    //
    //     }
    //
    //     void GetInput()
    //     {
    //         input.x = Input.GetAxisRaw("Horizontal");
    //         input.y = Input.GetAxisRaw("Vertical");
    //     }
    //
    //     void CalculateDirection()
    //     {
    //         angle = Mathf.Atan2(input.x, input.y);
    //         angle = Mathf.Rad2Deg * angle;
    //         angle += cam.eulerAngles.y;
    //     }
    //
    //     void Rotate()
    //     {
    //         if (ft != null && ft.camRotation)
    //         {
    //             transform.rotation = Quaternion.Euler(0, input.x * 1.5f, 0) * transform.rotation;
    //         }
    //         else
    //         {
    //             targetRotation = Quaternion.Euler(0, angle, 0);
    //         }
    //
    //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    //     }
    //
    //     void Move()
    //     {
    //         transform.position += transform.forward * velocity * Time.deltaTime;
    //     }
    // }

    
    

