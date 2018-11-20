using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDownPrefab;
    public GameObject gameOverPrefab;
    public GameObject WinPrefab;
    public AudioClip startClip;
    public Text remianText;
    public Text nowText;
    public Text scoreText;


    public bool isSuperPacman = false;


    private int PacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;
    private List<GameObject> pacdotGos = new List<GameObject>(); 
     
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int>() { 0, 1, 2, 3 };

    private void Start()
    {
        SetGameState(false);
        
    }

    private void Update()
    {
        if(nowEat == PacdotNum&&pacman.GetComponent<PacmanMove>().enabled != false)
        {
            gamePanel.SetActive(false);
            Instantiate(WinPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if(nowEat==PacdotNum)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        if(gamePanel.activeInHierarchy)
        {
            remianText.text = "Remain:\n\n" + (PacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" +  nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

    public void OnStartButton()
    {
        StartCoroutine(PlayStarCountDown());
        AudioSource.PlayClipAtPoint(startClip,new Vector3(3f,3f,0));
        startPanel.SetActive(false);
    }

    IEnumerator PlayStarCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);
        Invoke("CreateSuperPacdot", 10f);
        GetComponent<AudioSource>().Play();
        gamePanel.SetActive(true);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
    }

    public void OnEatSuperPacdots()
    {
        score += 200;
        Invoke("CreateSuperPacdot", 10f);
        isSuperPacman = true;
        FreezeEnemy();
        StartCoroutine(RecoveryEnemy());
        

    }

    IEnumerator RecoveryEnemy()
    {
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    private void FreezeEnemy()
    {
        blinky.GetComponent<GhostMove>().enabled = false;
        clyde.GetComponent<GhostMove>().enabled = false;
        inky.GetComponent<GhostMove>().enabled = false;
        pinky.GetComponent<GhostMove>().enabled = false;
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
    }
    private void DisFreezeEnemy()
    {
        blinky.GetComponent<GhostMove>().enabled = true;
        clyde.GetComponent<GhostMove>().enabled = true;
        inky.GetComponent<GhostMove>().enabled = true;
        pinky.GetComponent<GhostMove>().enabled = true;
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); 
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    private void Awake()
    {
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }

        PacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void CreateSuperPacdot()
    {
        if(pacdotGos.Count<5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperpacdot = true;
    }

    private void SetGameState(bool state)
    {
        pacman.GetComponent<PacmanMove>().enabled = state;
        blinky.GetComponent<GhostMove>().enabled = state;
        clyde.GetComponent<GhostMove>().enabled = state;
        inky.GetComponent<GhostMove>().enabled = state;
        pinky.GetComponent<GhostMove>().enabled = state;
    }
}
