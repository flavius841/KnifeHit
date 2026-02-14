using UnityEngine;
using System.Collections.Generic;

public class HomePageKnifes : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    [SerializeField] GameObject knifePrefab;
    [SerializeField] float spawnTime;
    [SerializeField] float Timer;
    [SerializeField] float speed;
    [SerializeField] int LeftToRight;
    [SerializeField] int Right = 1;
    [SerializeField] int Left = 0;
    [SerializeField] float Rotation;
    [SerializeField] float Limit;

    void Start()
    {

    }

    void Update()
    {
        foreach (var knife in knives)
        {
            spawnTime = Random.Range(0.5f, 2f);
            Timer += Time.deltaTime;

            if (Timer >= spawnTime)
            {
                Timer = 0;
                LeftToRight = random.randint(0, 1);

                if (LeftToRight == Right)
                {

                }

                else
                {
                    Rotation = Random.Range(-40f, 50f);
                    knife.transform.rotation = Quaternion.Euler(0, 0, Rotation);
                    knife.transform.position = new Vector3(0, 0, 0);


                }


            }

        }

    }
}
