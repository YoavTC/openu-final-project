using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Selectable Information Object")]
public class SelectableInformationObject : ScriptableObject
{
    public string informationName;
    public string informationDescription;
}