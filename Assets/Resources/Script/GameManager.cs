using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool HomeTransition;

    [SerializeField] GameObject HomeButtons;
    [SerializeField] GameObject Logo;
    [SerializeField] float Speed;
    [SerializeField] bool DelayDone;
    [SerializeField] GameObject TargetNormal;
    [SerializeField] GameObject SettingsButton;
    [SerializeField] bool SettingsTransition;

    void Start()
    {

    }

    void Update()
    {
        if (HomeTransition)
        {
            QuitHome(Logo, HomeButtons);
        }
    }


    public void QuitHomeBool()
    {
        HomeTransition = true;
    }

    public void SettingsButtonBool()
    {
        SettingsTransition = true;
    }

    public void Transition(GameObject RightTransition, GameObject LeftTransition, float TransitionSpeed)
    {
        RightTransition.transform.position += new Vector3(TransitionSpeed * Time.deltaTime, 0, 0);
        LeftTransition.transform.position += new Vector3(-TransitionSpeed * Time.deltaTime, 0, 0);
    }

    public void QuitHome(GameObject RightTransition, GameObject LeftTransition)
    {
        if (DelayDone)
        {
            Transition(LeftTransition, RightTransition, -Speed);
            TargetNormal.transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);

            if (TargetNormal.transform.position.y <= 1)
            {
                HomeTransition = false;
            }
        }

        else
        {
            Transition(LeftTransition, RightTransition, Speed / 2);
        }

        if (RightTransition.transform.position.x <= -0.3f)
        {
            DelayDone = true;
        }
    }

}
