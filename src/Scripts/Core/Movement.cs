using HelperPackage;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{

    #region MovementData

    [SerializeField] const int walkableLayerID = 8;
    [SerializeField] const int enemyLayerID = 9;
    [SerializeField] const int stiffLayerID = 10;
    [SerializeField] const int raystopLayerID = -1;
    [SerializeField] const int npcLayerID = 11;

    /// <summary>
    /// Player animator controller
    /// </summary>
    [SerializeField] Animator player_animator;

    /// <summary>
    /// CameraRaycaster object
    /// </summary>
    [SerializeField] CameraRaycaster cameraRaycaster;

    /// <summary>
    /// Attack Range between player and monster or npc
    /// </summary>
    [SerializeField] float attackRange = 1f;

    /// <summary>
    /// Navmesh agent to control the player movements, saving also the player speed, movestopradius etc
    /// </summary>
    [SerializeField] NavMeshAgent m_navMeshAgent;

    /// <summary>
    /// This var is set by cameraraycaster when we detect and filter a click
    /// </summary>
    private Vector3 currentDestination;

    /// <summary>
    /// Current player_position, just for simplicity
    /// </summary>
    private Vector3 player_position;

    /// <summary>
    /// Public method to retrieve the player position from this class
    /// </summary>
    public Vector3 playerPos
    {
        get{ return player_position; }
    }

    public NavMeshAgent NavAgent
    {
        get { return this.m_navMeshAgent; }
        private set { }
    }
    #endregion

    #region UnityEvents
    /// <summary>
    /// Initializate the necessary data
    /// </summary>
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>(); //get component
        m_navMeshAgent = GetComponent<NavMeshAgent>(); // get component
        currentDestination = transform.position; //initialize position
        cameraRaycaster.notifyMouseClickObservers += RetrieveMouseClick;

        //State machine states initialize
        StateManager.isIdle = true;
        StateManager.isRunning = false;
        StateManager.isDead = false;
        StateManager.isCasting = -1;

    }

    private void RetrieveMouseClick(RaycastHit hit, int layerHit)
    {
        switch (layerHit)
        {
            case enemyLayerID:
                //Set the navAgent propertiies and destination
                GameObject enemy = hit.collider.gameObject;
                currentDestination = enemy.transform.position;
                MovePlayer();
                break;
            case walkableLayerID:
                currentDestination = hit.point;
                MovePlayer();
                break;

            case stiffLayerID:
                break;

            case npcLayerID:
                Debug.Log("We click a click on a npc layer");
                Interact(hit.collider.gameObject);
                break;
            /*case itemLayerID:
                break;*/

            default:
                Debug.LogWarning("Dont know how to handle the click");
                return;
        }
    }
    /// <summary>
    /// Fixed update is called in sync with physics
    /// </summary>
    private void FixedUpdate()
    {
        //Verify if the player is not dead yet so we can move
        if (!StateManager.isDead && currentDestination != Vector3.zero)
            MovePlayer();

    }


#endregion

    #region Mechanisms


    public void Interact(GameObject obj)
    {
        NPCType? npcType = obj.GetComponent<NPCActions>().npcType;
        if (npcType != null)
        {
            if (obj.transform.position != transform.position)
            {
                currentDestination = obj.transform.position;
                MovePlayer();
            }
            else
            {
                switch(npcType)
                {
                    case NPCType.QUEST_NPC:
                        obj.GetComponent<NPCActions>().ShowDialogue();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Move player according to the screen input point, filtered by layer and with player animations
    /// </summary>
    public void MovePlayer()
    {
        //Force the player to be quite while touching on the UI
        if (gameObject.GetComponent<StateManager>().isDraggingUI)
        {
            m_navMeshAgent.destination = transform.position;
            StateManager.isRunning = false;
            StateManager.isIdle = true;
            return;
        }

        //He's dead or is attacking so we can't move
        if (StateManager.isDead)
            return;

        //Is casting a spell that can be canceled?
        if (StateManager.isCasting != -1)
        {
            int skill_id = StateManager.isCasting;
            //After lets look if the spell has attribute of can be interupted and do it and return the func;
            return;
        }

        //Are we in the same position of our click target?
        if (currentDestination != transform.position)
        {
            //Set the navAgent propertiies and destination
            m_navMeshAgent.destination = currentDestination;
            m_navMeshAgent.angularSpeed = 1200;
            m_navMeshAgent.acceleration = 1000;

        }

        //Check if the player remaining distance is less than the stopping, if is near or not to the click end point
        if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
        {
            StateManager.isIdle = true;
            StateManager.isRunning = false;
        }
        else
        {
            StateManager.isIdle = false;
            StateManager.isRunning = true;
        }

        //Update character Y rotation
        UpdateRotation();
    }


    /// <summary>
    /// Updates the current transform rotation to the point
    /// </summary>
    private void UpdateRotation()
    {

        //Set the new rotation between the new destination and player
        Quaternion newRotation = Quaternion.LookRotation(currentDestination - transform.position);


        //we want just y axyz to rotate
        newRotation.z = 0;
        newRotation.x = 0;

        //rotate the player
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
    }

    /// <summary>
    /// Gizmos helpers to show the physics
    /// </summary>
    void OnDrawGizmos()
    {
        //Draw movement
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.15f);
        Gizmos.DrawSphere(m_navMeshAgent.destination, 0.10f);

        //Draw attack
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public bool InAttackRange(Vector3 opponent_pos)
    {
        return (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance);
    }
#endregion
    

    
}
