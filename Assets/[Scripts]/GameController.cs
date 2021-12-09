using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Sphere Launch Properties")]
    public GameObject spherePrefab;
    public GameObject pingpongBall;
    public GameObject baseBall;
    public GameObject basketBall;
    public GameObject bowlingBall;
    public float FiringDelayTimeInSecond = 1;
    public float Offset;
    public float LauchSpeed;

    private float LimitFiring = 0;

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.GetAxisRaw("Fire1") > 0 && LimitFiring > FiringDelayTimeInSecond)
            {
                //var randomSphereIndex = Random.Range(0, spherePrefabs.Count);
                var bullet = Instantiate(spherePrefab, Camera.main.transform.position + Camera.main.transform.forward * Offset, Quaternion.identity);
                bullet.GetComponent<MyPhysicObject>().Velocity = Camera.main.transform.forward * LauchSpeed;
                bullet.GetComponent<MyPhysicObject>().NewPosition = bullet.GetComponent<MyPhysicObject>().transform.position;

                MyPhysicsSystem system = FindObjectOfType<MyPhysicsSystem>();
                system.gameObjectList.Add(bullet.GetComponent<MyPhysicObject>());

                LimitFiring = 0.0f;
            }
        }
        LimitFiring += Time.deltaTime;
    }
}
