using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet2 : MonoBehaviour
{
    public Transform otherPlanet;
    float mass = 100;
    public float initialForce;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.back * initialForce * Time.fixedDeltaTime, ForceMode.Force);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dirToOtherPlanet = (otherPlanet.position - transform.position);
        Debug.DrawLine(transform.position, otherPlanet.position, Color.red);
        float force = 0.06674f * ((mass * mass) / dirToOtherPlanet.magnitude);
        GetComponent<Rigidbody>().AddForce(dirToOtherPlanet.normalized * force * Time.fixedDeltaTime, ForceMode.Force);

    }
}
