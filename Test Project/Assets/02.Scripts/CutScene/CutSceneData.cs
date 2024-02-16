using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutScene", menuName = "Scriptable Object/CutSceneData")]
public class CutSceneData : ScriptableObject
{
    public enum CutSceneType
    {
        Opening = 0,
        Chapter01 = 1,
        Chapter02 = 2,
        Chapter03 = 3,
        Chapter04 = 4
    }

    public CutSceneType cutSceneType;
    public Sprite[] frameSprites;
    public AudioClip[] sfxClips;
}