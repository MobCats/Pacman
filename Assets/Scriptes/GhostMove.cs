using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMove : MonoBehaviour
{
    public GameObject[] wayPointsGos;
    public float speed = 0.2f;
    private List<Vector3> wayPoints = new List<Vector3>();
    private int index = 0;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position + new Vector3(0, 3, 0);
        LoadAPath(wayPointsGos[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
    }
    private void FixedUpdate()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        if (transform.position != wayPoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            rigidbody2D.MovePosition(temp);
        }
        else
        {
            index++;
            if (index >= wayPoints.Count)
            {
                index = 0;
                LoadAPath(wayPointsGos[Random.Range(0, wayPointsGos.Length)]);
            }

        }
        //获取运动方向
        Vector2 dir = wayPoints[index] - transform.position;
        //将运动方向赋予状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

    }

    private void LoadAPath(GameObject go)
    {
        wayPoints.Clear();
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, startPos);
        wayPoints.Add(startPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.isSuperPacman)
            {
                GameManager.Instance.score += 500;
                transform.position = startPos - new Vector3(0, 3, 0);
                index = 0;
            }
            else
            {

                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameOverPrefab);
                Invoke("ReStart", 3f);
            }

        }
    }

    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }


}
