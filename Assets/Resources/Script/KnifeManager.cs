using UnityEngine;
using System.Collections.Generic;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    public GameManager gameManager;


    void Start()
    {
        foreach (var knife in knives)
        {
            // knife.transform.position = new Vector3(0, -3.5f, 0);
            knife.transform.position = new Vector3(0, -6.5f, 0);
        }
    }

    void Update()
    {
        if (gameManager.GameRunning)
        {
            if (knives[GameManager.ID].transform.position.y < -3.5f)
            {
                knives[GameManager.ID].transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            knives[GameManager.ID].transform.position += new Vector3(0, 20 * Time.deltaTime, 0);
        }

    }
}
