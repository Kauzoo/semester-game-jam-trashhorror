using UnityEngine;

[CreateAssetMenu(fileName = "HealthVariable", menuName = "Scriptable Objects/HealthVariable")]
public class HealthVariable : ScriptableObject
{
    public float current;
    public float max;
}