using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform waypointTransform;
    public Transform groupTransform;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetNewDestination();
    }
    void GetNewDestination()
    {
        Transform currentDestination = waypointTransform;
        
        do
        {
            int index = Random.Range(0, groupTransform.childCount);      //Random.Range(0,3) : 3 car 3 enfants
            waypointTransform = groupTransform.GetChild(index);         //waypointTransform = groupTransform.GetChild(Random.Range(0,3));
        } while (waypointTransform == currentDestination);
        
        agent.SetDestination(waypointTransform.position); 
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform == waypointTransform){
            GetNewDestination();
        }
    }
    void Update()
    {
        
    }
}
