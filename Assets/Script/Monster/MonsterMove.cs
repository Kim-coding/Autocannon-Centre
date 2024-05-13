using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using static MonsterData;

public class MonsterMove : MonoBehaviour
{
    public Transform endPoint;
    private NavMeshAgent agent;
    private float threshold = 0.5f;

    private int id;

    private void Start()
    {
        id = int.Parse(name.Replace("(Clone)", ""));

        GameObject endPointObject = GameObject.FindWithTag("endPoint");
        if (endPointObject != null)
        {
            endPoint = endPointObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();

        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if(monsterTable != null ) 
        {
            var data = monsterTable.GetID(id);
            agent.speed = data.monsterSpeed;
        }
        
        Move();
    }

    private void Update()
    {
        if(endPoint != null && agent != null)
        {
            if(!agent.pathPending && agent.remainingDistance <= threshold)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("endPoint"))
        {
            //플레이어 체력 감소
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