using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {
    [RequireComponent (typeof (Net_ID))]
    public abstract class Net_Component : MonoBehaviour {
        //used by the server when a new player joins
        public bool isDirty; // Start is called before the first frame update
        //pointer back to network core
        public Net_Core myCore;
        //pointer to current gameobject id
        public Net_ID myId;

        public void Awake () {
            myId = gameObject.GetComponent<Net_ID> ();
            myCore = GameObject.FindObjectOfType<Net_Core> ();
        }

        public void sendCommand (string var, string value) {
            var = var.Replace ('_', ' ');
            value = value.Replace ('_', ' ');

            if (myCore != null && myCore.isClient) {
                Debug.Log ("D");
                string msg = "COMMAND_" + myId.netId + "_" + var + "_" + value;
                myId.addMsg (msg);
            }
            Debug.Log ("E");
        }
        public void sendUpdate (string var, string value) {
            if (myCore != null && myCore.isServer) {
                string msg = "UPDATE_" + myId.netId + "_" + var + "_" + value;
                myId.addMsg (msg);
            }
        }

        //must be implemented individually by inherited behavior
        public abstract void handle_Message (string var, string value);
    }
}