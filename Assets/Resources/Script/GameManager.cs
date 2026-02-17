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
    [SerializeField] int ID;
    [SerializeField] bool KnifeMenuTransition;
    [SerializeField] GameObject KnifeMenuButton;
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject KnifeMenu;
    [SerializeField] bool SettingsOpened;
    [SerializeField] bool BackToHomeTransition1;
    [SerializeField] bool BackToHomeTransition2;
    [SerializeField] int Phase;

    void Start()
    {

    }

    void Update()
    {
        if (HomeTransition)
        {
            QuitHome(Logo, HomeButtons, ref HomeTransition, TargetNormal, Speed, 1);
        }

        if (SettingsTransition)
        {
            QuitHome(Logo, PlayButton, ref SettingsTransition, SettingsButtons, Speed, 1);
            SettingsOpened = true;
        }

        if (KnifeMenuTransition)
        {
            QuitHome(ExitButton, KnifeMenuButton, ref KnifeMenuTransition, KnifeMenu, Speed, 1);
            Phase = 3;
        }

        if (BackToHomeTransition1)
        {
            QuitHome(Logo, PlayButton, ref BackToHomeTransition1, SettingsButtons, -Speed, 8);
            SettingsOpened = false;
        }

        if (BackToHomeTransition2)
        {
            //QuitHome(Logo, PlayButton, ref BackToHomeTransition2, KnifeMenu, -Speed, 13);
            QuitHome(Logo, PlayButton, ref BackToHomeTransition2, KnifeMenu, -Speed / 3, 13.3f);
            KnifeMenu.transform.position += new Vector3(0, Speed / 3.3f * Time.deltaTime, 0);
        }
    }


    public void QuitHomeBool()
    {
        HomeTransition = true;
    }

    public void SettingsButtonBool()
    {
        if (!SettingsTransition && !BackToHomeTransition1 && !KnifeMenuTransition)
        {
            Phase++;

            if (Phase == 3)
            {
                Phase = 1;
            }

            // if (Phase == 4)
            // {
            //     Phase = 0;
            // }

        }

        if (Phase == 1)
        {
            BackToHomeTransition1 = true;
        }

        else if (Phase == 2)
        {
            SettingsTransition = true;
        }

        else if (Phase == 4)
        {
            Phase = 1;
            ExitButton.transform.position = new Vector3(0, 6.24f, 0);
            KnifeMenuButton.transform.position = new Vector3(0, 9.67f, 0);
            BackToHomeTransition2 = true;

        }
    }

    public void KnifeMenuButtonBool()
    {
        KnifeMenuTransition = true;
    }

    public void Transition(GameObject RightTransition, GameObject LeftTransition, float TransitionSpeed)
    {
        RightTransition.transform.position += new Vector3(TransitionSpeed * Time.deltaTime, 0, 0);
        LeftTransition.transform.position += new Vector3(-TransitionSpeed * Time.deltaTime, 0, 0);
    }

    public void QuitHome(GameObject RightTransition, GameObject LeftTransition,
     ref bool QuitHomeTransition, GameObject NextObject, float TransitionSpeed, float NextObjectY)
    {
        if (NextObject.transform.position.y <= NextObjectY + 0.3f && NextObject.transform.position.y >= NextObjectY)
        {
            QuitHomeTransition = false;
        }

        else if (DelayDone)
        {
            Transition(LeftTransition, RightTransition, -TransitionSpeed);
            NextObject.transform.position += new Vector3(0, -TransitionSpeed * Time.deltaTime, 0);
        }

        else
        {
            Transition(LeftTransition, RightTransition, TransitionSpeed / 2);
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

    public void KnifeID(int id)
    {
        ID = id;
    }

}
