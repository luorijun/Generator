using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager: MonoBehaviour {

    public Player player;

    public Planet target;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {

        if (player.transform.hasChanged) {
            var position = player.transform.position;

            player.transform.position = Vector3.zero;
            target.transform.position -= position;

            player.transform.hasChanged = false;
        }
    }
}
