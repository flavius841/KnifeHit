using UnityEngine;
using System.Collections.Generic;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    // [SerializeField] List<GameObject> CurrentKnive;
    public GameManager gameManager;
    [SerializeField] float Speed;
    [SerializeField] bool Shoot;
    [SerializeField] GameObject Target;
    [SerializeField] float distance;
    [SerializeField] bool NewKnife;
    [SerializeField] int NumberOfKnives;
    [SerializeField] GameObject CurrentKnife;
    [SerializeField] float Timer;
    [SerializeField] float CurrentTime;

    void Start()
    {
        foreach (var knife in knives)
        {
            knife.transform.position = new Vector3(0, -6.5f, 0);
        }

        CurrentKnife = Instantiate(
        knives[GameManager.ID],
        new Vector3(0, -6.5f, 0),
        Quaternion.identity
        );


    }


    void Update()
    {
        if (gameManager.GameRunning)
        {
            if (CurrentKnife.transform.position.y < -3.5f)
            {
                CurrentKnife.transform.position += new Vector3(0, 10 * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Shoot = true;

        }

        if (Shoot)
        {
            ShootKnife();
        }

        if (NewKnife)
        {
            CurrentKnife = Instantiate(
            knives[GameManager.ID],
            new Vector3(0, -6.5f, 0),
            Quaternion.identity
            );

            NewKnife = false;
        }

    }

    public void ShootKnife()
    {
        distance = Vector3.Distance(CurrentKnife.transform.position, Target.transform.position);

        if (distance < 2)
        {
            Shoot = false;
            NewKnife = true;
            NumberOfKnives++;
            CurrentKnife.transform.SetParent(Target.transform);

            return;
        }

        CurrentKnife.transform.position += new Vector3(0, Speed * Time.deltaTime, 0);

    }
}
