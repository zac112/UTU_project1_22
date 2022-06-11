using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPathfinding : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Transform enemyGraph;

    [SerializeField] float speed = 200f;
    [SerializeField] float nextWaypointDistance = 0.3f;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        GameEvents.current.MapChange += OnTileRevealed;
    }

    void OnDisable(){
        GameEvents.current.MapChange -= OnTileRevealed;
    }

    void OnTileRevealed(Vector3 pos, int size){
        GraphUpdateObject guo = new GraphUpdateObject(new Bounds(pos,Vector3.one*size));
        AstarPath.active.UpdateGraphs(guo);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        } 
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGraph.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.y >= 0.01f)
        {
            enemyGraph.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    public void setTarget(Transform t){
        target=t;
    }
    
    public Transform getTarget(){
        return target;
    }


}
