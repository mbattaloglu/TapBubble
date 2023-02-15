using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsScreen : Grimanda.Common.GrimandaWindow
{
    [SerializeField] TextMeshProUGUI TMPVersion;
    public override void DoSpecificThingsAtOpen()
    {
        base.DoSpecificThingsAtOpen();
        TMPVersion.text = Application.version;
    }
    public void quitButtonOnClick()
    {
        gameController.soundManager.PlaySoundClip(SoundNames.ButtonClick1);
        SetNextDialogToBeOpen(gameController.mainMenuModule);
        CloseDialog();
    }
}
