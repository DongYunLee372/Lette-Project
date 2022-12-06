using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int ID { get { return _id; } set { _id = value; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } set { _name = value; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } set { _icon = value; } }

    public abstract Item2 CreateItem();
}
