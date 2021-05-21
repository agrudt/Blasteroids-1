using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    private enum PlanetState
    {
        normalState,
        hitState,
        invincibleState,
        fireState,
    };

    public int planetHealth = 4;
    private PlanetState pState = PlanetState.normalState;
    public float invTime = 4000f;
    public float hurtTime = 30f;
    private int stateFrameTick = 0;
    public LaserStatSystem mLaserStat = null;
    public float fireTime = 90f;
    public int shots = 0;
    public int shotsTot = 4;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateFSM();
        transform.Rotate(Vector3.forward, -0.01f);
       // ProcessLaserSpwan();

        //This is just to test the invincible state, delete later
        if (Input.GetKeyDown(KeyCode.I))
        {
            stateFrameTick = 0;
            pState = PlanetState.invincibleState;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            pState = PlanetState.normalState;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pState = PlanetState.fireState;
        }
    }


    //Deals with asteroid collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Emeny OnTriggerEnter");
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            //print("collided with" + collision.gameObject.name);
            Destroy(collision.gameObject);
            if(pState != PlanetState.invincibleState)
            {
                planetHealth--;
                if (planetHealth == 0)
                {
                    Destroy(gameObject);
                }
                planetHit();
                GlobalBehavior.sTheGlobalBehavior.UpdatePlanetHealth("Planet Health: " + planetHealth);
            }
            
        }
    }

    void planetHit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        pState = PlanetState.hitState;
        /*if (1 == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }*/
    }

    public void addPlanetHealth()
    {
        planetHealth++;
        GlobalBehavior.sTheGlobalBehavior.UpdatePlanetHealth("Planet Health: " + planetHealth);

    }

    private void updateFSM()
    {
        switch (pState)
        {
            case PlanetState.normalState:
                ServiceNormalState();
                break;
            case PlanetState.invincibleState:
                ServiceInvincibleState();
                break;
            case PlanetState.hitState:
                ServiceHitState();
                break;
            case PlanetState.fireState:
                ServiceFireState();
                break;
        }
    }

    private void ServiceNormalState()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void ServiceInvincibleState()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        if (stateFrameTick > invTime)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            stateFrameTick = 0;
            pState = PlanetState.normalState;
        }
        else
        {
            stateFrameTick++;
        }
    }
    private void ServiceHitState()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        if (stateFrameTick > hurtTime)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            stateFrameTick = 0;
            pState = PlanetState.normalState;
        }
        else
        {
            stateFrameTick++;
        }
    }

    private void ServiceFireState()
    {
        if (stateFrameTick > fireTime)
        {
            if (shotsTot > shots)
            {
                stateFrameTick = 0;
                shots++;
                ProcessLaserSpwan();
            }
            else
            {
                ProcessLaserSpwan();
                stateFrameTick = 0;
                shots = 0;
                pState = PlanetState.normalState;
            }
            
        }
        else
        {
            stateFrameTick++;
        }
    }


    private void ProcessLaserSpwan()
    {

       for (int i = -100; i <= 100; i += 5)
        {
            mLaserStat.SpawnLaser(new Vector3(i, -60), new Vector3(i, 300));
        }
       
    }
}