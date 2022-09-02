using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public enum State{
        isWalking, isChasing
    }
    public State status = State.isWalking;
    private NavMeshAgent agent;
    private Transform waypointTransform;
    public Transform playerTransform;
    public Transform groupTransform;

    public Transform centerEye;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetNewDestination();
        if(centerEye == null){
            Debug.LogError("You need to define the variable centerEye");
        }
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

    private bool DetectPlayer() {
        Vector3 direction = playerTransform.position - centerEye.position;
        direction = direction.normalized;
        float angle = Vector3.Angle(centerEye.forward, direction);

        if(angle <= 45) {
            RaycastHit raycasthit;
            Debug.DrawRay(centerEye.position, direction * 100, Color.red, 1f);

            if(Physics.Raycast(centerEye.position, direction, out raycasthit)) {
                Debug.Log($"Hit : {raycasthit.transform.name}");
                if(raycasthit.transform == playerTransform){
                    return true;
                }
            }
        }
        return false;
    }
    void UpdateWalking(){
        if(DetectPlayer()){
            status = State.isChasing;
        }    

    }
    void UpdateChasing(){
        agent.SetDestination(playerTransform.position);
        if(!DetectPlayer()){
            status = State.isWalking;   
            GetNewDestination();     
        }
    }
    void Update()
    {
        switch (status)
        {
            case State.isWalking: UpdateWalking(); break;
            case State.isChasing: UpdateChasing(); break;
        }
    }
}
