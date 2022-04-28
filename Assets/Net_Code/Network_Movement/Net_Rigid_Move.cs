using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {
    public class Net_Rigid_Move : Net_Component {
        Vector3 lastVelocity;

        // Use this for initialization
        void Start () {
            StartCoroutine (slowUpdate ());
        }

        public IEnumerator slowUpdate () {
            while (true) {
                if (myId.isLocalPlayer) {
                    float x = Input.GetAxisRaw ("Horizontal");
                    float z = Input.GetAxisRaw ("Vertical");
                    float y = Input.GetAxisRaw ("Jump");

                    Vector3 speed = new Vector3 (x * 2, y * 6, z * 2);
                    if (speed != lastVelocity) {
                        lastVelocity = speed;
                        sendCommand ("RIG_SPEED",
                            speed.x.ToString ("n2") + "," +
                            speed.y.ToString ("n2") + "," +
                            speed.z.ToString ("n2"));
                    }
                }
                yield return new WaitForSeconds (0.1f);
            }
        }
        public override void handle_Message (string var, string value) { }
    }
}