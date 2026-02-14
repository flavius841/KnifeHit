using UnityEngine;
using System.Collections.Generic;

public class HomePageKnifes : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;
    [SerializeField] GameObject knifePrefab;

    void Start()
    {
        foreach (var knife in knives)
        {
            knives.Add(knifePrefab);
            // knife.SetActive(true);
        }
    }

    void Update()
    {

    }
}
