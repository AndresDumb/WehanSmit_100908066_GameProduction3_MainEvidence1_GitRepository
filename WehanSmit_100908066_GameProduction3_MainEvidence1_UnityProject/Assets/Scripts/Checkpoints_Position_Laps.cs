using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Checkpoints_Position_Laps : MonoBehaviour
{
    

    public int numberOfLaps = 3;
    
    private float Maxfloat, Currentfloat;
    private int Maxint;
    public bool goodToCopy = false;
    public Text Output;
    public Text OutputMaj;
    

    public List<Checkpoint> CheckpointList = new List<Checkpoint>();

    public List<GameObject> Players = new List<GameObject>();
    public List<GameObject> Positions = new List<GameObject>();
    public List<GameObject> Finished = new List<GameObject>();

    public void OnSpawn(GameObject player)
    {
        
        Players.Add(player);
    }

    private void Update()
    {
        
        OrganiseRankings();
        
        for (int i = 0; i < CheckpointList.Count; i++)
        {
            CheckpointList[i].DistanceToNextCheckpoint = CalcDistance(i);
            
        }
        goodToCopy = true;


    }

    public string PlacementsString()
    {
        string Output = "";
        for (int i = 0; i < Positions.Count; i++)
        {
            Output = Output + i.ToString() + ". " + Positions[i] + Environment.NewLine;
        }

        return Output;
    }

    private float CalcDistance(int i)
    {
        if (i == CheckpointList.Count-1)
        {
            return Vector2.Distance(CheckpointList[i].CheckPoint.transform.position,
                CheckpointList[0].CheckPoint.transform.position);
        }
         
        
            return Vector2.Distance(CheckpointList[i].CheckPoint.transform.position,
                                    CheckpointList[i+1].CheckPoint.transform.position);
        
        

        
    }

     void OrganiseRankings()
     {
         
         Positions = new List<GameObject>();
         for (int j = 0; j < Players.Count; j++)
         {  
             
             Maxfloat = 0f;
            for (int i = 0; i < Players.Count; i++)
            {
                PlayerInfo player = Players[i].GetComponent<PlayerInfo>();
                Currentfloat = player.posValue;
                if (Maxfloat < Currentfloat && player.hasBeenChecked == false)
                {
                    Maxfloat = Currentfloat;
                    Maxint = i;
                }
                player.hasBeenChecked = true;
                
            }
            Players[Maxint].GetComponent<PlayerInfo>().hasBeenUsed = true;
            Positions.Add(Players[Maxint]);
            for (int i = 0; i < Players.Count; i++)
            { 
                if (!Players[i].GetComponent<PlayerInfo>().hasBeenUsed) 
                {
                    Players[i].GetComponent<PlayerInfo>().hasBeenChecked = false; 
                }
            }
            
         }
         
     }

    

}
