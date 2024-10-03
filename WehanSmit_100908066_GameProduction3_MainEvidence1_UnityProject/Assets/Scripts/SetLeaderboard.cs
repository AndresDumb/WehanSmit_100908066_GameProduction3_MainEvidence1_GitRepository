using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLeaderboard : MonoBehaviour
{
    public Text position;
    public Text Driver;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPositionText(string newPosition)
    {
        position.text = newPosition;
    }
    
    public void SetDriverText(string newDriver)
    {
        Driver.text = newDriver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
