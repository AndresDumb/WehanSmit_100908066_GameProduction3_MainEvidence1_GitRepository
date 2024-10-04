using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerInfo : MonoBehaviour
{
    public Checkpoints_Position_Laps checkpoints;
    public string PlayerID;
    public int LapCount = -1;
    public int CheckpointCount;

    public Checkpoint[] playerCheckpointList;
    public string MissedCheckpoint;
    
    public string RaceFinished;
    public float posValue = 0;
    
    public bool hasBeenChecked = false;
    public bool hasCopied = false;
    public bool hasFinished = false;
    public bool hasBeenUsed = false;
    public int lastCheckpoint;
    public float distanceDelta;
    public float distance;


    private void Awake()
    {
        
        MissedCheckpoint = $"{PlayerID}, Looks Like You Missed The Previous Checkpoint";
        RaceFinished = $"{PlayerID} Has Finished";
    }

    private void Update()
    {
        
        
        posValue = 0;
        if (checkpoints.goodToCopy  && !hasCopied)
        {
            playerCheckpointList = new Checkpoint[checkpoints.CheckpointList.Count];
            checkpoints.CheckpointList.CopyTo(playerCheckpointList, 0);

            hasCopied = true;


        }

        distance = Vector2.Distance(transform.position,
            playerCheckpointList[lastCheckpoint + 1].CheckPoint.transform.position);
        distanceDelta = playerCheckpointList[lastCheckpoint].DistanceToNextCheckpoint - distance;
        
        int i = 0;
        while (i <= CheckpointCount)
        {
            posValue += (100f / playerCheckpointList.Length);
        }

        i = 0;
        
            while (i <= LapCount)
            {
                posValue += 100f;
                i++;
            }
        

        

        

        posValue += (distanceDelta / playerCheckpointList[lastCheckpoint].DistanceToNextCheckpoint) * (100f / playerCheckpointList.Length);
    }
    public void CheckpointMissed()
    {
        checkpoints.OutputMaj.text = "";
        checkpoints.OutputMaj.text = checkpoints.Output.text + Environment.NewLine + MissedCheckpoint;
    }

    public void FinishedRace()
    {
        checkpoints.Output.text = checkpoints.Output.text + Environment.NewLine + RaceFinished;
    }
}
