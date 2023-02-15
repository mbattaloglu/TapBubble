using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : Grimanda.Common.GrimandaWindow
{
    public void CloseButtonOnClick()
    {
        SetNextDialogToBeOpen(gameController.mainMenuModule);
        CloseDialog();
    }

}
