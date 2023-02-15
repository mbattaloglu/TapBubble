using UnityEngine;

namespace Grimanda.Common
{
    public class SoundModule : MonoBehaviour
    {

        public AudioSource audioSourceForSounds;
        public AudioSource audioSourceForMusic;
        public SoundClips soundClips;
        [SerializeField] AudioSource[] additionalAudioSources;

        public GameController gameController;

        public void ConnectToGameController(GameController gameController)
        {
            this.gameController = gameController;
        }


        public void PlayMusic()
        {
            if (gameController.playerData.GetBool(PlayerDataTags.Music))
            {
                audioSourceForMusic.Play();
            }
            else
            {
                audioSourceForMusic.Pause();
            }
        }


        public void SetAdditionalAudioSources(bool stop)
        {
            for(int i=0;i<additionalAudioSources.Length;i++)
            {
                if (gameController.playerData.GetBool(PlayerDataTags.SFX))
                {
                    if (!stop)
                    {
                        if (additionalAudioSources[i].loop)
                        {
                            additionalAudioSources[i].Play();
                        }
                    }
                    else
                    {
                        additionalAudioSources[i].Stop();
                    }
                }
                else
                {
                    additionalAudioSources[i].Stop();
                }
            }
        }


        public void PlaySoundClip(AudioClip audioClip)
        {
            if (!gameController.playerData.GetBool(PlayerDataTags.SFX))
            {
                return;
            }

            audioSourceForSounds.PlayOneShot(audioClip);
        }

        public void PlaySoundClip(SoundNames soundName=SoundNames.ButtonClick1)
        {
            if (!gameController.playerData.GetBool(PlayerDataTags.SFX))
            {
                return;
            }

            audioSourceForSounds.PlayOneShot(soundClips.soundClipSources[(int)soundName].value);
        }

        public void PlaySoundClip(AudioSource audioSource, SoundNames soundName = SoundNames.ButtonClick1)
        {
            if (!gameController.playerData.GetBool(PlayerDataTags.SFX))
            {
                return;
            }

            audioSource.Play();
        }



    }
}
