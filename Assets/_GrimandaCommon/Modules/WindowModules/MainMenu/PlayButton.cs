using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    Grimanda.Common.GameController gameController;
    public void ConnectToGameController(Grimanda.Common.GameController gameController)
    {
        this.gameController = gameController;
    }
    private void Awake()
    {
        this.gameController = GameObject.Find("GameController").GetComponent<Grimanda.Common.GameController>();
    }

    public void PlayOnClick()
    {
        gameController.soundManager.PlaySoundClip(SoundNames.ButtonClick1);
        gameController.mainMenuModule.SetFade(true);
        gameController.mainMenuModule.SetCloseAndopenAction(Grimanda.Common.CloseActions.PlayCoreGame);
        gameController.mainMenuModule.nextDialogToBeOpenedWhenClosed = gameController.coreGame.coreGameScreen;
        gameController.mainMenuModule.animatableUIElements[0].fadeOutAtEnd = true;
        gameController.mainMenuModule.CloseDialog();
    }
}
