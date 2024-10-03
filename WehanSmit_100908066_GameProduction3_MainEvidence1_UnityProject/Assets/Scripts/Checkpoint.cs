using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Checkpoint
{
    public int id;
    public GameObject CheckPoint;
    public bool isFinish;
    public bool hasPassedThrough;
    public float DistanceToNextCheckpoint;
}   
