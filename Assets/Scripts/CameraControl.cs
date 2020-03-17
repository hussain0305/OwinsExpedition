using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public float MoveSpeed = 10f; 
    public float SnapDistance = 0.25f;
    public Transform MainAxis;
    public Transform ShakeAxis;

    // For moving camera
    public bool IsMoving { get; private set; }
    private Vector3 NewPosition;
    private float CurrentMoveSpeed;

    // For shaking camera
    private bool IsShaking = false;
    private int ShakeCount;
    private float ShakeIntensity, ShakeSpeed, BaseX, BaseY;
    private Vector3 NextShakePosition;


    void Start()
    {
        enabled = false;

        BaseX = ShakeAxis.localPosition.x;
        BaseY = ShakeAxis.localPosition.y;
    }


    void Update()
    {
        // Are we moving?
        if (IsMoving)
        {
            // Move us toward the new position
            MainAxis.position = Vector3.MoveTowards(MainAxis.position, NewPosition, Time.deltaTime * CurrentMoveSpeed);

            // Determine if we are there or not (within snap distance)
            if (Vector2.Distance(MainAxis.position, NewPosition) < SnapDistance)
            {
                MainAxis.position = NewPosition;
                IsMoving = false;
                if (!IsShaking) enabled = false;
            }
        }
        // ...or are we shaking? (Could be both)
        if (IsShaking)
        {
            // Move toward the previously determined next shake position
            ShakeAxis.localPosition = Vector3.MoveTowards(ShakeAxis.localPosition, NextShakePosition, Time.deltaTime * ShakeSpeed);

            // Determine if we are there or not
            if (Vector2.Distance(ShakeAxis.localPosition, NextShakePosition) < ShakeIntensity / 5f)
            {
                //Decrement shake counter
                ShakeCount--;

                // If we are done shaking, turn this off if we're not longer moving
                if (ShakeCount <= 0)
                {
                    IsShaking = false;
                    ShakeAxis.localPosition = new Vector3(BaseX, BaseY, ShakeAxis.localPosition.z);
                    if (!IsMoving) enabled = false;
                }
                // If there is only 1 shake left, return back to base
                else if (ShakeCount <= 1)
                {
                    NextShakePosition = new Vector3(BaseX, BaseY, ShakeAxis.localPosition.z);
                }
                // If we are not done or nearing done, determine the next position to travel to
                else
                {
                    DetermineNextShakePosition();
                }
            }
        }
    }

    public void Move(float x, float y, float speed = 0)
    {
        // If a speed is passed in, use that. Otherwise use the default.
        if (speed > 0) CurrentMoveSpeed = speed;
        else CurrentMoveSpeed = MoveSpeed;

        // Set us up to move
        NewPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        IsMoving = true;
        enabled = true;
    }

    public void SetPosition(Vector2 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.y, MainAxis.position.z);
        MainAxis.position = newPosition;
    }
    
    public void Shake(float intensity, int shakes, float speed)
    {
        enabled = true;
        IsShaking = true;
        ShakeCount = shakes;
        ShakeIntensity = intensity;
        ShakeSpeed = speed;

        DetermineNextShakePosition();
    }

    public void ShakeDefault()
    {
        enabled = true;
        IsShaking = true;
        ShakeCount = 5;
        ShakeIntensity = 0.4f;
        ShakeSpeed = 100;

        DetermineNextShakePosition();
    }


    private void DetermineNextShakePosition()
    {
        NextShakePosition = new Vector3(Random.Range(-ShakeIntensity, ShakeIntensity),
            Random.Range(-ShakeIntensity, ShakeIntensity),
            ShakeAxis.localPosition.z);
    }
}