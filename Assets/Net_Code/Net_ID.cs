using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {
    public class Net_ID : MonoBehaviour {
        public int netId = -1;
        public int owner = -2;
        public bool isLocalPlayer = false;
        public Net_Core myCore;
        public string gameObjectMessages = "";
        // Use this for initialization
        void Start () {
            myCore = GameObject.FindObjectOfType<Net_Core> ();
            StartCoroutine (Init ());
        }

        public IEnumerator Init () {
            yield return new WaitUntil (() => netId != -1 && owner != -2);
            if (myCore.localPlayerId == owner) {
                isLocalPlayer = true;
            }
        }

        public void Net_Update (string type, string var, string value) {
            if ((myCore.isServer && type == "COMMAND") || (myCore.isClient && type == "UPDATE")) {
                Net_Component[] myNets = gameObject.GetComponents<Net_Component> ();
                Debug.Log ("We are in here net_update");
                for (int i = 0; i < myNets.Length; i++) {
                    myNets[i].handle_Message (var, value);
                }
            }
        }

        public void addMsg (string msg) {
            Debug.Log ("F - addmsg");
            gameObjectMessages += (msg + "\n");
        }
    }
}