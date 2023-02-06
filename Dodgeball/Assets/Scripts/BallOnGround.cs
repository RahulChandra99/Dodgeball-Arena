using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOnGround : MonoBehaviour
{
    public float timer;
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            Debug.Log("Enter plane");
            timer = 0;
            
            StartCoroutine("TimerCounting");
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            
        }
    }

    IEnumerator TimerCounting()
    {
        if (timer < 8f)
        {
            timer += 1;
            yield return new WaitForSeconds(1f);
            StartCoroutine("TimerCounting");
        }
        else
        {
            Destroy(GameManager.Instance.ballGO.GetComponent<Rigidbody>());
            GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = true;

            if (GameManager.Instance.loserPlayer.name == "Player_1")
            {
                GameManager.Instance.loserPlayer.GetComponent<PlayerController>().hasBomb = true;
            }
            else 
            {
                GameManager.Instance.loserPlayer.GetComponent<EightDirectionMovement>().hasBomb = true;
                GameManager.Instance.loserPlayer.GetComponent<EightDirectionMovement>().throwAI();
            }
            
            
            GameManager.Instance.ballGO.transform.SetParent(GameManager.Instance.loserPlayer.transform.GetChild(0).gameObject.transform);
            GameManager.Instance.ballGO.transform.position = GameManager.Instance.loserPlayer.transform.GetChild(0).transform.position;
            GameManager.Instance.ballGO.transform.rotation = GameManager.Instance.loserPlayer.transform.GetChild(0).transform.rotation;
            
            
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            Debug.Log("exit plane");
            timer = 0;
            
        }
    }
}
