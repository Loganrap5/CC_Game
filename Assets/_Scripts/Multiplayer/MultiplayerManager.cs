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


    //DEBUG TESTING
    public List<string> names = new List<string>();
    //DEBUG TESTING

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        //DEBUG TESTING
        names.AddRange(new string[] { "John", "Mason", "Morgan", "Susan" });
        //DEBUG TESTING
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
            //DEBUG TESTING
            player.name = names[Random.Range(1, 3)];
            //DEBUG TESTING

            //Otherwise add to list.
            players.Add(player);
            
        }

    }
    
}
