using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    public class MusicButton : SwitchButton
    {
        public SoundModule soundModule;

        public override void ExtraThingsToDoAfterSwitch()
        {
            soundModule.PlayMusic();
            soundModule.PlaySoundClip(SoundNames.ButtonClick1);
        }

    }
}
