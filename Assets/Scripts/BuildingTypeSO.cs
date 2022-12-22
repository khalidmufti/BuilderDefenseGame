using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Building Type")]
public class BuildingTypeSO : ScriptableObject
{
    public string BuildingName;
    public Transform Prefab;
    public bool isGenerateResource;
    public ResourceGeneratorData ResourceGeneratorData;
    public Sprite Sprite;
    public float MinConstructionRadius;
    public ResourceAmount[] ConstructionResourceCostArray;
    public int HealthAmountMax;
    public float ConstructionTimerMax;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in ConstructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.ResourceType.colorHex + ">" + resourceAmount.ResourceType.ResourceNameShort + " " + resourceAmount.Amount + "</color> ";
        }
        return str;
    }
}
