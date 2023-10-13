using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="New TG Level")]
public class LevelTemplate : ScriptableObject
{
    public string levelName;
    public Sprite icon;
    public GameObject Nodes;
    public GameObject animModel;
    public Vector3 animPos;
    public Color backgroundColor;
}
