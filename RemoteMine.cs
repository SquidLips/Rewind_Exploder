using UnityEngine;
using System.Collections;

public class RemoteMine : MonoBehaviour {
    public Material mat1;
    public Material mat2;
    public float explosionRadius = 10.0f;
    public float explosionForce = 1000.0f;
    public float upwardsForce = 10.0f;
    public float alternate = 1.0f;

    float timer;
    bool mat;

	// Use this for initialization
	void Start () 
    {
        timer = alternate;
        renderer.material = mat2;
        mat = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (mat)
            {
                renderer.material = mat2;
                mat = false;
            }
            else
            {
                renderer.material = mat1;
                mat = true;
            }
            timer = alternate;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Collider[] hit = Physics.OverlapSphere(transform.position, explosionRadius);
            for (int i = 0; i < hit.Length; ++i)
            {
                hit[i].rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsForce);
            }
            GameObject.Destroy(gameObject);
        }
	}
}
