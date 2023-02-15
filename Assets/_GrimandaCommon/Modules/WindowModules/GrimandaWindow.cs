using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

namespace Grimanda.Common
{
    public enum UIAnimType
    {
        NoMovement,
        FadeAnim,
        MoveFromLeftToMiddle,
        MoveFromRightToMiddle,
        MoveFromTopToMiddle,
        MoveFromBottomToMiddle,
        ScaleUp,
        ActivateAnimatorTriggerAtEnd,
        ActivateAnimatorTriggerAtStart,
        ActivateAnimatorTriggerAtStartAndEnd
    }

    public enum CloseActions
    {
        None,
        PlayCoreGame
    }

    [System.Serializable]
    public struct AnimatableUIElement
    {
        public RectTransform rectTransform;
        public Vector2 anchoredPosInScreen;
        public Vector2 anchoredPosOutScreenLeft;
        public Vector2 anchoredPosOutScreenRight;
        public bool fadeOutAtEnd;
        public bool fadeInAtStart;
        public UIAnimType animType;
        public string animatorStartTrigger;
        public string animatorEndTrigger;
        public float startingAlpha;
        public float endAlpha;
        public Vector3 endScale;
    }

    public class GrimandaWindow : MonoBehaviour
    {
        public GameController gameController;

        public GrimandaWindow[] dialogsToBeRefreshedAfterClose;
        public bool refreshConnectedDialogs;
        public GrimandaWindow nextDialogToBeOpenedWhenClosed;
        public int priority;
        public bool animateOnOpen;
        public bool animateOnClose;
        
        public AnimatableUIElement[] animatableUIElements;
        [SerializeField]bool fadeActive;

        public UnityEvent OnWindowEnable;
        public UnityEvent OnWindowDisable;

        public CloseActions closeAction;

        public void ConnectToGameController(GameController gameController)
        {
            
            this.gameController = gameController;
        }

        public virtual void OnOpenComplete()
        {

        }

        public void SetCloseAndopenAction(CloseActions closeAction)
        {
            this.closeAction = closeAction;
        }

        public virtual void OnCloseComplete()
        {

        }

        void Awake()
        {
            SetCloseAndopenAction(CloseActions.None);
            for (int i = 0; i < animatableUIElements.Length; i++)
            {
                switch (animatableUIElements[i].animType)
                {
                    case UIAnimType.FadeAnim:
                        animatableUIElements[i].startingAlpha = 0;
                        animatableUIElements[i].endAlpha = animatableUIElements[i].rectTransform.GetComponent<Image>().color.a;
                    break;
                    case UIAnimType.ScaleUp:
                        animatableUIElements[i].endScale = animatableUIElements[i].rectTransform.localScale;
                        break;
                    case UIAnimType.MoveFromLeftToMiddle:
                    case UIAnimType.MoveFromRightToMiddle:
                        animatableUIElements[i].anchoredPosInScreen = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenLeft = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenRight = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenRight.x += Screen.width;
                        animatableUIElements[i].anchoredPosOutScreenLeft.x -= Screen.width;
                    break;
                    case UIAnimType.MoveFromBottomToMiddle:
                        animatableUIElements[i].anchoredPosInScreen = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenLeft = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenLeft.y -= Screen.height;
                    break;
                    case UIAnimType.MoveFromTopToMiddle:
                        animatableUIElements[i].anchoredPosInScreen = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenLeft = animatableUIElements[i].rectTransform.anchoredPosition;
                        animatableUIElements[i].anchoredPosOutScreenLeft.y += Screen.height;
                    break;

                }
            }

        }

        public void CloseDialog()
        {
            if (animateOnClose)
            {
                StartCoroutine(SetTheUIElementsOut());
            }
            else
            {
                gameObject.SetActive(false);
                OnCloseComplete();
                ProceedToTheNextScreen();
            }
            if (refreshConnectedDialogs)
            {
                for (int i = 0; i < dialogsToBeRefreshedAfterClose.Length; i++)
                {
                    dialogsToBeRefreshedAfterClose[i].Refresh();
                }
            }
            DoSpecificThingsAtClose();
            OnWindowDisable.Invoke();
        }

        public virtual void Refresh()
        {

        }

        public virtual void DoSpecificThingsAtOpen()
        {

        }

        public virtual void DoSpecificThingsAtClose()
        {

        }

        IEnumerator SetTheUIElementsIn()
        {
            if (animateOnOpen)
            {
                for (int i = 0; i < animatableUIElements.Length; i++)
                {
                    switch (animatableUIElements[i].animType)
                    {
                        case UIAnimType.MoveFromLeftToMiddle:
                            animatableUIElements[i].rectTransform.anchoredPosition = animatableUIElements[i].anchoredPosOutScreenLeft;
                            break;
                        case UIAnimType.MoveFromRightToMiddle:
                            animatableUIElements[i].rectTransform.anchoredPosition = animatableUIElements[i].anchoredPosOutScreenRight;
                            break;
                        case UIAnimType.MoveFromTopToMiddle:
                        case UIAnimType.MoveFromBottomToMiddle:
                            animatableUIElements[i].rectTransform.anchoredPosition = animatableUIElements[i].anchoredPosOutScreenLeft;
                            break;
                        case UIAnimType.FadeAnim:
                            if (fadeActive)
                            {
                                Color c = animatableUIElements[i].rectTransform.GetComponent<Image>().color;
                                c.a = 0;
                                animatableUIElements[i].rectTransform.GetComponent<Image>().color = c;
                            }
                            break;
                        case UIAnimType.ScaleUp:
                            animatableUIElements[i].rectTransform.localScale = Vector3.zero;
                            break;
                    }
                }

                for (int i = 0; i < animatableUIElements.Length; i++)
                {
                    switch (animatableUIElements[i].animType)
                    {
                        case UIAnimType.MoveFromLeftToMiddle:
                        case UIAnimType.MoveFromRightToMiddle:
                        case UIAnimType.MoveFromTopToMiddle:
                        case UIAnimType.MoveFromBottomToMiddle:
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosInScreen, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease);
                            break;
                        case UIAnimType.FadeAnim:
                            if (fadeActive)
                            {
                                animatableUIElements[i].rectTransform.GetComponent<Image>().DOFade(animatableUIElements[i].endAlpha, gameController.activeGameConfig.elementMovementTime);
                            }
                            else
                            {
                                Color c = animatableUIElements[i].rectTransform.GetComponent<Image>().color;
                                c.a = animatableUIElements[i].endAlpha;
                                animatableUIElements[i].rectTransform.GetComponent<Image>().color = c;
                            }
                            break;
                        case UIAnimType.ScaleUp:
                            animatableUIElements[i].rectTransform.DOScale(animatableUIElements[i].endScale, gameController.activeGameConfig.elementMovementTime);
                            break;
                        case UIAnimType.ActivateAnimatorTriggerAtStart:
                        case UIAnimType.ActivateAnimatorTriggerAtStartAndEnd:
                            animatableUIElements[i].rectTransform.GetComponent<Animator>().SetTrigger(animatableUIElements[i].animatorStartTrigger);
                            break;
                    }
                    if (animateOnOpen)
                    {
                        yield return new WaitForSeconds(gameController.activeGameConfig.defaultDelayBetweenElements);
                    }
                }
            }
            else
            {
                for (int i = 0; i < animatableUIElements.Length; i++)
                {
                    switch (animatableUIElements[i].animType)
                    {
                        case UIAnimType.MoveFromLeftToMiddle:
                        case UIAnimType.MoveFromRightToMiddle:
                        case UIAnimType.MoveFromTopToMiddle:
                        case UIAnimType.MoveFromBottomToMiddle:
                            animatableUIElements[i].rectTransform.anchoredPosition = animatableUIElements[i].anchoredPosInScreen;
                            break;
                        case UIAnimType.FadeAnim:
                            if (fadeActive)
                            {
                                Color c = animatableUIElements[i].rectTransform.GetComponent<Image>().color;
                                c.a = animatableUIElements[i].endAlpha;
                                animatableUIElements[i].rectTransform.GetComponent<Image>().color = c;
                            }
                            break;
                        case UIAnimType.ScaleUp:
                            animatableUIElements[i].rectTransform.localScale =animatableUIElements[i].endScale;
                            break;
                    }
                }

            }

        }

        void ProceedToTheNextScreen()
        {
            if (nextDialogToBeOpenedWhenClosed != null)
            {
                nextDialogToBeOpenedWhenClosed.OpenDialog();
            }
            gameObject.SetActive(false);

        }

        public void SetNextDialogToBeOpen(GrimandaWindow grimandaWindow)
        {
            nextDialogToBeOpenedWhenClosed = grimandaWindow;
        }

        IEnumerator SetTheUIElementsOut()
        {
            for (int i = animatableUIElements.Length-1; i >-1 ; i--)
            {

                switch (animatableUIElements[i].animType)
                {
                    case UIAnimType.NoMovement:
                        if (i == 0)
                        {
                            OnCloseComplete();
                            ProceedToTheNextScreen();
                        }
                        break;
                    case UIAnimType.MoveFromLeftToMiddle:
                        if (i == 0)
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenRight, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease).OnComplete(() =>
                            {
                                OnCloseComplete();
                                ProceedToTheNextScreen();
                            });
                        }
                        else
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenRight, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease);
                        }
                        break;
                    case UIAnimType.MoveFromRightToMiddle:
                        if (i == 0)
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenLeft, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease).OnComplete(() =>
                            {
                                OnCloseComplete();
                                ProceedToTheNextScreen();
                            });
                        }
                        else
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenLeft, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease);
                        }
                        break;
                    case UIAnimType.MoveFromTopToMiddle:
                    case UIAnimType.MoveFromBottomToMiddle:
                        if (i == 0)
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenLeft, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease).OnComplete(() =>
                            {
                                OnCloseComplete();
                                ProceedToTheNextScreen();
                            });
                        }
                        else
                        {
                            animatableUIElements[i].rectTransform.DOAnchorPos(animatableUIElements[i].anchoredPosOutScreenLeft, gameController.activeGameConfig.elementMovementTime,true).SetEase(gameController.activeGameConfig.ease);
                        }
                        break;
                    case UIAnimType.FadeAnim:
                        if (i == 0)
                        {
                            if (fadeActive)
                            {
                                animatableUIElements[i].rectTransform.GetComponent<Image>().DOFade(0, gameController.activeGameConfig.elementMovementTime).OnComplete(() =>
                                {
                                    OnCloseComplete();
                                    ProceedToTheNextScreen();
                                });
                            }
                            else
                            {
                                OnCloseComplete();
                                ProceedToTheNextScreen();
                            }
                        }
                        else
                        {
                            if (fadeActive)
                            {
                                animatableUIElements[i].rectTransform.GetComponent<Image>().DOFade(0, gameController.activeGameConfig.elementMovementTime);
                            }
                        }
                        break;
                    case UIAnimType.ScaleUp:
                        if (i == 0)
                        {
                            animatableUIElements[i].rectTransform.DOScale(Vector3.zero, gameController.activeGameConfig.elementMovementTime).OnComplete(() =>
                            {
                                OnCloseComplete();
                                ProceedToTheNextScreen();
                            });
                        }
                        else
                        {
                            animatableUIElements[i].rectTransform.DOScale(Vector3.zero, gameController.activeGameConfig.elementMovementTime);
                        }
                        break;
                    case UIAnimType.ActivateAnimatorTriggerAtEnd:
                    case UIAnimType.ActivateAnimatorTriggerAtStartAndEnd:
                        animatableUIElements[i].rectTransform.GetComponent<Animator>().SetTrigger(animatableUIElements[i].animatorEndTrigger);
                        if (i == 0)
                        {
                            OnCloseComplete();
                            ProceedToTheNextScreen();
                        }
                        break;
                }
                yield return new WaitForSeconds(gameController.activeGameConfig.defaultDelayBetweenElements);
            }
        }

        public void SetFade(bool fadeStatus)
        {
            fadeActive = fadeStatus;
        }

        public void OpenDialog()
        {
            gameObject.SetActive(true);
            SetCloseAndopenAction(CloseActions.None);
            StartCoroutine(SetTheUIElementsIn());
            DoSpecificThingsAtOpen();
            OnWindowEnable.Invoke();
        }
    }
}
