using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = " ScriptableItem", menuName = "ScriptableObjects/SpawnItemScriptableObject", order = 1)]
public class ScriptableItem : ScriptableObject
{
    [SerializeField] private float weight;
    [SerializeField] private GameObject prefab;
    public float Weight => weight;
    public GameObject Prefab => prefab;


}
