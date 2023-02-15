using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Grimanda.Common
{
    public class TMPROAmountIncreaseAnimated : MonoBehaviour
    {
        TextMeshProUGUI tmpText;
        public bool isMaxMin;
        public PlayerResource playerResource;


        private void Start()
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }
        /// <summary>
        /// This changes the value of a TextMeshPro Text with animation
        /// </summary>
        /// <param name="startingValue"></param>
        /// <param name="endValue"></param>
        /// <param name="incrementTime"></param>
        /// <param name="incrementcount"> This indicates how many increments will there be</param>
        /// <returns></returns>
        public IEnumerator startTextCounting(int startingValue, int endValue, float incrementTime = 1, int incrementcount = 20)
        {
            Debug.LogError("Increasing TMP value" + startingValue + " to " + endValue);
            if (endValue - startingValue > 0)
            {
                if (incrementcount > endValue - startingValue)
                {
                    incrementcount = endValue - startingValue;
                }
            }
            else
            {
                if (incrementcount > startingValue - endValue)
                {
                    incrementcount = startingValue - endValue;
                }
            }
            int increment = (endValue - startingValue) / incrementcount;

            for (int i = 1; i < incrementcount + 1; i++)
            {
                if (isMaxMin)
                {
                    Debug.LogError("XXXXXXX-------" + startingValue + (i * increment));
                    tmpText.text = (startingValue + (i * increment)).ToString() + "/" + playerResource.maximumAmount.ToString();
                }
                else
                {
                    tmpText.text = (startingValue + (i * increment)).ToString();
                }
                yield return new WaitForSeconds((float)incrementTime / incrementcount);
            }
        }
    }
}