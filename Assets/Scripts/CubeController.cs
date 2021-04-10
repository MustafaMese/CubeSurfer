using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private static CubeController instance = null;
    public static CubeController Instance { get => instance; }

    [SerializeField] Transform character;
    [SerializeField] Transform cam;
    [SerializeField] Cube cubePrefab;
    [SerializeField] MovementHandler movement;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private LinkedList<Transform> objectList = new LinkedList<Transform>();
    private ActorType actor = ActorType.CONTROLLER;

    private bool canShift = true;
    private bool fallControl = true;

    public event EventHandler<CubeEventArgs> touchHandle;

    private void Awake() 
    {
        instance = this;
    }

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        objectList.AddFirst(character);
        TakeCube();
    }
 
    public virtual void OnTouchedHappened(CubeEventArgs e)
    {
        if(e.targetObjType == actor)
        {
            if(!e.blocked)
                TakeCube();
            else
                Dismiss(e.transform);
        }

        if(touchHandle != null)
            touchHandle(this, e);
    }

    private void Dismiss(Transform cubeTransform)
    {
        objectList.Remove(cubeTransform);

        if (fallControl)
        {
            if (objectList.Count <= 1)
            {
                GameManager.Instance.canStart = false;
                StartCoroutine(UIManager.Instance.OpenDefeatPanel());
            }
            else if (objectList.Count > 1)
                StartCoroutine(OpenTimer());
        }   
        else
        {
            ConfigureCamera();
            if (objectList.Count <= 1)
            {
                GameManager.Instance.canStart = false;
                CubeController.Instance.OnTouchedHappened(new CubeEventArgs(ActorType.CHARACTER, transform, false));
            }
        }
        
    }

    private IEnumerator OpenTimer()
    {
        float time = 2f / Mathf.Abs(movement.movementSpeed);
        yield return new WaitForSeconds(time);
        ConfigureCubes();
    }

    private void ConfigureCubes()
    {
        float index = 0.0f;
        LinkedList<Transform>.Enumerator em = objectList.GetEnumerator();
        while (em.MoveNext())
        {
            Transform val = em.Current;
            Vector3 newPos = val.position;
            newPos.y = index + 0.55f;
            val.GetComponent<IMovable>().Run(newPos);
            index += 1.0f;
        }
    }

    private void ConfigureCamera()
    {
        Vector3 position = cam.position;
        StepUp(cubePrefab.transform.localScale.y, cam);
    }

    private void TakeCube()
    {
        Vector3 position = character.transform.position;
        StepUp(cubePrefab.transform.localScale.y, character);
        Cube cube = CreateCube(position);
        
        LinkedListNode<Transform> last = objectList.Last;
        objectList.AddBefore(last, cube.transform);
    }

    private void StepUp(float height, Transform tForm)
    {
        Vector3 position = tForm.position;
        position.y += height;
        tForm.position = position;
    }

    private Cube CreateCube(Vector3 position)
    {
        Cube cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.transform.SetParent(transform);
        cube.IsTaken = true;
        return cube;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "FinishLine")
        {
            fallControl = false;
        } 
        else if(other.tag == "Final")
        {
            GameManager.Instance.canStart = false;
            CubeController.Instance.OnTouchedHappened(new CubeEventArgs(ActorType.CHARACTER, transform, false));
        }

    }
}