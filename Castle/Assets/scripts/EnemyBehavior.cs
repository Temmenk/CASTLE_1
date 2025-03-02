using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public Transform patrolRoute;
    public List<Transform> locations;
    
    private int locationIndex = 0;
    private NavMeshAgent agent;

    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
            }
        }
    }

    private GameBehavior _gameManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    private void Update()
    {
        // Patrol behavior
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            MoveToNextPatrolLocation();
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0) return;
        agent.destination = locations[locationIndex].position;
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    private void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("Player detected - attack!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
            MoveToNextPatrolLocation();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit!");
        }
    }

    void AttackPlayer()
    {
        int damage = 1;
        if (_gameManager.Armor > 0)
        {
            _gameManager.Armor -= damage;
        }
        else
        {
            _gameManager.PlayerHealth -= damage;
        }

        _gameManager.UpdateMessage("Enemy hit you! Health: " + _gameManager.PlayerHealth);

        if (_gameManager.PlayerHealth <= 0)
        {
            _gameManager.GameOver();
        }
    }
}
