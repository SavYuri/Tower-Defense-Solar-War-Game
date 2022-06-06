
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

    bool timeBoostActiveX2;
    bool timeBoostActiveX4;
    bool timeBoostActiveX8;

    public GameObject TimeBoosterX2Obj;
    public GameObject TimeBoosterX4Obj;
    public GameObject TimeBoosterX8Obj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimeBoosterX2()
    {
        timeBoostActiveX2 = !timeBoostActiveX2;
        timeBoostActiveX4 = false;
        if (timeBoostActiveX2)
        {
            Time.timeScale = 2f;
            TimeBoosterX8Obj.gameObject.GetComponent<Animator>().enabled = false;
            TimeBoosterX2Obj.gameObject.GetComponent<Animator>().enabled = true;
            TimeBoosterX4Obj.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (!timeBoostActiveX2)
        {
            TimeBoosterX2Obj.gameObject.GetComponent<Animator>().enabled = false;
            Time.timeScale = 1f;
        }
    }

    public void TimeBoosterX4()
    {
        timeBoostActiveX4 = !timeBoostActiveX4;
        timeBoostActiveX2 = false;
        if (timeBoostActiveX4)
        {
            Time.timeScale = 4f;
            TimeBoosterX8Obj.gameObject.GetComponent<Animator>().enabled = false;
            TimeBoosterX4Obj.gameObject.GetComponent<Animator>().enabled = true;
            TimeBoosterX2Obj.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (!timeBoostActiveX4)
        {
            TimeBoosterX4Obj.gameObject.GetComponent<Animator>().enabled = false;
            Time.timeScale = 1f;
        }

    }

    public void TimeBoosterX8()
    {
        timeBoostActiveX8 = !timeBoostActiveX8;
        timeBoostActiveX2 = false;
        timeBoostActiveX4 = false;
        if (timeBoostActiveX8)
        {
            Time.timeScale = 8f;
            TimeBoosterX8Obj.gameObject.GetComponent<Animator>().enabled = true;
            TimeBoosterX4Obj.gameObject.GetComponent<Animator>().enabled = false;
            TimeBoosterX2Obj.gameObject.GetComponent<Animator>().enabled = false;
        }
        if (!timeBoostActiveX8)
        {
            TimeBoosterX8Obj.gameObject.GetComponent<Animator>().enabled = false;
            Time.timeScale = 1f;
        }

    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
            {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        gameObject.GetComponent<InterstitialAdExample>().ShowAd();
        Toggle();
        //загружает текщую сцену
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);

    }

    public void Menu()
    {
        Toggle();

        sceneFader.FadeTo(menuSceneName);
       
    }

}