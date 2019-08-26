using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    private enum Direction { North, East, South, West };
    private Direction playerDirection = Direction.South;
    private Animator animator;
    public float maxSpeed = 7;
    private Vector2 targetVelocity;
    private Rigidbody2D playerRigidbody;
    private ContactFilter2D movementContactFilter;

    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;
    private bool fireIsPressed = false;
    private bool controlAreEnable = true;
    // Use this for initialization
    void Start()
    {
        movementContactFilter = BuildContactFilter2DForPlayer(LayerMask.LayerToName(gameObject.layer));
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controlAreEnable)
        {
            ProcessIput();
        }
        Vector2 velocityX = new Vector2();
        Vector2 velocityY = new Vector2();
        velocityX.x = targetVelocity.x;
        velocityY.y = targetVelocity.y;
        Vector2 deltaPositionX = velocityX * Time.deltaTime;
        Mouvement(deltaPositionX);
        Vector2 deltaPositionY = velocityY * Time.deltaTime;
        Mouvement(deltaPositionY);
    }
    private void ProcessIput()
    {
        ComputeVelocity();
    }
    private void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        targetVelocity = move.normalized * maxSpeed;
        UpdateDiretion(move.x, move.y);
        UpdateAnimationSpeed(targetVelocity.magnitude);
    }

    private void UpdateDiretion(float movementX, float movementY)
    {

    }

    private void UpdateAnimationSpeed(float speed)
    {

    }

    private void Mouvement(Vector2 move)
    {
        float distance = move.magnitude;
        RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
        if (distance > minMoveDistance)
        {
            int movementCollisionHitCount = playerRigidbody.Cast(move, movementContactFilter, hitbuffer, distance + shellRadius);
            List<RaycastHit2D> hitbufferList = BufferArrayhotToList(hitbuffer, movementCollisionHitCount);
            for (int i = 0; i < hitbufferList.Count; i++)
            {
                Vector2 currentNormal = hitbufferList[i].normal;
                float modifiedDistance = hitbufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        playerRigidbody.position = playerRigidbody.position + move.normalized * distance;
    }
    private ContactFilter2D BuildContactFilter2DForPlayer(string LayerName)
    {
        ContactFilter2D contactFilder2D = new ContactFilter2D();
        contactFilder2D.useTriggers = false;
        contactFilder2D.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer(LayerName)));
        contactFilder2D.useLayerMask = true;
        return contactFilder2D;
    }

    private List<RaycastHit2D> BufferArrayhotToList(RaycastHit2D[] hitBuffer, int count)
    {
        List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(count);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++)
        {
            hitBufferList.Add(hitBuffer[i]);
        }
        return hitBufferList;
    }
}
