using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject GM = new GameObject("GM");
                GM.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }


    public GameObject ballGO;
    public GameObject ballExplosionEffect;
    
    //ScoreBoard Timer
    public TextMesh timerText;
    private float currentTime = 0;
    public float timer = 60;

    public bool isGameStarted;
    
    //Game Starting Coutdown Timer
    public bool isTimerStarted;
    public TextMeshProUGUI cdTimer;
    private float cTime = 0;
    private float cdtimer = 3;
    public GameObject startBtn;

    public TextMeshProUGUI throw_dodgeTXT;

    public bool enableCameraMovement;

    public float slowTime = 0.2f;
    public bool isSlowed;
    public bool canSlowNow;

    public GameObject loserPlayer;
    public TextMeshProUGUI playerWithThebomb;

    public Transform[] allPlayers;

    public GameObject levelMusic;

    // public bool ballonGround = false;

    public GameObject gameOverPanel;

    public GameObject endCDTimer;

    public GameObject obstacle1, obstacle2;
    public GameObject incomingText;
    
    private void Start()
    {
        // loserPlayer = allPlayers[UnityEngine.Random.Range(0, allPlayers.Length - 1)].gameObject;
        Time.timeScale = 1f;
        isSlowed = false;
        
        if (isTimerStarted)
        {
            
            currentTime = timer;
        }
    }

    private void Update()
    {
        // if (ballonGround)
        // {
        //     StartCoroutine("WaitAndBallToOriginal");
        // }

        if (loserPlayer != null)
        {
            playerWithThebomb.text = loserPlayer.name;
            if (loserPlayer.name == "Player_1")
            {
                playerWithThebomb.color = Color.blue;
            }
            if (loserPlayer.name == "Enemy_1")
            {
                playerWithThebomb.color = Color.yellow;
            }
            if (loserPlayer.name == "Enemy_2")
            {
                playerWithThebomb.color = Color.blue;
            }
            if (loserPlayer.name == "Enemy_3")
            {
                playerWithThebomb.color = Color.grey;
            }
            if (loserPlayer.name == "Enemy_4")
            {
                playerWithThebomb.color =Color.magenta;
            }
        }
        
        if (canSlowNow)
        {
            SlowTime();
        }
        

        if (isGameStarted)
        {
            cdTimer.gameObject.SetActive(true);
            cTime -= 1 * Time.deltaTime* 0.7f;
            cdTimer.text = cTime.ToString("0");
            if (cTime < 0 && isGameStarted)
            {
                
                cdTimer.text = "";
                //cdTimer.gameObject.SetActive(false);
                isGameStarted = false;

                GameManager.Instance.enableCameraMovement = true;
                ballGO.AddComponent<Rigidbody>();
                ballGO.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                ballGO.GetComponent<TrailRenderer>().enabled = true;
                currentTime = timer;
                isTimerStarted = true;
                
                loserPlayer = allPlayers[UnityEngine.Random.Range(0, allPlayers.Length - 1)].gameObject;
            }
        }
        
        if (isTimerStarted)
        {
            enableCameraMovement = true;
            
            levelMusic.SetActive(true);
            currentTime -= 1 * Time.deltaTime;
            timerText.text = currentTime.ToString("0");
            if (currentTime <= 50)
            {
                incomingText.SetActive(true);
            }
            
            if (currentTime <= 45)
            {
                incomingText.SetActive(false);
                obstacle1.SetActive(true);
            }

            if (currentTime <= 35)
            {
                obstacle1.SetActive(false);
            }

            if (currentTime <= 25)
            {
                obstacle2.SetActive(true);
            }
            
            if (currentTime <= 10)
            {
                timerText.color = Color.red;

                if (currentTime <= 3)
                {
                    endCDTimer.SetActive(true);
                }
                
                if (currentTime <= 0)
                {
                    //ballExplosionEffect.SetActive(true);
                    gameOverPanel.SetActive(true);
                    currentTime = 0;
                    GameEnd();
                    ballGO.transform.GetChild(0).gameObject.SetActive(false);
                    ballGO.transform.GetChild(1).gameObject.SetActive(false);
                    
                }
            }
        }
    }

    // IEnumerator WaitAndBallToOriginal()
    // {
    //     yield return new WaitForSeconds(3f);
    //
    //     if (ballonGround)
    //     {
    //         //ball in players hand
    //         Destroy(GameManager.Instance.ballGO.GetComponent<Rigidbody>());
    //         GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = false;
    //         GameManager.Instance.ballGO.transform.SetParent(loserPlayer.transform.GetChild(0).gameObject.transform);
    //         GameManager.Instance.ballGO.transform.position = loserPlayer.transform.GetChild(0).transform.position;
    //         GameManager.Instance.ballGO.transform.rotation = loserPlayer.transform.GetChild(0).transform.rotation;
    //
    //         ballonGround = false;
    //     }
    //         
    //         
    // }

    public void StartGame()
    {
        isGameStarted = true;
        cTime = cdtimer;
        startBtn.SetActive(false);
    }

    void GameEnd()
    {
        isTimerStarted = false;
        //ball in players hand
        Destroy(GameManager.Instance.ballGO.GetComponent<Rigidbody>());
        GameManager.Instance.ballGO.GetComponent<SphereCollider>().enabled = false;
        GameManager.Instance.ballGO.transform.SetParent(loserPlayer.transform.GetChild(0).gameObject.transform);
        GameManager.Instance.ballGO.transform.position = loserPlayer.transform.GetChild(0).transform.position;
        GameManager.Instance.ballGO.transform.rotation = loserPlayer.transform.GetChild(0).transform.rotation;

        //Explosion
        ballExplosionEffect.GetComponent<ParticleSystem>().Play();
        
        //stop movement of camera
        enableCameraMovement = false;
        
        //Focum camera pn player
        StartCoroutine(Transition());
        
       
    }

    
    
    public float transitionDuration = 2.5f;
    public Transform target;
    
    IEnumerator Transition()
    {
        
        
        // float t = 0.0f;
        // Vector3 startingPos = transform.position;
        // while (t < 0.5f)
        // {
        //     t += Time.deltaTime * (Time.timeScale/transitionDuration);
        //
        //
        //     transform.position = Vector3.Lerp(startingPos, target.position, t);
        //     yield return 0;
        // }
        yield return new WaitForSeconds(0.5f);
        //Slow Game
        // ballExplosionEffect.GetComponent<ParticleSystem>().Stop();
        // GameManager.Instance.ballGO.SetActive(false);
        canSlowNow = true;
        //player dead animation
        if (loserPlayer.name == "Player_1")
        {
            loserPlayer.GetComponent<PlayerController>().anim.Play("Death");
        }
        else
        {
            loserPlayer.GetComponent<EightDirectionMovement>().anim.Play("Death");
            
        }
        
       

    }
    
    void SlowTime()
    {
        
        isSlowed = true;
        Time.timeScale = slowTime;
        Time.fixedDeltaTime = slowTime * Time.deltaTime;
        
    }


    public void TryAgain()
    {
        SceneManager.LoadSceneAsync("02_Desert");
        
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
       
    }
   
}
