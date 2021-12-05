using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhysicsSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public List<MyPhysicObject> gameObjectList;
    public float Gravity = 9.81f;
    public static float GRAVITY;

    enum CubeAxis{
        LEFT = 0,
        RIGHT,
        DOWN,
        UP,
        BACK,
        FORWARD
    }

    private void Awake()
    {
        GRAVITY = Gravity;
        gameObjectList = new List<MyPhysicObject>(FindObjectsOfType<MyPhysicObject>());
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (MyPhysicObject obj in gameObjectList)
        {
            //float newPositionY = obj.transform.position.y + gravity * Time.deltaTime * 0.1f;
            //obj.transform.position = new Vector3(obj.transform.position.x, newPositionY, 0);
            //Vector3 newAccelerationVector3 = obj.GetComponent<MyPhysicObject>().Velocity + Vector3.down * gravity * Time.deltaTime;
            //obj.GetComponent<MyPhysicObject>().Velocity = newAccelerationVector3;
        }

        DetectCollision();
    }

    void DetectCollision()
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            for (int j = i + 1; j < gameObjectList.Count; j++)
            {

                if (CheckCollision(gameObjectList[i], gameObjectList[j]))
                {
                    gameObjectList[i].Collision(gameObjectList[j]);
                    gameObjectList[j].Collision(gameObjectList[i]);
                }
            }
        }
    }

    bool CheckCollision(MyPhysicObject obj1, MyPhysicObject obj2)
    {
        MyPhysicObject first = obj1;
        MyPhysicObject second = obj2;


        // PLANE Shape is always should be in first
        // PLANE -- SPHERE
        // PLANE -- AABB
        // SPHERE -- AABB
        switch(obj1.shape)
        {
            case CollisionShapeE.SPHERE:
                if(obj2.shape == CollisionShapeE.SPHERE)
                {
                    first = obj1;
                    second = obj2;
                }
                else if(obj2.shape == CollisionShapeE.PLANE)
                {
                    first = obj2;
                    second = obj1;
                }
                else if (obj2.shape == CollisionShapeE.AABB)
                {
                    first = obj1;
                    second = obj2;
                }
                break;

            case CollisionShapeE.PLANE:
                if (obj2.shape == CollisionShapeE.SPHERE)
                {
                    first = obj1;
                    second = obj2;
                }
                else if (obj2.shape == CollisionShapeE.PLANE)
                {
                    first = obj1;
                    second = obj2;
                }
                else if (obj2.shape == CollisionShapeE.AABB)
                {
                    first = obj1;
                    second = obj2;
                }
                break;
            case CollisionShapeE.AABB:
                if (obj2.shape == CollisionShapeE.SPHERE)
                {
                    first = obj2;
                    second = obj1;
                }
                else if (obj2.shape == CollisionShapeE.PLANE)
                {
                    first = obj2;
                    second = obj1;
                }
                else if (obj2.shape == CollisionShapeE.AABB)
                {
                    first = obj1;
                    second = obj2;
                }
                break;
            default:
                break;
        }

        if(first.shape == CollisionShapeE.SPHERE && second.shape == CollisionShapeE.SPHERE)
        {
            Vector3 subResult = second.GetNewPosition() - first.GetNewPosition();
            float distanceBetweenObjs = Mathf.Sqrt(subResult.x * subResult.x + subResult.y * subResult.y + subResult.z * subResult.z);

            // when the distance between two objects is less than the sum of two objects' radious, they collided.
            if(distanceBetweenObjs < first.GetRadius() + second.GetRadius())
            {
                Vector3 normalFirst = subResult.normalized;
                Vector3 normalVelocityFirst = normalFirst * Vector3.Dot(normalFirst, first.Velocity);
                Vector3 tangentialVelocityFirst = first.Velocity - normalVelocityFirst;

                Vector3 normalSecond = -normalFirst;
                Vector3 normalVelocitySecond = normalSecond * Vector3.Dot(normalSecond, second.Velocity);
                Vector3 tangentialVelocitySecond = second.Velocity - normalVelocitySecond;

                first.Velocity = (normalVelocityFirst * (first.Mass - second.Mass) + normalVelocitySecond * (2 * second.Mass)) / (first.Mass + second.Mass) + tangentialVelocityFirst;
                second.Velocity = (normalVelocitySecond * (second.Mass - first.Mass) + normalVelocityFirst * (2 * first.Mass)) / (first.Mass + second.Mass) + tangentialVelocitySecond;

                return true;
            }
        }
        else if (first.shape == CollisionShapeE.AABB && second.shape == CollisionShapeE.AABB)
        {
            bool noCollision = first.GetMax().x <= second.GetMin().x ||
                first.GetMax().y <= second.GetMin().y ||
                first.GetMax().z <= second.GetMin().z ||
                second.GetMax().x <= first.GetMin().x ||
                second.GetMax().y <= first.GetMin().y ||
                second.GetMax().z <= first.GetMin().z;

            if (!noCollision)
            {
                Vector3 normalFirst = new Vector3(0,0,0);

                // for finding collision axis
                float[] distance = new float[6];
                distance[(int)CubeAxis.LEFT] = first.GetMax().x - second.GetMin().x;
                distance[(int)CubeAxis.RIGHT] = second.GetMax().x - first.GetMin().x;
                distance[(int)CubeAxis.DOWN] = first.GetMax().y - second.GetMin().y;
                distance[(int)CubeAxis.UP] = second.GetMax().y - first.GetMin().y;
                distance[(int)CubeAxis.BACK] = first.GetMax().z - second.GetMin().z;
                distance[(int)CubeAxis.FORWARD] = second.GetMax().z - first.GetMin().z;

                float least = float.MaxValue;
                int leastIdx = 0;
                for(int i = 0; i < distance.Length; i++)
                {
                    if(distance[i] < least)
                    {
                        least = distance[i];
                        leastIdx = i;
                    }
                }

                switch((CubeAxis)leastIdx)
                {
                    case CubeAxis.LEFT:
                        normalFirst = second.left;
                        break;
                    case CubeAxis.RIGHT:
                        normalFirst = second.right;
                        break;
                    case CubeAxis.DOWN:
                        normalFirst = second.down;
                        break;
                    case CubeAxis.UP:
                        normalFirst = second.up;
                        break;
                    case CubeAxis.BACK:
                        normalFirst = second.back;
                        break;
                    case CubeAxis.FORWARD:
                        normalFirst = second.forward;
                        break;

                }

                //Vector3 normalFirst = subResult.normalized;
                //Vector3 normalVelocityFirst = normalFirst * Vector3.Dot(normalFirst, first.Velocity);
                //Vector3 tangentialVelocityFirst = first.Velocity - normalVelocityFirst;

                //Vector3 normalSecond = -normalFirst;
                //Vector3 normalVelocitySecond = normalSecond * Vector3.Dot(normalSecond, second.Velocity);
                //Vector3 tangentialVelocitySecond = second.Velocity - normalVelocitySecond;

                //first.Velocity = (normalVelocityFirst * (first.Mass - second.Mass) + normalVelocitySecond * (2 * second.Mass)) / (first.Mass + second.Mass) + tangentialVelocityFirst;
                //second.Velocity = (normalVelocitySecond * (second.Mass - first.Mass) + normalVelocityFirst * (2 * first.Mass)) / (first.Mass + second.Mass) + tangentialVelocitySecond;
                //normalFirst = -1 * normalFirst;
                float e = Mathf.Min(first.Bounciness, second.Bounciness);
                Vector3 relativeVelocity = first.Velocity - second.Velocity;

                // finding impulse
                float j = (-1*(1 + e) * Vector3.Dot(relativeVelocity, normalFirst) ) /
                    (1 / first.Mass + 1 / second.Mass);

                first.Velocity = first.Velocity + (j / first.Mass) * normalFirst;
                second.Velocity = second.Velocity - (j / second.Mass) * normalFirst;

                return true;
            }



        }
        else if (first.shape == CollisionShapeE.SPHERE && second.shape == CollisionShapeE.AABB)
        {

            // get closestPoint
            Vector3 closestPoint = GetClosestPointBetweenSphereAndAABB(first, second);
            Vector3 differenceVector = first.GetNewPosition() - closestPoint;

            // magnitude for checking collision
            float distanceSquared = differenceVector.sqrMagnitude;
            float radiusSquared = first.GetRadius() * first.GetRadius();

            if( distanceSquared < radiusSquared)
            {
                //Vector3 normal = (first.GetNewPosition() - closestPoint).normalized;

                Vector3 normalFirst = (first.GetNewPosition() - closestPoint).normalized;
                Vector3 normalVelocityFirst = normalFirst * Vector3.Dot(normalFirst, first.Velocity);
                Vector3 tangentialVelocityFirst = first.Velocity - normalVelocityFirst;

                Vector3 normalSecond = -normalFirst;
                Vector3 normalVelocitySecond = normalSecond * Vector3.Dot(normalSecond, second.Velocity);
                Vector3 tangentialVelocitySecond = second.Velocity - normalVelocitySecond;

                first.Velocity = (normalVelocityFirst * (first.Mass - second.Mass) + normalVelocitySecond * (2 * second.Mass)) / (first.Mass + second.Mass) + tangentialVelocityFirst;
                second.Velocity = (normalVelocitySecond * (second.Mass - first.Mass) + normalVelocityFirst * (2 * first.Mass)) / (first.Mass + second.Mass) + tangentialVelocitySecond;


                return true;
            }
        }
        else if (first.shape == CollisionShapeE.PLANE && second.shape == CollisionShapeE.SPHERE)
        {
            Vector3 upvectorOfPlane = first.transform.up;
            Vector3 positionVectorOfSphere = second.GetNewPosition() - first.GetNewPosition();
            //Debug.Log("--------- " + Vector3.Angle(first.transform.up, positionVectorOfSphere));
            // using dot product find distance between plane and sphere
            // if distance is less than sphere's radius, they collide.
            float distanceBetweenObjs = Vector3.Dot(upvectorOfPlane, positionVectorOfSphere);
            if(distanceBetweenObjs < second.GetRadius())
            {
                // r+ d dot n
                //float tmp = second.GetRadius() + Vector3.Dot(first.transform.up, -positionVectorOfSphere);
                //float angle = Vector3.Angle(first.transform.right, second.Velocity);

                //float deltaS = tmp / Mathf.Sin(angle) + 0.01f;
                //Debug.Log("===" + deltaS);
                //Vector3 paraVector = Vector3.Normalize(second.Velocity) * deltaS;
                //second.transform.position = second.transform.position - paraVector;
                // response
                Vector3 normalVelocity =  -upvectorOfPlane * Vector3.Dot(second.Velocity, -upvectorOfPlane);
                Vector3 tangentialVelocity = second.Velocity - normalVelocity;

                second.Velocity = -normalVelocity + tangentialVelocity;


                return true;
            }

        }
        else if (first.shape == CollisionShapeE.PLANE && second.shape == CollisionShapeE.AABB)
        {
            Vector3 centerOfAABB = second.GetNewPosition();
            Vector3 extents = second.GetMax() - centerOfAABB;
            //var r = extents.x * Math.abs( Plane.normal.x ) + extents.y * Math.abs( Plane.normal.y ) + extents.z * Math.abs( Plane.normal.z );
            //var s = Plane.normal.dot(center) - Plane.constant;
            //Math.abs( s ) <= r;

            float r = extents.x * Mathf.Abs(first.transform.up.x) + extents.y * Mathf.Abs(first.transform.up.y) + extents.z * Mathf.Abs(first.transform.up.z);
            var s = Vector3.Dot(first.transform.up, centerOfAABB) - Vector3.Dot(first.transform.up, first.GetNewPosition());
            //s = Vector3.Dot(first.transform.up, centerOfAABB - first.GetNewPosition());

            if (Mathf.Abs(s) < r)
            {

                //second.Pause = true;
                Vector3 normalVelocity = -first.transform.up * Vector3.Dot(second.Velocity, -first.transform.up);
                Vector3 tangentialVelocity = second.Velocity - normalVelocity;

                second.Velocity = -normalVelocity + tangentialVelocity;
                return true;
            }
        }
        // we don't need
        else if (first.shape == CollisionShapeE.PLANE && second.shape == CollisionShapeE.PLANE)
        {

        }

        return false;
    }

    private Vector3 GetClosestPointBetweenSphereAndAABB(MyPhysicObject sphere, MyPhysicObject aabb)
    {
        Vector3 result = new Vector3(0,0,0);
        if(sphere.GetNewPosition().x > aabb.GetMax().x)
        {
            result.x = aabb.GetMax().x;
        }
        else if (sphere.GetNewPosition().x < aabb.GetMin().x)
        {
            result.x = aabb.GetMin().x;
        }
        else
        {
            result.x = sphere.GetNewPosition().x;
        }

        if (sphere.GetNewPosition().y > aabb.GetMax().y)
        {
            result.y = aabb.GetMax().y;
        }
        else if (sphere.GetNewPosition().y < aabb.GetMin().y)
        {
            result.y = aabb.GetMin().y;
        }
        else
        {
            result.y = sphere.GetNewPosition().y;
        }

        if (sphere.GetNewPosition().z > aabb.GetMax().z)
        {
            result.z = aabb.GetMax().z;
        }
        else if (sphere.GetNewPosition().z < aabb.GetMin().z)
        {
            result.z = aabb.GetMin().z;
        }
        else
        {
            result.z = sphere.GetNewPosition().z;
        }

        return result;
    }
}
