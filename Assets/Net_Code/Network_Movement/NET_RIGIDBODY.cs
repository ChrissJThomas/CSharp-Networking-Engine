using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {
    public class NET_RIGIDBODY : Net_Component {
        private Rigidbody myRig;
        public Vector3 lastPosition, lastVelocity, lastRotation, lastAngVelocity;
        public bool syncRotation;
        public bool syncAngularSpeed;

        // Use this for initialization
        void Start () {
            if (gameObject.GetComponent<Rigidbody> () == null) {
                myRig = gameObject.AddComponent<Rigidbody> ();
            } else {
                myRig = gameObject.GetComponent<Rigidbody> ();
            }

            lastPosition = myRig.position;
            lastVelocity = myRig.velocity;
            lastRotation = myRig.rotation.eulerAngles;
            lastAngVelocity = myRig.angularVelocity;

            StartCoroutine (slowUpdate ());
        }

        public IEnumerator slowUpdate () {
            while (true) {
                if (myCore.isServer) {
                    if (syncRotation && lastRotation != myRig.rotation.eulerAngles || isDirty) {
                        string values =
                            myRig.rotation.eulerAngles.x.ToString ("n2") + "," +
                            myRig.rotation.eulerAngles.y.ToString ("n2") + "," +
                            myRig.rotation.eulerAngles.z.ToString ("n2");

                        sendUpdate ("ROT", values);
                        lastRotation = myRig.rotation.eulerAngles;
                        isDirty = false;
                    }
                    if (lastPosition != myRig.position || lastVelocity != myRig.velocity || isDirty) {
                        string values =
                            myRig.position.x.ToString ("n2") + "," +
                            myRig.position.y.ToString ("n2") + "," +
                            myRig.position.z.ToString ("n2") + "," +
                            myRig.velocity.x.ToString ("n2") + "," +
                            myRig.velocity.y.ToString ("n2") + "," +
                            myRig.velocity.z.ToString ("n2");

                        sendUpdate ("POS", values);
                        lastPosition = myRig.position;
                        lastVelocity = myRig.velocity;
                        isDirty = false;
                    }

                    if (syncAngularSpeed && lastAngVelocity != myRig.angularVelocity || isDirty) {
                        string values =
                            myRig.angularVelocity.x.ToString ("n2") + "," +
                            myRig.angularVelocity.y.ToString ("n2") + "," +
                            myRig.angularVelocity.z.ToString ("n2");

                        sendUpdate ("ANG", values);
                        lastAngVelocity = myRig.angularVelocity;
                        isDirty = false;
                    }
                }
                yield return new WaitForSeconds (0.1f);
            }
        }

        public void changePosition (Vector3 pos) {
            if (myCore.isServer) {
                myRig.position = pos;
            }
        }

        public void changeVelocity (Vector3 vel) {
            if (myCore.isServer) {
                myRig.velocity = vel;
            }
        }

        public void changeRotation (Vector3 rot) {
            if (myCore.isServer) {
                myRig.rotation = Quaternion.Euler (rot);
            }
        }

        public void changeAngVel (Vector3 ang) {
            if (myCore.isServer) {
                myRig.angularVelocity = ang;
            }
        }

        public override void handle_Message (string var, string value) {
            if (myCore.isClient) {
                if (var == "POS") {
                    string[] args = value.Split (',');
                    Debug.Log (value);
                    if (args.Length == 6) {
                        Vector3 p = new Vector3 (float.Parse (args[0]), float.Parse (args[1]), float.Parse (args[2]));
                        Vector3 v = new Vector3 (float.Parse (args[3]), float.Parse (args[4]), float.Parse (args[5]));
                        myRig.position = p;
                        myRig.velocity = v;
                    }
                }

                if (var == "ROT") {
                    string[] args = value.Split (',');
                    if (args.Length == 3) {
                        Vector3 r = new Vector3 (float.Parse (args[0]), float.Parse (args[1]), float.Parse (args[2]));
                        myRig.rotation = Quaternion.Euler (r);
                    }
                }

                if (var == "ANG") {
                    string[] args = value.Split (',');
                    if (args.Length == 3) {
                        Vector3 a = new Vector3 (float.Parse (args[0]), float.Parse (args[1]), float.Parse (args[2]));
                        myRig.angularVelocity = a;
                    }
                }
            }

            if (myCore.isServer) {
                if (var == "RIG SPEED") {
                    Vector3 v = new Vector3 (
                        float.Parse (value.Split (',') [0]),
                        float.Parse (value.Split (',') [1]),
                        float.Parse (value.Split (',') [2]));
                    myRig.velocity = v;
                }

                if (var == "RIG ROT") {
                    string[] args = value.Split (',');
                    if (args.Length == 3) {
                        Vector3 r = new Vector3 (float.Parse (args[0]), float.Parse (args[1]), float.Parse (args[2]));
                        myRig.rotation = Quaternion.Euler (r);
                    }
                }
                if (var == "RIG ANG") {
                    string[] args = value.Split (',');
                    if (args.Length == 3) {
                        Vector3 angVel = new Vector3 (float.Parse (args[0]), float.Parse (args[1]), float.Parse (args[2]));
                        myRig.angularVelocity = angVel;
                    }
                }
            }
        }
    }
}