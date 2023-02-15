using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Utilities : MonoBehaviour
{
    //Sensitivity is 100 which means it reads the 2 digits after floating point
    static int[] DropProbabilities;
    static int[] DropProbabilitySingle= new int[100];

    static int[] ExcludeElementFromIntegerArray(int[] _array,int ElementToBeExclude)
    {
        int[] NewArray = new int[_array.Length - 1];

        for(int i=0;i<ElementToBeExclude;i++)
        {
            NewArray[i] = _array[i];
        }
        for (int i = ElementToBeExclude+1;i<_array.Length;i++)
        {
            NewArray[i] = _array[i];
        }
        return NewArray;
    }

    static int CalculateDropRates(Dictionary<int,float> _DropRates)
    {
        int counter = 0;
        for (int i = 0; i < _DropRates.Count; i++)
        {
            int dropRateAmount = (int)_DropRates[i] * 100;
            for (int j = 0; j < dropRateAmount; j++)
            {
                DropProbabilities[counter] = i;
                counter++;
            }
        }

        return counter;
    }

    public static void changeUISprite(GameObject image,Sprite sprite)
    {
        image.GetComponent<Image>().sprite = sprite;
    }
    public static void changeTMPText(GameObject tmpText, string text)
    {
        tmpText.GetComponent<TextMeshProUGUI>().text = text;
    }
    // Returns the amount of drop
    // Sensitivity is 2 digits after floating point
    static int ifDrop(float Probability,int maxAmount, int minAmount,bool Drop=false)
    {
        /*for (int i = 0; i < (int)Probability * 100; i++)
        {
            DropProbabilitySingle[i] = 1;
        }

        for (int i = (int)Probability * 100;i<100; i++)
        {
            DropProbabilitySingle[i] = 0;
        }*/
        if (Drop)
        {
            return Random.Range(minAmount, maxAmount);
        }
        else
        { 
            if (Random.Range(0, 99) > (int)Probability * 100)
            {
                return Random.Range(minAmount, maxAmount);
            }
            else
            {
                return 0;
            }
        }
    }


    // Grimanda Documentation: Hakan Yuksel
    // This function returns Amount of drop with keys, keys are itemIds in the gameController
    // For Now Amounts are only 1
    // DropCountProbabilities are the item count drop rates: For example %25 for first item drop, %10 for second drop and %5 for third drop
    // The minimum drop count is guaranteed from start
    // If the drop count is bigger than maximum drop count and maximum drop count exits than
    // Whole probabilities multililed with 100 to increase the sensitivity by 2 digits after floating point
    static public Dictionary<int,int> DropCalculation(Dictionary<int,float>_DropRates,float[] DropCountProbabilitiesFromGameplay, int MaximumDropCount = 99999, int MinimumDropCount = 0)
    {

        // Determinind how many items dropped
        int dropCount = MinimumDropCount;
        Debug.LogError(" DropCountProbabilitiesFromGameplay :" + DropCountProbabilitiesFromGameplay.Length);
        for (int i = MinimumDropCount; i < DropCountProbabilitiesFromGameplay.Length; i++)
        {
            if (Random.Range(0, 10000)<(int)DropCountProbabilitiesFromGameplay[i]*100)
            {
                Debug.LogError(" DropCountProbabilitiesFromGameplay :" + DropCountProbabilitiesFromGameplay[i]);
                dropCount++;
            }
        }

        if(dropCount>MaximumDropCount)
        {
            dropCount = MaximumDropCount;
        }

        // Filling the probability array
        // Determining the dimention of DropprobabilitiesArray
        Dictionary<int,int> Drops =new Dictionary<int, int>();
        Debug.LogError("Drop Rates Length:" + _DropRates.Count);
        int[] Keys = new int[_DropRates.Count];
        int[] Values = new int[_DropRates.Count];
        int ValuesSum = 0;
        int counter = 0;

        foreach (KeyValuePair<int, float> item in _DropRates)
        {
            Keys[counter] = item.Key;
            Values[counter] = (int)item.Value*100;
            ValuesSum += Values[counter];
            Debug.LogError("Item Id:" + Keys[counter] + " "  + " Drop Rate:" + Values[counter]);

            counter++;
        }

        Debug.LogError("Keys Length " + Keys.Length);
        if(Keys.Length<dropCount)
        {
            dropCount = Keys.Length - 1;
        }

        // Filling the dropprobability array with item IDs
        DropProbabilities = new int[ValuesSum];
        counter = 0;
        for(int i=0;i<Keys.Length;i++)
        {
            for(int j=0;j<Values[i];j++)
            {
                DropProbabilities[counter] = Keys[i];
                counter++;
            }
        }


        Debug.LogError("Drop Count:" + dropCount);
        int range = DropProbabilities.Length-1;
        for (int i=0;i<dropCount;i++)
        {
            int DropIndex = Random.Range(0, range);
            Debug.LogError("Range:" + range);
            Debug.LogError("Drop Index:"+DropIndex);
            int ItemId = DropProbabilities[DropIndex];
            Debug.LogError("Drop Id:" + ItemId);
            Drops.Add(ItemId, 1);

            int forwardCounter = DropIndex;
            int backwardCounter = DropIndex - 1;
            bool backwardFinished = false, forwardFinished = false;
            do
            {
                if (forwardCounter < DropProbabilities.Length)
                {
                    if(DropProbabilities[forwardCounter]!=ItemId)
                    {
                        forwardFinished = true;
                    }
                    DropProbabilities[forwardCounter] = DropProbabilities[range];
                    range--;
                    forwardCounter++;
                }
                else
                {
                    forwardFinished = true;
                }

                if (backwardCounter >0)
                {

                    if (DropProbabilities[backwardCounter] != ItemId)
                    {
                        backwardFinished = true;
                    }

                    DropProbabilities[backwardCounter] = DropProbabilities[range];
                    range--;
                    backwardCounter--;
                }
                else
                {
                    backwardFinished = true;
                }
                if(backwardFinished && forwardFinished)
                {
                    break;
                }

                if (range <= 0)
                {
                    break;
                }
            } while (true);


            //DropIndex = start+1;

        }

     /*   do
        {
            int ProbabilityCount=CalculateDropRates(dropRates);

            int DroppedItem = DropProbabilities[Random.Range(0, ProbabilityCount)];
            // TODO amount Can be add here instead of "1" later
            Drops.Add(DroppedItem, 1);

            dropRates.Remove(DroppedItem);
            //ExcludeElementFromIntegerArray(dropRates, Drops[dropCount - 1]);
            dropCount--;
        } while (dropCount > 0);*/

        return (Drops);
    }
}
