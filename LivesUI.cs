
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesText;
    
   
    void Update()
    {
        //строка жизни в меню
        livesText.text = PlayerStats.Lives.ToString(); 
    }
}
