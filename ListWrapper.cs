using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T>
{
    public ListWrapper(List<T> x)
    {
        list = x;
    }
    public List<T> list;
}
