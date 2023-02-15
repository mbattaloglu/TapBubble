using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grimanda.Common
{
    [CreateAssetMenu(fileName = "Time Renewing Resource", menuName = "Time Renewing Resource", order = 57)]
    public class TimeRenewingResource : PlayerResource
    {
        public int renewTimeInSeconds;

        public int RenewTimeInSeconds
        {
            get
            {
                return renewTimeInSeconds + PlayerDataUnity.GetInt("EnergyFillTimeCut", 0);
            }
        }


    }
}
