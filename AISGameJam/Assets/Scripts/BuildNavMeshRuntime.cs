using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static PeopleAI;

public class BuildNavMeshRuntime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private NavMeshSurface navMeshSurface;
    [SerializeField]
    private PeopleAI personAI;
    void Awake()
    {
        // Needs a NavMeshSurface component
        navMeshSurface = GetComponent<NavMeshSurface>();
        personAI = GetComponent<PeopleAI>();

        //BuildDynamicNavMesh();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            BuildDynamicNavMesh();

            ResumeAllAgents();
            SetAllToNavigate();
        }
    }
    public void BuildDynamicNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void ResumeAllAgents()
    {
        Debug.Log("Run Everyone");
        NavMeshAgent[] allAgents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in allAgents)
        {
            // Resumes movement along their existing paths
            agent.isStopped = false; 
        }
    }

    public void SetAllToNavigate()
    {
        Debug.Log("Begin Navigating");
        PeopleAI[] allPeople = FindObjectsByType<PeopleAI>(FindObjectsSortMode.None);

        foreach (PeopleAI people in allPeople)
        {
            people.currentPersonState = PeopleState.Navigate;
        }
    }

}
