using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign")]
public class LevelDesign : ScriptableObject
{
    [SerializeField] public GameObject levelPrefab;
    [SerializeField] public Sprite paddleSprite;
    [SerializeField] public Sprite levelBackground;
}
