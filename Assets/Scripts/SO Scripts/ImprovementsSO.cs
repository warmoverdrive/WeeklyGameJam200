using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Improvement Data", menuName = "ScriptableObjects/ImprovementData", order = 1)]
public class ImprovementsSO : ScriptableObject
{
    public string improvementName;
    [TextArea]
    public string improvementRules;
    public bool canGrow;
    public int growTime;
    public GameObject modelPrefab;
    public Color color;
}
