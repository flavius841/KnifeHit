using UnityEngine;
using System.Collections.Generic;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    [SerializeField] List<GameObject> CurrentKnive;
    public GameManager gameManager;
    [SerializeField] float Speed;
    [SerializeField] bool Shoot;
    [SerializeField] GameObject Target;
    [SerializeField] float distance;
    [SerializeField] bool NewKnife;
    [SerializeField] int NumberOfKnives;

    void Start()
    {
        foreach (var knife in knives)
        {
            knife.transform.position = new Vector3(0, -6.5f, 0);
        }

        CurrentKnive.Add(knives[GameManager.ID]);
    }

    void Update()
    {
        if (gameManager.GameRunning)
        {
            if (knives[GameManager.ID].transform.position.y < -3.5f)
            {
                knives[GameManager.ID].transform.position += new Vector3(0, 10 * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(CurrentKnive[NumberOfKnives], new Vector3(0, -6.5f, 0), Quaternion.identity);
            Shoot = true;
            NumberOfKnives++;
        }

        if (Shoot)
        {
            ShootKnife();
        }

        if (NewKnife)
        {
            CurrentKnive.Add(knives[GameManager.ID]);
            NewKnife = false;
        }

    }

    public void ShootKnife()
    {
        distance = Vector3.Distance(CurrentKnive[NumberOfKnives].transform.position, Target.transform.position);

        if (distance < 2)
        {
            Shoot = false;
            NewKnife = true;
            return;
        }

        CurrentKnive[NumberOfKnives].transform.position += new Vector3(0, Speed * Time.deltaTime, 0);

    }
}
