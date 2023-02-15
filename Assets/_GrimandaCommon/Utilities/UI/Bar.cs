using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bar : MonoBehaviour
{
    public RectTransform Empty;
    public RectTransform Full;
    public GameObject bar;
    public enum BarType { Type1=1,Type2=2};
    public BarType barType;

    public void updateBar(int CurrentValue, int MaximumValue,bool Animate=false)
    {
        if (barType == BarType.Type1)
        {
            if (!Animate)
            {
       //         Debug.LogError("BAR UPDATE Type 1");
                float BarSize = Full.localPosition.x - Empty.localPosition.x;
                Vector3 pos = Empty.localPosition;
             /*   Debug.Log(BarSize);
                Debug.Log(Empty.localPosition);
                Debug.Log(Full.localPosition);
                Debug.Log((float)CurrentValue / MaximumValue);*/
                pos.x += BarSize * ((float)CurrentValue / MaximumValue);
                bar.GetComponent<RectTransform>().localPosition = pos;
            }
            else
            {

            }
        }
        else
        {
//            Debug.LogError("BAR UPDATE Type 2");
            float BarSize = Full.anchorMax.x;
//            Debug.Log(Full.right.x + " ----- " + Empty.right.x);
            Vector3 pos = bar.GetComponent<RectTransform>().anchorMax;
/*            Debug.Log(Full.sizeDelta);
            Debug.Log(Empty.localPosition);
            Debug.Log(Full.localPosition);
            Debug.Log((float)CurrentValue / MaximumValue);*/
            bar.GetComponent<RectTransform>().anchorMax = Empty.anchorMax;
            pos.x = BarSize * ((float)CurrentValue / MaximumValue);
            bar.GetComponent<RectTransform>().DOAnchorMax(pos,1.0f);
            //bar.GetComponent<RectTransform>().rect.Set(pos.x,pos.y,Size, bar.GetComponent<RectTransform>().rect.height);
        }
    }
}
