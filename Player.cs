using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public float moveSpeed;
    public float sprintModifier;
    public float jumpHeight;
    public float explosionRadius = 10.0f;
    public float explosionForce = 100.0f;
    public float upwardsForce = 1.0f;
    int jumplenght = 0;
    float MOVESPEED;
	// Use this for initialization
	void Start () 
    {
        MOVESPEED = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumplenght = 0;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] hit = Physics.OverlapSphere(transform.position, explosionRadius);
            for (int i = 0; i < hit.Length; ++i)
            {
                hit[i].rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsForce);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject foo;
            foo = (GameObject)GameObject.Instantiate(Resources.Load("Mine"));
            foo.transform.position = transform.position;
        }
	}

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddRelativeForce(Vector3.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddRelativeForce(Vector3.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddRelativeForce(Vector3.back * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddRelativeForce(Vector3.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -5, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 5, 0));
        }
        ++jumplenght;

        if (Input.GetKeyDown(KeyCode.C))
        {
            moveSpeed = MOVESPEED * sprintModifier;
        } 
        if (Input.GetKeyUp(KeyCode.C))
        {
            moveSpeed = MOVESPEED;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (jumplenght > 1)
        {
            //Debug.Log(jumplenght);
        }
        jumplenght = 0;
    }
}
