using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class MultiplayerManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    public List<Player> players = new List<Player>(); 

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    //This function is called every time a player joins the game. 
    public void AddPlayer()
    {
        //Whenever a player joins, this function is called. 
        //It searches for the player then adds them to a list. 
        Player player = FindFirstObjectByType<Player>();

        //just incase it detects player already within list.
        if(players.Contains(player)) { return; }

        else
        {
            //Otherwise add to list.
            players.Add(player);
        }

    }
    
}
