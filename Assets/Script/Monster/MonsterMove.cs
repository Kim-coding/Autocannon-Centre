using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public Transform endPoint;
    private NavMeshAgent agent;
    private float threshold = 0.5f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Move();
    }

    private void Update()
    {
        if(endPoint != null && agent != null)
        {
            if(!agent.pathPending && agent.remainingDistance <= threshold)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Move()
    {
        if (endPoint != null) 
        {
            agent.destination = endPoint.position; 
        }
    }
}