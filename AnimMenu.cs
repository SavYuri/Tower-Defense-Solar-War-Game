using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMenu : MonoBehaviour
{
    public Animator SpeedAnim;
    public Animator SWAnim;
    public Animator TorretAnim;
    public Animator StatAnim;
    public static AnimMenu animMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (animMenu != null) return;
        else animMenu = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAllNavigation()
    {
        SpeedAnim.SetTrigger("Out");
        SWAnim.SetTrigger("Out");
        TorretAnim.SetTrigger("Out");
        StatAnim.SetTrigger("Out");
    }
}
