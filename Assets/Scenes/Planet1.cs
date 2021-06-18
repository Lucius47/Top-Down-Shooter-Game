using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet1 : MonoBehaviour
{
    float mass = 100;
    public Transform otherPlanet;
    public float initialForce;
    
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * initialForce * Time.fixedDeltaTime, ForceMode.Force);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dirToOtherPlanet = (otherPlanet.position - transform.position);
        float force = 0.06674f * ((mass * mass) /dirToOtherPlanet.magnitude);
        GetComponent<Rigidbody>().AddForce(dirToOtherPlanet.normalized * force *Time.fixedDeltaTime, ForceMode.Force);
        
    }
}
