using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform targetObject;
    public bool easing = true;
    public int timeStaying = 1;
    public float Speed = 5;


    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Awake()
    {
        startPosition = transform.position;
        targetPosition = targetObject.position;
    }

    void Start()
    {
        StartCoroutine(MoveActionCoroutine());
    }

    void OnDrawGizmos()
    {
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

        float time = 0;
        float duration = distance / Speed;

        AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        if (easing)
        {
            curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, curve.Evaluate(time / duration));


            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(timeStaying);

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

    void OnCollisionEnter2D(Collision2D col)
    {
        col.gameObject.transform.SetParent(gameObject.transform, true);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        col.gameObject.transform.parent = null;
    }
}