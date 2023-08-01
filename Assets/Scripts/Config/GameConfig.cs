using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="GameConfig", menuName = "Game/Config", order =1)]
public class GameConfig : ScriptableObject
{
    public AudioClip clipTrueClick;
    public AudioClip clipFalseClick;
}
