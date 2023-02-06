using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float transitionDuration = 2.5f;
    public Transform[] target;

    

    private void Start()
    {
        
        StartCoroutine(Transition(UnityEngine.Random.Range(0, 5)));
       
    }


    IEnumerator Transition(int cameraNumber)
    {
        
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale/transitionDuration);


            transform.position = Vector3.Lerp(startingPos, target[cameraNumber].position, t);
            yield return 0;
        }

        
        StartCoroutine(Transition(UnityEngine.Random.Range(0, 5)));
       
        

    }
}
