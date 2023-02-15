using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Grimanda.Common
{
    public class CommonGameConfig : ScriptableObject
    {
        [Header("Common Game Settings")]
        public Grimanda.Common.Avatar[] avatars;
        public GameObject specialMainMenuPrefab;
        public GameObject specialAvatarIcon;
        public Vector3 avatarScale;
        public Vector3 avatarOffset;
        public float defaultDelayBetweenElements;
        public float elementMovementTime;
        public Ease ease;

    }
}
