using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IMovable
{
    [SerializeField] Animator animator;

    private ActorType actor = ActorType.CHARACTER;
    private Vector3 targetPosition;
    private bool isRunning = false;
    
    private void Start()
    {
        CubeController.Instance.touchHandle += OnTouchedHappened;
    }

    private void Update() 
    {
        if(isRunning)
        {
            Vector3 newPos = transform.position;
            float y = Mathf.Lerp(newPos.y, targetPosition.y, 0.1f);
            newPos.y = y;
            if (Mathf.Abs(targetPosition.y - y) < 0.01)
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

    protected virtual void OnTouchedHappened(object sender, CubeEventArgs e)
    {
        if (e.targetObjType == actor)
            animator.SetTrigger("Dance");
    }
    
}
