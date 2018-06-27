using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IListExtensions
{
    /// <summary>
    /// Randomly shuffles the target list
    /// </summary>
    public static void Shuffle(this IList targetList)
    {
        for (int i = 0; i < targetList.Count - 1; i++)
        {
            int random = Random.Range(i, targetList.Count);
            object temp = targetList[i];
            targetList[i] = targetList[random];
            targetList[random] = temp;
        }
    }
}