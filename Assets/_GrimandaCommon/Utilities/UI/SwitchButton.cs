using UnityEngine;
using UnityEngine.UI;

namespace Grimanda.Common
{
    public class SwitchButton : MonoBehaviour
    {
        public GameController gameController;
        public GameObject imgOn;
        public GameObject imgOff;

        public string playerDataTag;


        private void Awake()
        {
            SetButtonVisual();
            GetComponent<Button>().onClick.AddListener(()=> { OnClick(); });
        }

        public void SetButtonVisual()
        {
            imgOff.SetActive(!gameController.playerData.GetBool(playerDataTag));
            imgOn.SetActive(gameController.playerData.GetBool(playerDataTag));
        }

        public virtual void ExtraThingsToDoAfterSwitch()
        {

        }

        public void OnClick()
        {
            gameController.playerData.InvertBool(playerDataTag);
            SetButtonVisual();
            gameController.soundManager.PlaySoundClip(SoundNames.ButtonClick1);
            ExtraThingsToDoAfterSwitch();
        }

        

        private void OnEnable()
        {
            SetButtonVisual();
        }
    }
}
