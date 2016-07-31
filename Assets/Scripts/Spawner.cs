using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public List<Group> GroupsQueue;

    public Vector3[] QueuePositions;
    public Vector3 SpawnPosition;

    //GroupsPrefabs
    public Group[] GroupsPrefabs;


	// Use this for initialization
    void Start()
    {
        GroupsQueue = new List<Group>();
        for (int i = 0; i < QueuePositions.Length; i++)
        {
            var groupIndex = Random.Range(0, GroupsPrefabs.Length);
            var group = Instantiate(GroupsPrefabs[groupIndex], QueuePositions[i], Quaternion.identity) as Group;
            group.enabled = false;
            GroupsQueue.Add(group);
        }
        SpawnNext();
    }
	
	// Update is called once per frame
	void Update () { }

    public void SpawnNext()
    {
        var groupToSpawn = GroupsQueue[0];
        GroupsQueue.RemoveAt(0);
        groupToSpawn.transform.position = SpawnPosition;
        groupToSpawn.enabled = true;

        for (int i = 0; i < QueuePositions.Length - 1; i++)
        {
            GroupsQueue[i].transform.position = QueuePositions[i];
        }
        var groupIndex = Random.Range(0, GroupsPrefabs.Length);
        var group = Instantiate(GroupsPrefabs[groupIndex], QueuePositions[QueuePositions.Length -1], Quaternion.identity) as Group;
        group.enabled = false;
        GroupsQueue.Add(group);
    }    
}
