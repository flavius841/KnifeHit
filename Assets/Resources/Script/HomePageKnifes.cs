using UnityEngine;
using System.Collections.Generic;


public class HomePageKnifes : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    [SerializeField] float spawnTime;
    [SerializeField] float Timer;
    [SerializeField] float speed;
    [SerializeField] int LeftOrRight;
    [SerializeField] int Right = 1;
    [SerializeField] float Rotation;
    public GameManager gameManager;
    [SerializeField] bool Stop;


    void Start()
    {
        spawnTime = Random.Range(5f, 10f);
    }

    void Update()
    {
        if (gameManager.StartTransition)
        {
            Stop = true;
        }

        foreach (var knife in knives)
        {

            Timer += Time.deltaTime;

            if (Timer >= spawnTime && !Stop)
            {
                spawnTime = Random.Range(15f, 20f);
                Timer = 0;
                LeftOrRight = Random.Range(0, 2);

                if (LeftOrRight == Right)
                {
                    Rotation = Random.Range(-210f, -120f);
                }

                else
                {
                    Rotation = Random.Range(-40f, 50f);
                }

                knife.transform.rotation = Quaternion.Euler(0, 0, Rotation);
                knife.transform.position = new Vector3(0, 0, 0);
            }

            else if (Timer <= 30f)
            {
                knife.transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
            }



        }

    }

}
