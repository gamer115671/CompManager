using System;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using UnityEngine.XR;
using UnityEngine;
namespace GameManager
{
    [BepInPlugin("org.bepinex.plugins.GameManager", "GameManager", "1.0.0.0")]

    public class MyPatcher : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = new Harmony("com.kfc.monkeytag.GameManager");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        public void OnGUI()
        {
            if (GUI.Button(new Rect(200, 20, 160, 20), "Game Manager"))
            {
                ShowHide = !ShowHide;
            }
            if (ShowHide)
            {


                if (PhotonNetwork.CurrentRoom != null && !PhotonNetwork.CurrentRoom.IsVisible)
                {
                    GUI.Box(new Rect(200, 40, 220, PhotonNetworkController.instance.currentGorillaParent.GetComponentsInChildren<VRRig>().Length * 30 + 40), "");
                    int i = 1;
                    foreach (VRRig player in PhotonNetworkController.instance.currentGorillaParent.GetComponentsInChildren<VRRig>())
                    {
                        object obj;
                        Player player2 = this.FindPlayerforVRRig(player);
                        Color col = GUI.color;
                        

                        
                        
                        if (player.setMatIndex != 0)
                        {
                            GUI.color = Color.red;
                        }
                        

                        
                        GUI.Box(new Rect(200, 20 + (i * 30), 160, 20), player2.NickName);
                        GUI.color = col;
                        if (GUI.Button(new Rect(370 , 20 + (i * 30), 40, 20), "Tag"))
                        {
                            foreach (var ply in PhotonNetwork.PlayerList)
                            {
                                PhotonView.Get(GorillaTagManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                                {
                                        ply,
                                        player2
                                });
                            }
                        }

                        i++;
                    }
                    if(GUI.Button(new Rect(200, (i - 1)*30 + 50 , 160, 20 ), "Reset Round")){
                        foreach (var ply1 in PhotonNetwork.PlayerList)
                        {
                            foreach (var ply2 in PhotonNetwork.PlayerList)
                            {
                                PhotonView.Get(GorillaTagManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
                                {
                                            ply1,
                                            ply2
                                });
                            }
                        }
                    }
                }
                else
                {
                    GUI.Box(new Rect(200, 40, 160, 20), "Not In room");
                }
            }
            
        }


        public Player FindPlayerforVRRig(VRRig vRRig)
        {
            if (vRRig.photonView != null && vRRig.photonView.Owner != null)
            {
                return vRRig.photonView.Owner;
            }
            return null;
        }
        public static bool ShowHide = false;
    }

    public class PrimaryManager : MonoBehaviour
    {
        
                        

            

    }
 
}



