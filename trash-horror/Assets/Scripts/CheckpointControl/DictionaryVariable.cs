using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DictionaryVariable", menuName = "Scriptable Objects/DictionaryVariable")]
public class DictionaryVariable : ScriptableObject
{
    public Dictionary<string, Dictionary<string, string>> value = new();
}