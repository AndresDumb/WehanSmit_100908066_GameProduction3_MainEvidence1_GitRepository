using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public GameObject item;
    public List<GameObject> itemList = new List<GameObject>();
    public Checkpoints_Position_Laps CheckpointsPositionLaps;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            SetLeaderboard SL = itemList[i].GetComponent<SetLeaderboard>();
            SL.SetDriverText(CheckpointsPositionLaps.Positions[i].GetComponent<PlayerInfo>().PlayerID);
            SL.SetPositionText((i + 1).ToString());
        }
    }

    
}
