using UnityEngine;
using System;
using System.Collections.Generic;


// DS for the current state within a turn .
public enum TurnState
{
    PlayersTurn,
    PlayerMoving,
    EnemysTurn,
    EnemyMoving
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance {get;private set;}

    //References 
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerMovement enemy;

    public TurnState CurrentState {get;private set;} = TurnState.PlayersTurn;

    public event Action<TurnState>OnStateChanged;

    private void Awake()
    {
        if (Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance= this;
    }

    private void OnEnable()
    {
        
        player.OnMovementCompleted+= HandlePlayerMovementCompleted;
        
        enemy.OnTurnCompleted+= HandleEnemyTurnCompleted;
        
    }

    private void OnDisable()
    {
          
        player.OnMovementCompleted-= HandlePlayerMovementCompleted;
        enemy.OnTurnCompleted-= HandleEnemyTurnCompleted;
        
    } 
    public bool CanPlayerAct=>CurrentState==TurnState.PlayersTurn;

    public void PlayerMove(List<Tile> path)
    {
        if (!CanPlayerAct||path==null || path.Count ==0)
        {
            return;
        }

        SetState(TurnState.PlayerMoving);
        player.MoveAlongPath(path);
    }

    private void HandlePlayerMovementCompleted()
    {
        if (enemy != null)
        {
            SetState(TurnState.EnemysTurn);
            SetState(TurnState.EnemyMoving);
            enemy.RunTurn();
        }
        else
        {
            SetState(TurnState.PlayersTurn);
        }
    }

    private void HandleEnemyTurnCompleted()
    {
        SetState(TurnState.PlayersTurn);
    }

    private void SetState(TurnState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}


