using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helper class that stores all the coroutines
public static class GlobalCoroutineList
{
    private static List<Coroutine> all_coroutines = new List<Coroutine>();

    public static List<Coroutine> GetList() { return all_coroutines; }

    public static void Add(Coroutine cor) { all_coroutines.Add(cor); }
}
