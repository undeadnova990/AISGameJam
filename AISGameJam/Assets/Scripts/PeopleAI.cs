using UnityEngine;
using UnityEngine.AI;

public class PeopleAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public enum PeopleState { Navigate, Idle, Die}

    [Header("General Settings")]
    public PeopleState currentPersonState = PeopleState.Idle;
    public Transform exitPosition;
    public string exitPositionTag = "ExitPlatform";
    public string deathZoneTag = "DeathZone";
    public GameObject comicalDeathEffectPrefab;

    private bool aliveState = true;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!exitPosition)
        {
            exitPosition = GameObject.FindGameObjectWithTag(exitPositionTag).transform;

            if (!exitPosition)
            {
                Debug.Log("There is no exit, They are Trapped Forever :<");
                return;
            }
            
        }
        agent.isStopped = true;
        agent.SetDestination(exitPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentPersonState)
        {
            case PeopleState.Navigate:
                // Activates the NavMeshAgent Script
                Navigate();
                break;
            case PeopleState.Idle:
                // Deactivates the NavMeshAgentScript; Makes them stand still
                Idle();
                break;
            case PeopleState.Die:
                // Dies/Lose State for the player
                Die();
                break;
        }
    }

    void Navigate()
    {
        /*
        Moves the person to the end of the level/Activates the NavMeshAgent

        */
        if(agent.isStopped == true) agent.isStopped = false;
        //Debug.Log(exitPosition.position);

    }

    void Idle()
    {
        // Stops the agent from moving at the start of the game and when they touch the exitPosition
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

    }

    void Die()
    {
        if (aliveState) return; // if its alive, then do nothing

        /*Once the Person touches the death pit:
        Game OVER - Lose State
        Maybe a comical explosion 
        Stop Every Person
        */

        Debug.Log($"{gameObject.name} died :<");
        agent.isStopped = true;

        if (comicalDeathEffectPrefab)
        {
            Instantiate(comicalDeathEffectPrefab, transform.position, transform.rotation);
        }

        aliveState = false;

        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag(deathZoneTag))
        {
            currentPersonState = PeopleState.Die;
        }
        if(collision.transform.CompareTag(exitPositionTag))
        {
            Debug.Log("End Reached");
            currentPersonState = PeopleState.Idle;
            Destroy(gameObject);
            Debug.Log("Made It Out Safe");
        }
    }

    // Call this function to freeze all agents in place
    public void StopAllAgents()
    {
        Debug.Log("Stop Everyone");
        // Locates every NavMeshAgent currently active in the scene
        NavMeshAgent[] allAgents = Object.FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in allAgents)
        {
            // Pauses the agent without clearing its current path data
            agent.isStopped = true; 
            
            // Optional: Eliminates sliding by instantly removing momentum
            agent.velocity = Vector3.zero; 
        }
    }

    public void SetAllToIdle()
    {
        Debug.Log("Begin Idoling");
        PeopleAI[] allPeople = FindObjectsByType<PeopleAI>(FindObjectsSortMode.None);

        foreach (PeopleAI people in allPeople)
        {
            people.currentPersonState = PeopleState.Idle;
        }
    }
}
