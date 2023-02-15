using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "New Avatar", menuName = "Avatar", order = 51)]
    public class Avatar : CraftablePlayerResource
    {
        public GameObject prefab;
        public bool ownedByDefault;
    }
}
