using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IMovable
{
    [SerializeField] bool isTaken;
    [SerializeField] ActorType actor = ActorType.CUBE;

    public bool IsTaken { get => isTaken; set => isTaken = value; }

    #region MOVE VARIABLES
    private Vector3 targetPosition;
    private bool isRunning = false;
    #endregion
    
    private void Update() 
    {
        if(isRunning)
        {
            Vector3 newPos = transform.position;
            float y = Mathf.Lerp(newPos.y, targetPosition.y, 0.15f);
            newPos.y = y;
            if(Mathf.Abs(targetPosition.y - y) < 0.01)
            {
                transform.position = targetPosition;
                isRunning = false;
            }
            transform.position = newPos;
        }
    }

    public void Run(Vector3 targetPosition)
    {
        isRunning = true;
        this.targetPosition = targetPosition;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(IsTaken && other.tag == "Cube" && !other.GetComponent<Cube>().IsTaken)
        {
            Kill(other.gameObject);
            CubeController.Instance.OnTouchedHappened(new CubeEventArgs(ActorType.CONTROLLER, transform, false));
        }
        else if(other.tag == "Obstacle")
        {
            Vector3 direction = GetDirection(other);
            transform.SetParent(null);
            transform.position = other.transform.position - direction;
            CubeController.Instance.OnTouchedHappened(new CubeEventArgs(ActorType.CONTROLLER, transform, true));
        }
        else if(other.tag == "FinishCube")
        {
            Vector3 direction = GetDirection(other);
            transform.SetParent(null);
            Vector3 pos = other.bounds.ClosestPoint(transform.position);
            transform.position = new Vector3(pos.x - direction.x, transform.position.y, transform.position.z);
            CubeController.Instance.OnTouchedHappened(new CubeEventArgs(ActorType.CONTROLLER, transform, true));
        }
    }

    private Vector3 GetDirection(Collider other)
    {
        var heading = other.transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return direction;
    }

    private void Kill(GameObject obj)
    {
        Destroy(obj);
    }
}
