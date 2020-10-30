using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Ball: MonoBehaviour
{
    public float speed = 1;

    private Vector3 moveDirection = Vector3.forward;
    private LayerMask tileLayerMask;
    private Vector3 startPoint;

    private Transform myTransform;
    private Rigidbody myRigidbody;

    void Awake()
    {
        myTransform = transform;
        startPoint = myTransform.position;
        myRigidbody = myTransform.GetComponent<Rigidbody>();
        tileLayerMask = LayerMask.GetMask(new string[] {"Tile"});
    }
    public void SetToStart()
    {
        myRigidbody.isKinematic = true;
        myTransform.position = startPoint;
        myTransform.rotation = Quaternion.identity;
        moveDirection = Vector3.forward;
    }

    private void ChangeDirection()
    {
        if (moveDirection == Vector3.forward)
            moveDirection = Vector3.right;
        else
            moveDirection = Vector3.forward;
    }

    void Update()
    {
        if (GameManager.instance.gameStatus == gameStatus.Game)
        {
            if (IsOnTile())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeDirection();
                }
                myTransform.position += moveDirection * speed * Time.deltaTime;
                LvlManager.instance.CheckPassedTileSets(myTransform.position);
            }
            else
            {
                Die();
            }
        }

        if (GameManager.instance.gameStatus == gameStatus.GameOver && myTransform.position.y > -10)   //some move after death for better fall
        {
            myTransform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    private bool IsOnTile()
    {
        RaycastHit hitInfo;
        if (Physics.Linecast(myTransform.position, myTransform.position - new Vector3(0, 1, 0), out hitInfo, tileLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Die()
    {
        myRigidbody.isKinematic = false;
        GameManager.instance.GameOver();
    }

}
