using UnityEngine;
using System.Collections.Generic;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] List<GameObject> knives;

    void Start()
    {
        foreach (var knife in knives)
        {
            knife.transform.position = new Vector3(0, -3.5f, 0);
        }
    }

    void Update()
    {

    }
}
