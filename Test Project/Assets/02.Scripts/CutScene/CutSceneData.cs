using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutScene", menuName = "Scriptable Object/CutSceneData")]
public class CutSceneData : ScriptableObject
{
    public enum CutSceneType
    {
        Opening,
        Chapter01,
        Chapter02,
        Chapter03,
        Chapter04
    }

    public CutSceneType cutSceneType;
    public Sprite[] frameSprites;
}