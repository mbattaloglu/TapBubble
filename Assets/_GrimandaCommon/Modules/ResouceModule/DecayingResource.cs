using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "Decaying Resource", menuName = "Decaying Resource", order = 57)]
    public class DecayingResource : PlayerResource
    {
        public int DecayingTime;
    }
}
