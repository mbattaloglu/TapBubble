using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameObjectPool : MonoBehaviour
{
    public GameObject baseObject;
    public Transform root;
    public List<Transform> objects = new List<Transform>();

    public bool hideObject;
    public bool setParent = false;
    Transform returningTransform;

    public Transform GetPoolObject()
    {
//        Debug.LogError(objects.Count);
        if(objects.Count==0)
        {
            objects.Add(Instantiate(baseObject, root).transform);
        }

        returningTransform = objects[objects.Count-1];
        objects.RemoveAt(objects.Count - 1);

        if(hideObject)
        {
            returningTransform.gameObject.SetActive(true);
        }

        return returningTransform;
    }

    public void ReleaseObject(Transform poolObject)
    {
        if(setParent)
        {
            poolObject.SetParent(root);
        }

        if(hideObject)
        {
            poolObject.gameObject.SetActive(false);
        }

        objects.Add(poolObject);
    }

    public void ReleaseAll()
    {
        if (objects.Count > 0)
        {
            objects.RemoveRange(0, objects.Count - 1);
        }

        for(int i=0;i<root.childCount;i++)
        {
            ReleaseObject(root.GetChild(i));
        }
    }
}
