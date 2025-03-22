
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Item : MonoBehaviour
{
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        ItemModel = new ItemModel(name, Weight, id, type);
    }
    public ScriptableItem scriptableItem;

    public Material material;
    public float Weight => scriptableItem.Weight;

    public new string name;

    public string id;

    public string type;

    public ItemModel ItemModel;

    public bool added = false;

}

public struct ItemModel
{
    public string name;
    public float weight;
    public string id;
    public string type;
    public ItemModel(string name, float weight, string id, string type)
    {
        this.name = name;
        this.weight = weight;
        this.id = id;
        this.type = type;

    }
 
}


