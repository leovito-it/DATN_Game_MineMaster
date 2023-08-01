using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NumberConfig",menuName ="Number/NumberConfig",order = 1)]
public class NumberConfig : ScriptableObject
{
    public GameObject deadEffect = null;
    public float scale = 1;
}
