using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour {
    public NodeManager.ResourceTypes heldResourceType;
    public bool isSelected = false;
    public bool isGathering = false;
    public string objectName;
    private NavMeshAgent agent;
    public int heldResource;
    public int maxHeldResource;
    GameObject[] drops;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine( GatherTick() );
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        if ( heldResource >= maxHeldResource ) {
            drops = GameObject.FindGameObjectsWithTag( "Drops" );
            agent.destination = GetClosestDropOff( drops ).transform.position;
        }

        if ( Input.GetMouseButtonDown( 1 ) && isSelected ) {
            RightClick();
        }
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
                agent.destination = hit.point;
                Debug.Log( "Moving" );
            } else if ( hit.collider.tag == "Resource" ) {
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log( "Harvesting" );
            }
        }
    }

    public void OnTriggerEnter( Collider other ) {
        GameObject hitObject = other.gameObject;

        if ( hitObject.tag == "Resource" ) {
            Debug.Log( "Is in Resource trigger" );
            isGathering = true;
            hitObject.GetComponent<NodeManager>().gatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        }
    }

    public void OnTriggerExit( Collider other ) {
        GameObject hitObject = other.gameObject;

        if ( hitObject.tag == "Resource" ) {
          Debug.Log( "Exited Resource trigger" );
            hitObject.GetComponent<NodeManager>().gatherers--;
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
