using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollisionShapeE
{
    SPHERE,
    PLANE,
    AABB
}

public class MyPhysicObject : MonoBehaviour
{
    public float Mass;
    public Vector3 Velocity;
    public CollisionShapeE shape;
    public float Bounciness = 1.0f;
    public float Friction = 1.0f;
    public bool Lock = false;
    public bool Ground = false;


    private float radius = 0.0f;

    public Vector3 NewPosition;

    public bool Pause { get; set; } = false;

    public Vector3 up { get; } = new Vector3(0, 1, 0);
    public Vector3 down { get; } = new Vector3(0, -1, 0);
    public Vector3 left { get; } = new Vector3(-1, 0, 0);
    public Vector3 right { get; } = new Vector3(1, 0, 0);
    public Vector3 forward { get; } = new Vector3(0, 0, 1);
    public Vector3 back { get; } = new Vector3(0, 0, -1);

    public bool OnGround { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (shape == CollisionShapeE.PLANE)
        {
            // ground has huge mass
            Mass = float.MaxValue;
        }
        if (shape == CollisionShapeE.SPHERE)
        {
            radius = transform.localScale.x / 2;
        }

        NewPosition = transform.position;
    }

    public void UpdateObject()
    {
        if (shape != CollisionShapeE.PLANE)
        {
            if (!Pause)
            {
                if (!Lock)
                {
                    transform.position = NewPosition;



                    Velocity.y = Velocity.y + Vector3.down.y * MyPhysicsSystem.GRAVITY * Time.deltaTime;
                    //Debug.Log("=== " + Velocity.y);
                    //Velocity.x = Velocity.x * Time.deltaTime * 0.001f;
                    //Velocity.z = Velocity.z * Time.deltaTime * 0.001f;
                    NewPosition.x = transform.position.x + Velocity.x * Time.deltaTime;

                    NewPosition.y = transform.position.y + Velocity.y * Time.deltaTime;
                    NewPosition.z = transform.position.z + Velocity.z * Time.deltaTime;

                    //NewPosition = transform.position + Velocity * Time.deltaTime;
                }
            }
            //if (!Pause)
            //{


            //    PreviousPosition = transform.position;
            //    //transform.position = NewPosition;



            //    Velocity.y = Velocity.y + Vector3.down.y * MyPhysicsSystem.GRAVITY * Time.deltaTime;
            //    //Velocity.x = Velocity.x * Time.deltaTime * 0.001f;
            //    //Velocity.z = Velocity.z * Time.deltaTime * 0.001f;

            //    transform.position = transform.position + Velocity * Time.deltaTime;
            //}
        }
    }

    public void Collision(MyPhysicObject otherObj)
    {
        //Debug.Log("Collision!! with " + otherObj.name);

        if (OnGround)
        {
            //NewPosition.x = transform.position.x;
            NewPosition.y = transform.position.y;
            //NewPosition.z = transform.position.z;
            //Debug.Log("==== ground and vel");
        }
        else
        {
            NewPosition = transform.position;
            //Debug.Log("==== why??");
        }
        //if (!(OnGround && Velocity.y < 0))
        //{
        //    //NewPosition.x = transform.position.x;
        //    NewPosition.y = transform.position.y;
        //    //NewPosition.z = transform.position.z;
        //}
        //else
        //{
        //    NewPosition = transform.position;
        //}
        //InitVelocity();
        if (shape != CollisionShapeE.PLANE)
        {
            ChagneMaterialColorRandom();
        }
        //Pause = true;
    }

    public float GetRadius()
    {
        return radius;
    }

    public void InitVelocity()
    {
        Velocity.x = 0;
        Velocity.y = 0;
        Velocity.z = 0;
    }

    public Vector3 GetNewPosition()
    {
        return NewPosition;
    }

    public Vector3 GetMax()
    {
        return new Vector3(NewPosition.x + transform.localScale.x / 2, NewPosition.y + transform.localScale.y / 2,
            NewPosition.z + transform.localScale.z / 2);
    }
    public Vector3 GetMin()
    {
        return new Vector3(NewPosition.x - transform.localScale.x / 2, NewPosition.y - transform.localScale.y / 2,
                NewPosition.z - transform.localScale.z / 2);
    }

    private void ChagneMaterialColorRandom()
    {
        Color ownColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        GetComponent<Renderer>().material.color = ownColor;
    }

}
