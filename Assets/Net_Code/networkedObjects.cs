using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {

    public class networkedObjects : Net_Component {

        public int score;
        void Start () {

        }

        public override void handle_Message (string var, string value) {
            if (myCore.isServer) {
                if (var == "IncScore") {
                    score++;
                    sendUpdate ("UIncScore", this.score.ToString ());
                }

            }
            if (myCore.isClient) {
                if (var == "UIncScore") {
                    score = int.Parse (value);
                }
            }
        }

        // Start is called before the first frame update

        // Update is called once per frame
        void Update () {
            Debug.Log ("A");
            if (myId.isLocalPlayer) {
                Debug.Log ("B");
                if (Input.GetAxisRaw ("Jump") > 0.0) {
                    Debug.Log ("C");
                    sendCommand ("IncScore", "1");
                }
            }
        }
    }
}