using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Resource Type")]
public class ResourceTypeSO : ScriptableObject
{
    public string ResourceName;
    public string ResourceNameShort;
    public Sprite Sprite;
    public string colorHex;
}
