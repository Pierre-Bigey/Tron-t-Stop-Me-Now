using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Team
{
    BLUE,
    RED,
}

public class GlobalEvents
{
    public static Action<Team> PlayerLost;
    public static Action RoundStart;
    public static Action ShowScores;
    public static Action CountdownEnd;
}


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 4;
    private float base_speed;
    
    //Rotation speed in degrees per second
    [SerializeField] private float rotationSpeed = 50;

    [SerializeField] private Team team;

    [SerializeField] private GameObject PlayerMoto;
    [SerializeField] private ParticleSystem explosionSystem;
    
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    
    private Vector3 moveDirection;
    
    private PlayerInputActions playerInputActions;
    
    private InputAction moveAction;

    [SerializeField] private AudioSource explosionSource;
    

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        base_speed = speed;
        speed = 0;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    private void OnEnable()
    {
        switch (team)
        {
            case Team.RED:
                moveAction = playerInputActions.Tron.RedMove;
                break;
            case Team.BLUE:
                moveAction = playerInputActions.Tron.BlueMove;
                break;
        }
        moveAction.Enable();
        
        GlobalEvents.PlayerLost += OnPlayerLost;
        GlobalEvents.RoundStart += OnRoundStart;
        GlobalEvents.ShowScores += OnShowScore;
        GlobalEvents.CountdownEnd += OnCountdownEnd;
    }
    
    private void OnDisable()
    {
        moveAction.Disable();
        
        GlobalEvents.PlayerLost -= OnPlayerLost;
        GlobalEvents.RoundStart -= OnRoundStart;
        GlobalEvents.ShowScores -= OnShowScore;
        GlobalEvents.CountdownEnd -= OnCountdownEnd;
    }

    private void OnPlayerLost(Team _)
    {
        speed = 0;
    }

    private void OnShowScore()
    {
        PlayerMoto.SetActive(true);
    }

    private void OnRoundStart()
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
        moveDirection = transform.forward;
        PlayerMoto.SetActive(true);
    }

    private void OnCountdownEnd()
    {
        speed = base_speed;        
    }

    // Update is called once per frame
    void Update()
    {
        if(speed <= 0) return;
        
        Vector2 inputMoveDirection = moveAction.ReadValue<Vector2>();

        if (inputMoveDirection.x > 0.5)
        {
            moveDirection = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * moveDirection;
        }
        else if (inputMoveDirection.x < -0.5)
        {
            moveDirection = Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0) * moveDirection;
        }
        
        
        
        transform.LookAt(transform.position + moveDirection, Vector3.up);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        explosionSource.Play();
        Debug.Log("Player "+team+" collided with "+other.gameObject.name);
        GlobalEvents.PlayerLost.Invoke(team);
        PlayerMoto.SetActive(false);
        explosionSystem.Play();
    }
}