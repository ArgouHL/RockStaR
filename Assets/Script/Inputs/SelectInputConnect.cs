using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(1)]
public class SelectInputConnect : MonoBehaviour
{
    [SerializeField] internal PlayerCtr[] playerCtrs;


    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += ConnectInputs;
    }


    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= ConnectInputs;
    }

    public void ConnectInputs(PlayerInput input)
    {
        // int index = PlayerConfigManager.instance.GetPlayerCount();
        Debug.Log("Player " + input.playerIndex + "Connected");


        var config = PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex);
        playerCtrs[input.playerIndex].SetInput(config);

        PlayerConfigManager.instance.ChangeActionMap(InputType.selectChara);
        var skinmgr = playerCtrs[input.playerIndex].GetComponent<PlayerSkinManagment>();
        CharaSwitcher.instance.AddPlayerSkinManagment(playerCtrs[input.playerIndex].GetComponent<PlayerSkinManagment>());
        CharaTeamManger.instance.AddTeamCtr(playerCtrs[input.playerIndex].GetComponent<CharaTeamCtr>());

    }



    //public void PlayerJoin(PlayerInput playerInput)
    //{
    //    Debug.Log("Player" + playerInput.playerIndex + " Joined");

    //    var config = new PlayerConfig(playerInput);

    //    playerInput.transform.SetParent(transform);


    //    playerCtrs[0].SetInput(config);



    //}
}
