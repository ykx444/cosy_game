using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Crop")]
public class Crop : ScriptableObject
{
    public int timeToGrow;
    public Item yield;
    public int count = 1;

    public float seedDropRate;
    public float seedDropCount;

    public List<Sprite> sprites;
    public List<int> growStageTime;
}
