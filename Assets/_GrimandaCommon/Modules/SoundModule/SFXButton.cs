using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class SFXButton : SwitchButton
    {
        public SoundModule soundModule;

        public override void ExtraThingsToDoAfterSwitch()
        {
            soundModule.PlaySoundClip(SoundNames.ButtonClick1);
        }
    }
}
