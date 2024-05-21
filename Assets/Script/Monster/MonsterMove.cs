using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    private int currentWayPointIndex = 0;

    public Transform endPoint;
    private float threshold = 0.8f;
    public float speed;
    private float rotSpeed = 5f;

    private int id;

    private void Start()
    {
        id = int.Parse(name.Replace("(Clone)", ""));

        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if(monsterTable != null ) 
        {
            var data = monsterTable.GetID(id);
            speed = data.monsterSpeed;
        }
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("endPoint"))
        {
            
        }
    }

    private void Move()
    {
        if(currentWayPointIndex < wayPoints.Count - 1)
        {
            Transform targetWayPoint = wayPoints[currentWayPointIndex];
            MoveTarget(targetWayPoint);
            
            if (Vector3.Distance(transform.position, targetWayPoint.position) <= threshold)
            {
                currentWayPointIndex++;
            }
        }
        else
        {
            MoveTarget(endPoint);

            if (Vector3.Distance(transform.position, endPoint.position) <= threshold)
            {
                PoolManager.instance.ReturnObjectToPool(gameObject);
                GameManager.Instance.SubHealth(10);
                if (GameManager.Instance.health <= 0)
                {
                    GameManager.Instance.EndGame();
                }
            }
        }
    }

    private void MoveTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotSpeed);
        }
    }

    public void SetWayPoints(Transform wayPointContainer)
    {
        wayPoints.Clear();
        foreach(Transform wayPoint in wayPointContainer)
        {
            wayPoints.Add(wayPoint);
        }

        GameObject endPointObject = GameObject.FindWithTag("endPoint");
        if(endPointObject != null) 
        {
            endPoint = endPointObject.transform;
        }
        currentWayPointIndex = 0;
    }

    private void OnEnable()
    {
        currentWayPointIndex = 0;
    }
}