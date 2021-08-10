using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtender
{
    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }
}



