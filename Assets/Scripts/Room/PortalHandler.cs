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
    [SerializeField] private BossRoomManager bossRoomManager;

    // Material related
    private Material matActive;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //roomManager.OnBattleStarted += RoomManager_OnBattleStarted;
        if (roomManager != null)
        {
            roomManager.OnBattleOver += RoomManager_OnBattleOver;
        }
        if(bossRoomManager != null)
        {
            bossRoomManager.OnBattleOver += BossRoomManager_OnBattleOver;
        }
        if (portalExit != null)
        {
            spriteRenderer = portalExit.GetComponent<SpriteRenderer>();
        }
        matActive = Resources.Load("Materials/exit-portal", typeof(Material)) as Material;
    }

   
    private void BossRoomManager_OnBattleOver(object sender, System.EventArgs e)
    {
        //Floor Cleared, do something

        print("Victorious!");
        print("Floor Cleared");
        bossRoomManager.OnBattleOver -= BossRoomManager_OnBattleOver;
    }

    /*private void RoomManager_OnBattleStarted(object sender, System.EventArgs e)
    {
        //Close transition Portal that was open
        //Debug.Log("Entry Portal Closed");
        roomManager.OnBattleStarted -= RoomManager_OnBattleStarted;

    }*/

    private void RoomManager_OnBattleOver(object sender, System.EventArgs e)
    {
        //Open transition Portal so the player can go to the next room
        Debug.Log("Exit Portal Open");
        if (portalExit != null)
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.material = matActive;
            portalExit.transform.GetChild(0).gameObject.SetActive(true);
            portalExit.transform.GetChild(1).gameObject.SetActive(true);

        }
        roomManager.OnBattleOver -= RoomManager_OnBattleOver;

    }

}
