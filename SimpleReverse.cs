using UnityEngine;
using System.Collections;

public class SimpleReverse : MonoBehaviour 
{
    bool timeShift = false;
    ArrayList positions;
    ArrayList rotations;
    ArrayList velocities;
    int index = 0;
    public static int captureFrame = 1;
    int currentFrame = 0;
	// Use this for initialization
	void Start ()
    {
        positions = new ArrayList();
        rotations = new ArrayList();
        velocities = new ArrayList();
	}
	
	// Update is called once per frame
	void Update ()
    {        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            timeShift = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            Time.timeScale = 1;
            index--;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            timeShift = false;
            rigidbody.isKinematic = false;
            Time.timeScale = 1;
            rigidbody.velocity = (Vector3)velocities[index];
        }
        if (timeShift)
        {
            --currentFrame;
            if (currentFrame % captureFrame == 0)
            {
                if (index > 1)
                {
                    transform.position = (Vector3)positions[index];
                    transform.localEulerAngles = (Vector3)rotations[index - 1];
                    --index;
                }                
            }
        }
        else
        {
            ++currentFrame;
            if (currentFrame % captureFrame == 0)
            {
                if (index < positions.Count)
                {
                    positions.Insert(index, transform.position);
                    rotations.Insert(index, transform.localEulerAngles);
                    velocities.Insert(index, rigidbody.velocity);
                }
                else
                {
                    positions.Add(transform.position);
                    rotations.Add(transform.localEulerAngles);
                    velocities.Add(rigidbody.velocity);
                }
                ++index;                
            }
        }
        //Debug.Log(index);
        //Debug.Log(((Vector3)rotations[index - 1]).z);
	}
}
