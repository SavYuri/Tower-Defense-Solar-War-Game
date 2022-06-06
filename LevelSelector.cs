using UnityEngine;
using UnityEngine.UI;


//открытие уровней
public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    public Button[] levelButtons;
    public DifficultLevelButtons [] DifficultLevelButtons;
    public Color unselectColor;
    public Color selectedColor;
   
    int selectedLevel;
   


    int lastOpenLevel;

    private void Start()
    {
        lastOpenLevel = PlayerPrefs.GetInt("LastLevel");
        DisableLavelsButon();
        CheckInterectableLevelButtons();
        SelectLevel(lastOpenLevel);
        SelectDifficulty(0);

    }


    

    void CheckInterectableLevelButtons()
    {
        for (int i = 0;  i < levelButtons.Length; i++)
        {
            if (i <= lastOpenLevel)
            {
                levelButtons[i].interactable = true;
            }
            else return;
            
        }
    }

    void DisableLavelsButon()
    {
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }

    void DeselectLevelButtonsColor()
    {
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].gameObject.GetComponent<Image>().color = unselectColor;
        }
    }


    void SetColorStarsOfLevel(int index) //index - Game Level
    {
        
        int valueDifficultOfLevel = 3;
        string GameLevelString = index.ToString(); ;
        string difficultOfLevelString;
        string pPrefsName; //format: "DifficultStar22"; 2 - Game Level, 2 - Difficult Level 

        for (int i = 0; i < valueDifficultOfLevel; i++) // i - DifficultOfLevel, 0 -normal, 1 - hard, 2 - extrime
        {
            difficultOfLevelString = i.ToString();
            pPrefsName = "DifficultStar" + GameLevelString + difficultOfLevelString;
            int levelStar = PlayerPrefs.GetInt(pPrefsName); //0 - level not finish, 1 - 1 star, etc...

            for (int star = 0; star < levelStar; star++)
            {
                DifficultLevelButtons[i].star[star].color = selectedColor;
            }
            for (int star = levelStar; star < 4; star++)
            {
                DifficultLevelButtons[i].star[star].color = unselectColor;
            }
        }
    }



    public void SelectLevel (int index)
    {
        DeselectLevelButtonsColor();
        levelButtons[index].gameObject.GetComponent<Image>().color = selectedColor;
        selectedLevel = index;
        SetColorStarsOfLevel(index);
    }

    public void StartLevel()
    {
        fader.FadeTo(GameManager.gameLevels[selectedLevel]);
    }

    public void SelectDifficulty(int index)
    {
       
        PlayerPrefs.SetInt("LevelOfDifficulty", index);

        for (int i = 0; i < 3; i++)
        {
            if (i == index)
            {
                DifficultLevelButtons[i].levelButton.gameObject.GetComponent<Image>().color = selectedColor;
            }
            else
            {
                DifficultLevelButtons[i].levelButton.gameObject.GetComponent<Image>().color = unselectColor;
            }
        }
    }

}



[System.Serializable]
public class DifficultLevelButtons
{
    public string level;
    public Button levelButton;
    public Image [] star;
}

