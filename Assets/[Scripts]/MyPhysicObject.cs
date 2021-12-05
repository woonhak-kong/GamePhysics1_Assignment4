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
    public float Bounciness;
    float radius = 0.0f;

    private Vector3 PreviousPosition;
    public Vector3 NewPosition;

    public bool Pause = false;

    public Vector3 up = new Vector3(0, 1, 0);
    public Vector3 down = new Vector3(0, -1, 0);
    public Vector3 left = new Vector3(-1, 0, 0);
    public Vector3 right = new Vector3(1, 0, 0);
    public Vector3 forward = new Vector3(0, 0, 1);
    public Vector3 back = new Vector3(0, 0, -1);


    // Start is called before the first frame update
    void Start()
    {
        if (shape == CollisionShapeE.SPHERE)
        {
            radius = transform.localScale.x / 2;
        }

        NewPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shape != CollisionShapeE.PLANE)
        {
            if (!Pause)
            {

                PreviousPosition = transform.position;
                transform.position = NewPosition;



                Velocity.y = Velocity.y + Vector3.down.y * MyPhysicsSystem.GRAVITY * Time.deltaTime;
                //Velocity.x = Velocity.x * Time.deltaTime * 0.001f;
                //Velocity.z = Velocity.z * Time.deltaTime * 0.001f;

                NewPosition = transform.position + Velocity * Time.deltaTime;

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
        NewPosition = transform.position;
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
