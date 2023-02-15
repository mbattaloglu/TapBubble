using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : Grimanda.Common.GrimandaWindow
{
    public void QuitOnClick()
    {
        gameController.coreGame.PlayerQuitedTheGame();
        SetFade(false);
        gameController.mainMenuModule.SetFade(false);
        SetNextDialogToBeOpen(gameController.mainMenuModule);
        CloseDialog();
    }

    public void ContinueOnClick()
    {
        SetFade(true);
        SetNextDialogToBeOpen(gameController.coreGame.coreGameScreen);
        CloseDialog();
        gameController.coreGame.ResumeGame();
    }
}
