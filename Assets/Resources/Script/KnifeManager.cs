using UnityEngine;
using System.Collections.Generic;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    // [SerializeField] List<GameObject> CurrentKnive;
    public GameManager gameManager;
    [SerializeField] float Speed;
    [SerializeField] bool Shoot;
    [SerializeField] GameObject CurrentTarget;
    [SerializeField] GameObject Target;
    [SerializeField] float distance;
    [SerializeField] bool NewKnife;
    [SerializeField] int NumberOfKnives;
    [SerializeField] GameObject CurrentKnife;
    [SerializeField] float Timer;
    [SerializeField] float CurrentTime;
    [SerializeField] int Durability;

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

        Durability = Random.Range(3, 7);
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

        if (Durability > 0 && CurrentTarget.transform.position.y <= 1.3f && CurrentTarget.transform.position.y > 1)
        {

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

        else if (Durability > 0 && gameManager.GameRunning)
        {
            CurrentTarget.transform.position += new Vector3(0, -10 * Time.deltaTime, 0);
        }

        else
        {
            if (CurrentTarget.transform.position.x < 10)
            {
                CurrentTarget.transform.position += new Vector3(7 * Time.deltaTime, 0, 0);
            }

            else
            {
                CurrentTarget = Instantiate(
                Target,
                new Vector3(0, 7.29f, 0),
                Quaternion.identity
                );
                Durability = Random.Range(3, 7);
            }
        }



    }

    public void ShootKnife()
    {
        distance = Vector3.Distance(CurrentKnife.transform.position, CurrentTarget.transform.position);

        if (distance < 2)
        {
            Shoot = false;
            NewKnife = true;
            NumberOfKnives++;
            CurrentKnife.transform.SetParent(CurrentTarget.transform);
            Durability--;
            return;
        }

        CurrentKnife.transform.position += new Vector3(0, Speed * Time.deltaTime, 0);

    }
}
