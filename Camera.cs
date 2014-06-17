using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour 
{
    GameObject player;
    public float xOffset;
    public float yOffset;
    public float zOffset;
	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z + zOffset), 1);
    }
}
