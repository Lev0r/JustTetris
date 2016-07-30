using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    //Groups
    public GameObject[] Groups;

	// Use this for initialization
    void Start()
    {
        SpawnNext();
    }
	
	// Update is called once per frame
	void Update () { }

    public void SpawnNext()
    {
        var groupIndex = Random.Range(0, Groups.Length);
        Instantiate(Groups[groupIndex], transform.position, Quaternion.identity);
    }
}
