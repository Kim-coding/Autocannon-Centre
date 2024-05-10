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

    private void Start()
    {
        GameObject endPointObject = GameObject.FindWithTag("endPoint");
        if (endPointObject != null)
        {
            endPoint = endPointObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();

        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        foreach (var kvp in monsterTable.monsterTable)
        {
            MonsterData monster = kvp.Value;
            if(monster.monsterName.ToString() == this.name.Replace("(Clone)", ""))
                agent.speed = monster.monsterSpeed;
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
                //gameObject.SetActive(false);  // 오브젝트 풀링을 위함
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