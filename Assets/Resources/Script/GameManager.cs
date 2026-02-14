using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LeftTransition;
    [SerializeField] GameObject RightTransition;
    [SerializeField] float Speed;
    public bool StartTransition;
    [SerializeField] bool DelayDone;
    [SerializeField] GameObject TargetNormal;

    void Start()
    {

    }

    void Update()
    {
        if (StartTransition)
        {
            if (DelayDone)
            {
                LeftTransition.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
                RightTransition.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            }

            else
            {
                LeftTransition.transform.position += new Vector3(Speed / 2 * Time.deltaTime, 0, 0);
                RightTransition.transform.position += new Vector3(-Speed / 2 * Time.deltaTime, 0, 0);

                if (RightTransition.transform.position.x <= -0.3f)
                {
                    DelayDone = true;
                }
            }

            TargetNormal.transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);

            if (TargetNormal.transform.position.y <= 1)
            {
                StartTransition = false;
            }
        }

    }

    public void Transition()
    {
        StartTransition = true;
    }

}
