using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemParameterData : ScriptableObject
{
    [field : SerializeField]
    public string ParameterName { get; set; }   
    
}
