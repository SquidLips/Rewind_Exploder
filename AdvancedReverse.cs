using UnityEngine;
using System.Collections;
using System;

public class AdvancedReverse : MonoBehaviour 
{
    public class timedPosition
    {
        short positionX;
        short positionY;
        short positionZ;
        float rotationX;
        float rotationY;
        float rotationZ;
        sbyte velocityX;
        sbyte velocityY;
        sbyte velocityZ; //Dan was here
        public timedPosition(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            positionX = (short)(position.x * positionMultiplier);
            positionY = (short)(position.y * positionMultiplier);
            positionZ = (short)(position.z * positionMultiplier);
            rotationX = (float)(rotation.x * rotationMultiplier);
            rotationY = (float)(rotation.y * rotationMultiplier);
            rotationZ = (float)(rotation.z * rotationMultiplier);
            velocityX = (sbyte)(velocity.x * velocityMultiplier);
            velocityY = (sbyte)(velocity.y * velocityMultiplier);
            velocityZ = (sbyte)(velocity.z * velocityMultiplier);
        }

        public Vector3 GetPosition() // decompresses and reformats the saved position
        {
            return new Vector3(
                (float)positionX / AdvancedReverse.positionMultiplier, 
                (float)positionY / AdvancedReverse.positionMultiplier, 
                (float)positionZ / AdvancedReverse.positionMultiplier
                );
        }

        public Vector3 GetVelocity() // decompresses and reformats the saved velocity
        {
            return new Vector3(
                (float)velocityX / AdvancedReverse.velocityMultiplier, 
                (float)velocityY / AdvancedReverse.velocityMultiplier, 
                (float)velocityZ / AdvancedReverse.velocityMultiplier
                );
        }

        public Vector3 GetRotation() // decompresses and reformats the saved velocity
        {
            return new Vector3(
                (float)rotationX / AdvancedReverse.rotationMultiplier, 
                (float)rotationY / AdvancedReverse.rotationMultiplier, 
                (float)rotationZ / AdvancedReverse.rotationMultiplier
                );
        }
    }

    ArrayList timedPositions; // the collection of saved states
    int currentFrame = 0; // keeps track of the current frame to determine when to save the state // used with capture frame for interpolation
    const int captureFrame = 1; // the number of frames between saved states
    static int positionMultiplier = 1000; // used in position compression // the multipliers determine how many places behind the decimal to save
    static int velocityMultiplier = 10; // used in velocity compression
    static int rotationMultiplier = 1; // used in rotation compression
    int index = 0; // keeps track of the current index for saving states and rewinding them
    bool timeShift; // controls update logic depending on if the object should be in timeshift mode or not.
    
	// Use this for initialization
	void Start () 
    {
        timedPositions = new ArrayList();
        index = 0;
        Debug.Log(index);
        timeShift = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // entering a timeshift
        {
            timeShift = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            --index;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // exiting a timeshift
        {
            timeShift = false;
            rigidbody.isKinematic = false;
            SetVelocity(); // sets the velocity only when exiting a timeshift, because velocity does nothing during a timeshift
        }
        if (timeShift)
        {
            if (index < 0)
                index = 0;
            SetPosition(); // sets the position every frame during a timeshift to give the illusion of movement
            SetRotation(); // sets the velocity every frame
            if (currentFrame == 0)
                --index;
            --currentFrame;
            if (currentFrame == -1)
                currentFrame = captureFrame - 1;
        }
        else
        {
            if (rigidbody.velocity.x > 12)                                          // clamps velocity from -12 to 12 so it can compress into an sbyte
                rigidbody.velocity = new Vector3(12, rigidbody.velocity.y, 0);
            else if (rigidbody.velocity.x < -12)
                rigidbody.velocity = new Vector3(-12, rigidbody.velocity.y, 0);

            if (rigidbody.velocity.y > 12)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 12, 0);
            else if (rigidbody.velocity.y < -12)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, -12, 0);   

            if (currentFrame == 0)
            {
                try // This will modify the value at the current index or create a new one if the index is out of range
                {
                    timedPositions[index] = new timedPosition(transform.position, transform.eulerAngles, rigidbody.velocity);
                }
                catch
                {
                    timedPositions.Add(new timedPosition(transform.position, transform.eulerAngles, rigidbody.velocity));
                }
                ++index;
            }
            ++currentFrame;
            if (currentFrame == captureFrame)
                currentFrame = 0;
        }
	}

    void SetPosition()
    {
        Debug.Log(((timedPosition)timedPositions[index]).GetPosition());
        if (currentFrame == 0)
        {
            transform.position = ((timedPosition)timedPositions[index]).GetPosition();
        }
        else
        {
            //transform.position = (((timedPosition)timedPositions[index]).GetPosition() + ((timedPosition)timedPositions[index - 1]).GetVelocity()) * ((float)currentFrame / (float)captureFrame);
            transform.position = new Vector3(   ((timedPosition)timedPositions[index]).GetPosition().x + ((timedPosition)timedPositions[index - 1]).GetPosition().x, 
                                                ((timedPosition)timedPositions[index]).GetPosition().y + ((timedPosition)timedPositions[index - 1]).GetPosition().y,
                                                ((timedPosition)timedPositions[index]).GetPosition().z + ((timedPosition)timedPositions[index - 1]).GetPosition().z)
                                                * ((float)currentFrame / (float)captureFrame);
        }
    }

    void SetVelocity()
    {
        if (currentFrame == 0)
        {
            rigidbody.velocity = ((timedPosition)timedPositions[index]).GetVelocity();
            return;
        }
        else
        {
            rigidbody.velocity = new Vector3(   ((timedPosition)timedPositions[index]).GetVelocity().x + ((timedPosition)timedPositions[index - 1]).GetVelocity().x,
                                                ((timedPosition)timedPositions[index]).GetVelocity().y + ((timedPosition)timedPositions[index - 1]).GetVelocity().y,
                                                ((timedPosition)timedPositions[index]).GetVelocity().z + ((timedPosition)timedPositions[index - 1]).GetVelocity().z)
                                                * ((float)currentFrame / (float)captureFrame);
            //rigidbody.velocity = (((timedPosition)timedPositions[index]).GetVelocity() + ((timedPosition)timedPositions[index - 1]).GetVelocity()) * (currentFrame / captureFrame);
        }
    }

    void SetRotation()
    {
        if (currentFrame == 0)
        {
            transform.eulerAngles = ((timedPosition)timedPositions[index]).GetRotation();
            return;
        }
        else
        {
            transform.eulerAngles = new Vector3(((timedPosition)timedPositions[index]).GetRotation().x + ((timedPosition)timedPositions[index - 1]).GetRotation().x,
                                                ((timedPosition)timedPositions[index]).GetRotation().y + ((timedPosition)timedPositions[index - 1]).GetRotation().y,
                                                ((timedPosition)timedPositions[index]).GetRotation().z + ((timedPosition)timedPositions[index - 1]).GetRotation().z)
                                                * ((float)currentFrame / (float)captureFrame);
            //rigidbody.velocity = (((timedPosition)timedPositions[index]).GetVelocity() + ((timedPosition)timedPositions[index - 1]).GetVelocity()) * (currentFrame / captureFrame);
        }
    }
}
