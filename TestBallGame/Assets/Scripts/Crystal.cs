using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crystal : MonoBehaviour
{
    private float rotateSpeed = 180;

    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        myTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            GameManager.instance.CollectCrystal();
            CrystalPooler.instance.DeactivateCrystal(gameObject);
        }
    }
}
