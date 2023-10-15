using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State {pickUp, delivery};
public class WaypointManager : MonoBehaviour
{
    public Scoring score;
    public Timer timer;
    public GameObject waypointPrefab;
    public float despawnDistance = 1.0f; // The distance at which a waypoint should be despawned.
    private Transform currentWaypoint;
    public Vector3 mapBounds = new Vector3(20f, 0f, 20f);
    private State state;

    void Start()
    {
        state = State.pickUp;
        SpawnWaypoint();
    }

    void Update()
    {
        timer.runtimer();
        if (currentWaypoint == null)
        {   
            score.addScore(1);
            SpawnWaypoint();
        }
    }

    void SpawnWaypoint()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-mapBounds.x, mapBounds.x),
            0f,
            -2000f + Random.Range(-mapBounds.z, mapBounds.z)
        );
        timer.resetTimer();

        currentWaypoint = Instantiate(waypointPrefab, randomPosition, Quaternion.identity).transform;
    }
}