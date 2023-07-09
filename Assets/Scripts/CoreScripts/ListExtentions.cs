using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtentions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = UnityEngine.Random.Range(0, n + 1);
            list.Swap(k, n);
        }
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
}
