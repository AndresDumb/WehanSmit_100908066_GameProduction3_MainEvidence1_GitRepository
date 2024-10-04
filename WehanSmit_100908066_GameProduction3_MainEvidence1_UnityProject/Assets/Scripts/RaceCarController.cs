using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaceCarController : MonoBehaviour
{

    public PlayerInfo playerInfo;
   
    public bool OnTrack;
    public int PlayerID = 1;

    public Checkpoints_Position_Laps checkpoints;
    public float Driftvar = 0.95f;
    public float acceleration = 30f;
    public float ORacceleration = 15f;
    public float ORmaxSpeed = 7f;
    public float maxSpeed = 10f;
    public float TurnRad = 3.5f;
    private float accelInput = 0;
    private float SteerInput = 0;
    private float rotateAngle = 0;
    private float velocityVsUp = 0;
    public bool wasAdded = false;
    
    public Rigidbody2D rb;
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        
        
        
        
        playerInfo = GetComponent<PlayerInfo>();
        
        
        
        

    }

    

    public float GetVelocityMagnitude()
    {
        return Vector2.Dot(transform.up, rb.velocity);
    }
    

    

    // Update is called once per frame
    void FixedUpdate()
    {
        
        ApplyForce();
        
        killOrthoVel();

        ApplySteering();
    }
    
    private void Update()
    {
        Vector2 input = new Vector2();
        switch (PlayerID)
        {
            case 1:
                input = new Vector2(Input.GetAxis("Horizontal_P1"), Input.GetAxis("Vertical_P1"));
                break;
            case 2:
                input = new Vector2(Input.GetAxis("Horizontal_P2"), Input.GetAxis("Vertical_P2"));
                break;
            case 3:
                input = new Vector2(Input.GetAxis("Horizontal_P3"), Input.GetAxis("Vertical_P3"));
                break;
            case 4:
                input = new Vector2(Input.GetAxis("Horizontal_P4"), Input.GetAxis("Vertical_P4"));
                break;
        }
                SetVector(input);
       if (playerInfo.LapCount >= checkpoints.numberOfLaps && !playerInfo.hasFinished)
       {
           playerInfo.FinishedRace();
           playerInfo.hasFinished = true;
           gameObject.SetActive(false);
           
       }
    }

    void ApplyForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        if (velocityVsUp > maxSpeed && accelInput > 0 && OnTrack)
        {
            return;
        }
        if (velocityVsUp < -maxSpeed * 0.5f && accelInput < 0 && OnTrack)
        {
            return;
        }

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelInput > 0 && OnTrack)
        {
            return;
        }
        if (velocityVsUp > ORmaxSpeed && accelInput > 0 && !OnTrack)
        {
            return;
        }
        if (velocityVsUp < -ORmaxSpeed * 0.5f && accelInput < 0 && !OnTrack)
        {
            return;
        }

        if (rb.velocity.sqrMagnitude > ORmaxSpeed * maxSpeed && accelInput > 0 && !OnTrack)
        {
            return;
        }
        if (accelInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3f, Time.fixedDeltaTime * 3);
        }
        else
        {
            rb.drag = 0;
        }

        if (OnTrack)
        {
           Vector2 ForceVec = transform.up * accelInput * acceleration; 
           rb.AddForce(ForceVec, ForceMode2D.Force);
        }
        if (OnTrack)
        {
            Vector2 ForceVec = transform.up * accelInput * ORacceleration; 
            rb.AddForce(ForceVec, ForceMode2D.Force);
        }
        
        
        
    }

    public float GetLatVelocity()
    {
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public bool isScreeching(out float latVel, out bool braking)
    {
        latVel = GetLatVelocity();
        braking = false;
        if (accelInput < 0 && velocityVsUp > 0)
        {
            braking = true;
            return true;
        }

        if (Mathf.Abs(GetLatVelocity()) > 4f)
        {
            return true;
        }

        return false;
    }

    private void SetVector(Vector2 input)
    {
        SteerInput = input.x;
        accelInput = input.y;
    }

    

    private void ApplySteering()
    {
        float minSpeed = rb.velocity.magnitude / 8;
        minSpeed = Mathf.Clamp01(minSpeed);
        rotateAngle -= SteerInput * TurnRad * minSpeed;
        rb.MoveRotation(rotateAngle);
    }

    private void killOrthoVel()
    {
        Vector2 forVel = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVel = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forVel + rightVel * Driftvar;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Track"))
        {
            OnTrack = true;
        }

        if (other.CompareTag("NotTrack"))
        {
            OnTrack = false;
        }
        if (other.CompareTag("Checkpoint"))
        {
            CheckpointID IDcheck = other.GetComponent<CheckpointID>();
            
            if (IDcheck.ID >= 1 && playerInfo.playerCheckpointList[IDcheck.ID-1].hasPassedThrough)
            {
                playerInfo.playerCheckpointList[IDcheck.ID].hasPassedThrough = true;
                playerInfo.playerCheckpointList[IDcheck.ID-1].hasPassedThrough = false;
                
                
                playerInfo.CheckpointCount++;
                if (playerInfo.playerCheckpointList[IDcheck.ID].isFinish)
                {
                    playerInfo.LapCount++;
                    playerInfo.CheckpointCount = 0;
                }

                playerInfo.lastCheckpoint = IDcheck.ID;
            }
            else if (IDcheck.ID == 0 && playerInfo.playerCheckpointList[^1].hasPassedThrough)
            {
                playerInfo.playerCheckpointList[IDcheck.ID].hasPassedThrough = true;
                playerInfo.playerCheckpointList[^1].hasPassedThrough = false;
                
                playerInfo.CheckpointCount++;
                if (playerInfo.playerCheckpointList[IDcheck.ID].isFinish)
                {
                    playerInfo.LapCount++;
                }

                playerInfo.lastCheckpoint = IDcheck.ID;
            }
            else
            {
                playerInfo.CheckpointMissed();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Track"))
        {
            OnTrack = false;
        }

        if (other.CompareTag("NotTrack"))
        {
            OnTrack = true;
        }
    }
}
