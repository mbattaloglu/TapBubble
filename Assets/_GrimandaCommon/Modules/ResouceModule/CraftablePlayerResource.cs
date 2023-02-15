using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [System.Serializable]
    public struct neededResource
    {
        public PlayerResource resource;
        public int amount;
    }

    [CreateAssetMenu(fileName = "Craftable Resource", menuName = "Craftable Resource", order = 57)]
    public class CraftablePlayerResource : PlayerResource
    {
        public neededResource[] neededResources;
        public neededResource[] resourcesToBeGainedWhenDisposed;

    }
}
