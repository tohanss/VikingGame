using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This script handles the opening and closing of portals in a room 
 */
public class PortalHandler : MonoBehaviour
{
    [SerializeField] private GameObject portalExit;
    [SerializeField] private GameObject portalEntry;
    [SerializeField] private RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager.OnBattleStarted += RoomManager_OnBattleStarted;
        roomManager.OnBattleOver += RoomManager_OnBattleOver;
    }

    private void RoomManager_OnBattleStarted(object sender, System.EventArgs e)
    {
        //Close transition Portal that was open
        Debug.Log("Entry Portal Closed");
        roomManager.OnBattleStarted -= RoomManager_OnBattleStarted;

    }

    private void RoomManager_OnBattleOver(object sender, System.EventArgs e)
    {
        //Open transition Portal so the player can go to the next room
        Debug.Log("Exit Portal Open");
        portalExit.GetComponent<SpriteRenderer>().color = Color.blue;
        portalExit.transform.GetChild(0).gameObject.SetActive(true);
        roomManager.OnBattleOver -= RoomManager_OnBattleOver;

    }

}
