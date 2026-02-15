using UnityEngine;

[ExecuteAlways]
public class KnifeMenuNames : MonoBehaviour
{
    [SerializeField] GameObject KnifeMenuContent;
    void Start()
    {
        int i = 0;

        foreach (Transform child in KnifeMenuContent.transform)
        {
            child.gameObject.transform.name = "Knife " + i;
            i++;
        }

    }

    void Update()
    {

    }
}
