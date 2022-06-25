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
    int currentWaypoint = 1;

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
        
        Move(path.vectorPath[currentWaypoint-1], path.vectorPath[currentWaypoint]);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    ///Called every FixedUpdate o move the transform
    virtual protected void Move(Vector3 from, Vector3 to){        
        Vector2 direction = ((Vector2) to - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        /*
        if (force.x >= 0.01f)
        {
            enemyGraph.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.y >= 0.01f)
        {
            enemyGraph.localScale = new Vector3(1f, 1f, 1f);
        }*/
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 1;
        }
    }
    void UpdatePath()
    {
        if (target && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    public void setTarget(Transform t){
        target=t;
        enemyGraph = t.transform;
    }
    
    public Transform getTarget(){
        return target;
    }

    public Path GetPath() => path;
    public float GetSpeed() => speed;

}
