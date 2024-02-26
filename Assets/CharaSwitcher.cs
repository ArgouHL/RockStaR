using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class CharaSwitcher : MonoBehaviour
{
    int index = 0;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private PlayerInputManager manager;

    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = 0;
        manager.playerPrefab = players[index];
    }

    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        index++;
      
        manager.playerPrefab = players[index];
    }

}
