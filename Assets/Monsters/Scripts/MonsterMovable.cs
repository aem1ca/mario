using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovable : Monster
{
    public Transform targetObject;
    public bool easing = true;
    public int timeStaying = 1;

    public bool replaceOnJump = true;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Awake()
    {
        startPosition = transform.position;
        targetPosition = targetObject.position;
    }

    internal override void Start()
    {
        base.Start();
        StartCoroutine(MoveActionCoroutine());
    }

    internal override void CollisionTop(GameObject go)
    {
        base.CollisionTop(go);
        if (go.CompareTag("Player"))
        {
            if (replaceOnJump)
            {
                ReplaceMonster();
            }
        }
    }
    internal override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;


        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetObject.position;

        Gizmos.DrawSphere(startPosition, 0.2f);
        Gizmos.DrawSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(startPosition, targetPosition);
    }

    IEnumerator MoveActionCoroutine()
    {
        float distance = Vector2.Distance(startPosition, targetPosition);
        AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        if (easing)
        {
            curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }


        if (startPosition.x > targetPosition.x)
        {
            directionX = -1;    
        }
        else
        {
            directionX = 1;
        }
        OrientSprite();
        
        float time = 0;
        float duration = distance / Speed;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(timeStaying);

        
        if (startPosition.x > targetPosition.x)
        {
            directionX = 1;    
        }
        else
        {
            directionX = -1;
        }
        OrientSprite();
        
        time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, startPosition, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;

        yield return new WaitForSeconds(timeStaying);

        StartCoroutine(MoveActionCoroutine());
    }

}