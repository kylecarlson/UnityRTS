using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : MonoBehaviour {
    public TaskList task;
    private ActionList AL;
    GameObject targetNode;
    GameObject[] drops;
    public NodeManager.ResourceTypes heldResourceType;
    public ResourceManager RM;
    public bool isGathering = false;
    private NavMeshAgent agent;
    public int heldResource;
    public int maxHeldResource;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine( GatherTick() );
        agent = GetComponent<NavMeshAgent>();
        AL = FindObjectOfType<ActionList>();
    }

    // Update is called once per frame
    void Update() {


        if ( targetNode == null ) {
            if ( heldResource != 0 ) {
                DeliverResource();
            } else {
                task = TaskList.Idle;
            }
        }

        if ( heldResource >= maxHeldResource ) {
            DeliverResource();
        }

        if ( Input.GetMouseButtonDown( 1 ) && GetComponent<ObjectInfo>().isSelected ) {
            RightClick();
        }
    }

    void DeliverResource() {
        drops = GameObject.FindGameObjectsWithTag( "Drops" );
        agent.destination = GetClosestDropOff( drops ).transform.position;
        drops = null;
        task = TaskList.Delivering;
    }

    GameObject GetClosestDropOff( GameObject[] dropOffs ) {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach ( GameObject targetDrop in dropOffs ) {
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;

            if ( distance < closestDistance ) {
                closestDistance = distance;
                closestDrop = targetDrop;
            }
        }

        drops = null;
        return closestDrop;
    }

    void RightClick() {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit, 100 ) ) {
            if ( hit.collider.tag == "Ground" ) {
                AL.Move( agent, hit );
                task = TaskList.Moving;
            } else if ( hit.collider.tag == "Resource" ) {
                AL.Move( agent, hit );
                targetNode = hit.collider.gameObject;
                task = TaskList.Gathering;
            }
        }
    }

    public void OnTriggerEnter( Collider other ) {
        GameObject hitObject = other.gameObject;

        if ( hitObject.tag == "Resource" && task == TaskList.Gathering ) {
            Debug.Log( "Is in Resource trigger" );
            isGathering = true;
            hitObject.GetComponent<NodeManager>().gatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        } else if ( hitObject.tag == "Drops" && task == TaskList.Delivering ) {

            if ( RM.stone >= RM.maxStone ) {
                task = TaskList.Idle;
            } else {
                RM.stone += heldResource;
                heldResource = 0;
                task = TaskList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void OnTriggerExit( Collider other ) {
        GameObject hitObject = other.gameObject;

        if ( hitObject.tag == "Resource" ) {
            Debug.Log( "Exited Resource trigger" );
            hitObject.GetComponent<NodeManager>().gatherers--;
            isGathering = false;
        }
    }

    IEnumerator GatherTick() {
        while( true ) {
            yield return new WaitForSeconds( 1 );

            if ( isGathering ) {
                if ( heldResource < maxHeldResource ) {
                    heldResource++;
                }
            }
        }
    }
}
