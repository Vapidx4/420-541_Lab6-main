using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretComponent : MonoBehaviour
{
    public GameObject particleSystemToSpawn;
    public GameObject player;
    public Slider healthBar;
    public Slider awarenessBar;
    public Text awarenessText;
    public Text healthText;
    public Vector3 initialForwardVector;
    public Vector3 playerDirection;
    public float maxAngle = 45;
    public float maxDistance = 100;
    public float health = 100;

    public float awarnessTimer = 0.0f;
    public float fullAwarnessTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        initialForwardVector = transform.forward;  
        player = GameObject.FindGameObjectWithTag("Player");
        healthBar.value = health;
        healthText.text = health.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
        UpdateTurretRotation();
        UpdateTurretAwareness(SeePlayer());
               
    }
    
    public void UpdateTurretAwareness(bool seePlayer)
    {
       awarnessTimer = (seePlayer)?awarnessTimer + Time.deltaTime : awarnessTimer - Time.deltaTime;
       float awarenessRatio = Mathf.Clamp(awarnessTimer / fullAwarnessTime, 0.0f, 1.0f);
       awarenessBar.value = awarenessRatio;
       if (awarnessTimer >= fullAwarnessTime)
       {
           Debug.Log("I see you!");
           awarenessText.text = "I see you!";

         awarenessText.enabled = true;
            // TO DO UPDATE ()
       }
       else
       {
         awarenessText.enabled = false ;
       }


    }
    public void ProcessHit()
    {
        health -= 10;
        healthBar.value = health;
        healthText.text = health.ToString();
        

        if (healthBar.value <= 0 )
        {
            Destroy(gameObject);
        }
    }
    public void UpdateTurretRotation()
    {
        if (SeePlayer())
        {
            playerDirection = new Vector3(playerDirection.x,0,playerDirection.z);
            transform.LookAt(player.transform.position + playerDirection);
        }

    }
    public bool SeePlayer()
    {
        playerDirection = player.transform.position - transform.position;
        if (playerDirection.magnitude < maxDistance)
        {
            Vector3 normPlayerDirection = Vector3.Normalize(playerDirection);
            float dotProduct = Vector3.Dot(initialForwardVector, normPlayerDirection);
            var angle = Mathf.Acos(dotProduct);
            float deg = angle * Mathf.Rad2Deg;
            if (deg < maxAngle)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, normPlayerDirection);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //if the player is seen
                        return true;

                    }
                }
            } 
        }
        return false;
       
    }
}