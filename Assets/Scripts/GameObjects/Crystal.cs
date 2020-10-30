using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crystal : MonoBehaviour
{
    private float rotateSpeed = 180;

    private Transform myTransform;

    private static List<GameObject> activeCrystals = new List<GameObject>(); 

    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        myTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        if(myTransform.position.y < -10)                            //to deactivate missed crystal
        {
            CrystalPooler.instance.DeactivateCrystal(gameObject);
        }
    }

    private void OnEnable()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 360), Space.World);
        activeCrystals.Add(gameObject);
    }
    private void OnDisable()
    {
        activeCrystals.Remove(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            GameManager.instance.CollectCrystal();
            CrystalPooler.instance.DeactivateCrystal(gameObject);
        }
    }
}
