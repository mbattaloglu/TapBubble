using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportScreen : Grimanda.Common.GrimandaWindow
{
    public void CloseButtonOnClick()
    {
        SetNextDialogToBeOpen(gameController.mainMenuModule);
        CloseDialog();
    }
}
