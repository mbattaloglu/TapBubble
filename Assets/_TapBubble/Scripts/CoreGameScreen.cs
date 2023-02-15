using System.Collections;
using UnityEngine;

public class CoreGameScreen : Grimanda.Common.GrimandaWindow
{
    public void PauseOnClick()
    {

    }

    public void UpdateHealthBar()
    {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
        }
    }

    public void DecreaseHealth(int index)
    {
        transform.GetChild(1).GetChild(index).gameObject.SetActive(false);
    }
}
