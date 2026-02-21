using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    [SerializeField] List<float> angles;
    // [SerializeField] List<GameObject> CurrentKnive;
    public GameManager gameManager;
    [SerializeField] float Speed;
    [SerializeField] bool Shoot;
    [SerializeField] GameObject CurrentTarget;
    [SerializeField] GameObject PrefabTarget;
    [SerializeField] float distance;
    [SerializeField] float Obstacledistance;
    [SerializeField] float KnifesDistance;
    [SerializeField] bool NewKnife;
    [SerializeField] int NumberOfKnives;
    [SerializeField] GameObject CurrentKnife;
    [SerializeField] float Timer;
    [SerializeField] float CurrentTime;
    [SerializeField] int Durability;
    [SerializeField] bool Hard;
    [SerializeField] float RotateSpeed;
    [SerializeField] float MaxRotateSpeed;
    [SerializeField] bool FirstTime;
    [SerializeField] bool FirstKnife = true;
    [SerializeField] int ObstacleNumber;
    [SerializeField] int Score;
    [SerializeField] GameObject ObstaclePrefab;
    [SerializeField] List<GameObject> Obstacles;
    [SerializeField] List<GameObject> KbifeList;
    [SerializeField] float min;
    [SerializeField] float max;
    [SerializeField] float minDistance = 5f;
    public bool Lose;
    [SerializeField] TextMeshProUGUI LoseText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] float TextAlpha = 0;
    [SerializeField] float KnifeAplha = 1;
    [SerializeField] bool FadedIn;

    void Start()
    {
        foreach (var knife in knives)
        {
            knife.transform.position = new Vector3(0, -6.5f, 0);
        }

        Durability = Random.Range(3, 7);
    }


    void Update()
    {
        if (Lose)
        {
            LoseText.text = "You Losed \nScore: " + Score.ToString();
            if (TextAlpha == 1)
            {
                FadedIn = true;
            }

            if (FadedIn)
            {
                FadeOutText();
            }

            else
            {
                FadeInText();
            }

            FadeCurrentKnife();
            Durability = 0;
        }

        if (Durability == 0)
        {
            FirstTime = false;
        }

        if (gameManager.GameRunning && FirstKnife)
        {
            CurrentKnife = Instantiate(
            knives[GameManager.ID],
            new Vector3(0, -6.5f, 0),
            Quaternion.identity
            );

            FirstKnife = false;
        }

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
                KbifeList.Add(CurrentKnife);

                CurrentKnife = Instantiate(
                knives[GameManager.ID],
                new Vector3(0, -6.5f, 0),
                Quaternion.identity
                );

                NewKnife = false;

            }


        }

        else if (Durability > 0 && gameManager.GameRunning && !FirstTime)
        {
            CurrentTarget.transform.position += new Vector3(0, -10 * Time.deltaTime, 0);
        }

        else if (gameManager.GameRunning && !FirstTime)
        {
            if (CurrentTarget.transform.position.x < 10)
            {
                CurrentTarget.transform.position += new Vector3(7 * Time.deltaTime, 0, 0);
            }

            else if (!Lose)
            {
                CurrentTarget = Instantiate(
                PrefabTarget,
                new Vector3(0, 7.29f, -1),
                Quaternion.identity
                );
                Durability = Random.Range(3, 7);
                RotateSpeed = Random.Range(100, 200);
                MaxRotateSpeed = RotateSpeed;
                Hard = Random.Range(0, 2) == 1;
                ObstacleNumber = Random.Range(0, 9);
                angles = GenerateAngles(ObstacleNumber);
                KbifeList.Clear();

                Obstacles.Clear();

                for (int i = 0; i < ObstacleNumber; i++)
                {
                    GameObject obs = Instantiate(
                        ObstaclePrefab,
                        CurrentTarget.transform
                    );

                    obs.transform.localPosition = new Vector3(0, 0, 1);
                    obs.transform.localRotation = Quaternion.Euler(0, 0, angles[i]);

                    Obstacles.Add(obs);
                }

                Score++;
            }
        }

        RotateTarget();
        LoseFunction();

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

    public void RotateTarget()
    {
        if (CurrentTarget == null)
        {
            return;
        }

        if (Hard)
        {

            RotateSpeed = Mathf.PingPong(Time.time * 20, MaxRotateSpeed * 2) - MaxRotateSpeed;
        }

        CurrentTarget.transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));
    }


    public List<float> GenerateAngles(int count)
    {
        List<float> values = new List<float>();

        int safety = 0;

        while (values.Count < count && safety < 10000)
        {
            float candidate = Random.Range(min, max);

            bool tooClose = false;

            foreach (float value in values)
            {
                if (Mathf.Abs(value - candidate) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                values.Add(candidate);

            safety++;
        }

        return values;
    }

    public void LoseFunction()
    {
        foreach (var obs in Obstacles)
        {
            Obstacledistance = Vector3.Distance(CurrentKnife.transform.position, obs.transform.GetChild(0).position);

            if (Obstacledistance < 0.5f)
            {
                Lose = true;
            }
        }

        foreach (var ShotedKnifes in KbifeList)
        {
            KnifesDistance = Vector3.Distance(CurrentKnife.transform.position, ShotedKnifes.transform.position);

            if (KnifesDistance < 0.4f)
            {
                Lose = true;
            }


        }
    }

    public void FadeInText(TextMeshProUGUI textType)
    {
        TextAlpha = Mathf.MoveTowards(TextAlpha, 1, 2 * Time.deltaTime);
        Color color = LoseText.color;
        color.a = TextAlpha;
        LoseText.color = color;
    }

    public void FadeOutText(TextMeshProUGUI textType)
    {
        TextAlpha = Mathf.MoveTowards(TextAlpha, 0, 0.2f * Time.deltaTime);
        Color color = LoseText.color;
        color.a = TextAlpha;
        LoseText.color = color;
    }


    public void FadeCurrentKnife()
    {
        KnifeAplha = Mathf.MoveTowards(KnifeAplha, 0, 2 * Time.deltaTime);
        Color color1 = CurrentKnife.GetComponent<SpriteRenderer>().color;
        color1.a = KnifeAplha;
        CurrentKnife.GetComponent<SpriteRenderer>().color = color1;

        foreach (Transform child in CurrentKnife.transform)
        {
            Color color2 = child.GetComponent<SpriteRenderer>().color;
            color2.a = KnifeAplha;
            child.GetComponent<SpriteRenderer>().color = color2;
        }
    }

}
