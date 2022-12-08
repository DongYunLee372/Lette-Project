using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data", order = 1)]

public class SkillData : ScriptableObject
{
    [SerializeField]
    private string _name;
    public string Name  { get { return _name; } set { _name = value; } }

    [SerializeField]
    private AnimationClip _clip;
    public AnimationClip Clip { get { return _clip; } set { _clip = value; } }

    [SerializeField]
    private float _damege;
    public float Damege { get { return _damege; } set { _damege = value; } }

    [SerializeField]
    private GameObject _effect;
    public GameObject Effect { get { return _effect; } set { _effect = value; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } set { _icon = value; } }
}
