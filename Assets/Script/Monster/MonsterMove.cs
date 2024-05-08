using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public Transform EndPoint;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Move();
    }

    private void Move()
    {
        if (EndPoint != null) 
        {
            agent.destination = EndPoint.position; 
        }
    }
}
