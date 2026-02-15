using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool HomeTransition;

    [SerializeField] GameObject HomeButtons;
    [SerializeField] GameObject Logo;
    [SerializeField] float Speed;
    [SerializeField] bool DelayDone;
    [SerializeField] GameObject TargetNormal;
    [SerializeField] GameObject SettingsButtons;
    [SerializeField] bool SettingsTransition;
    [SerializeField] GameObject PlayButton;

    void Start()
    {

    }

    void Update()
    {
        if (HomeTransition)
        {
            QuitHome(Logo, HomeButtons, ref HomeTransition, TargetNormal);
        }

        if (SettingsTransition)
        {
            QuitHome(Logo, PlayButton, ref SettingsTransition, SettingsButtons);
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

    public void QuitHome(GameObject RightTransition, GameObject LeftTransition, ref bool QuitHomeTransition, GameObject NextObject)
    {
        if (NextObject.transform.position.y <= 1)
        {
            QuitHomeTransition = false;
        }

        else if (DelayDone)
        {

            Transition(LeftTransition, RightTransition, -Speed);
            NextObject.transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);
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

    public void QuitGame()
    {
        Application.Quit();
    }

}
