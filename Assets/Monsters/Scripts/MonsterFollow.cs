using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollow : Monster
{
    public GameObject eggPrefab;
    public Transform eggShootingPoint;
    private float lastEgg = 0;
    internal override void MoveAction()
    {
        if (canMove)
        {
            float distanceHorizontal = Mathf.Abs(transform.position.x - CharacterMovement.Instance.transform.position.x);
            if (distanceHorizontal > 2f)
            {
                if (CharacterMovement.Instance.transform.position.x < transform.position.x)
                {
                    directionX = -1;
                }
                else
                {
                    directionX = 1;
                }

                m_Rigidbody2D.linearVelocity = new Vector2(Speed * directionX, m_Rigidbody2D.linearVelocity.y);
            }
            else
            {
                m_Rigidbody2D.linearVelocity = new Vector2(0, 0);
                TryToMakeEgg();
            }
        }
    }



    void TryToMakeEgg()
    {
        if (Time.time - lastEgg > 3)
        {
            lastEgg = Time.time;
            MakeEgg();
        }
    }

    void MakeEgg()
    {
        GameObject go = Instantiate(eggPrefab);
        go.transform.position = eggShootingPoint.position;

        float directionToMario = 1;
        if (CharacterMovement.Instance.transform.position.x < transform.position.x)
        {
            directionToMario = -1;
        }
        else
        {
            directionToMario = 1;
        }

        float dropForce = 2;
        go.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(go.GetComponent<Rigidbody2D>().linearVelocity.x, 0);
        go.GetComponent<Rigidbody2D>().AddForce(new Vector2(directionToMario, 1) * dropForce, ForceMode2D.Impulse);
    }
}