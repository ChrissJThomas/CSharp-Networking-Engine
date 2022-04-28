using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
=======
>>>>>>> refs/remotes/origin/master
using UnityEngine.UI;

namespace NetCORE {
    public class NetworkPlayer : Net_Component {

        public int color;
        public string name;
        public int shape;
<<<<<<< HEAD
        bool isActive;
        public GameObject canvasOb;
        public bool ready = false;
        public bool isScene2 = false;
=======

        bool isActive;
        public GameObject canvasOb;

        public bool ready = false;
>>>>>>> refs/remotes/origin/master
        public int tempcolor;
        public string tempname;

        void Start () {
<<<<<<< HEAD
            DontDestroyOnLoad (this.gameObject);

=======
>>>>>>> refs/remotes/origin/master
            isActive = true;
            StartCoroutine (CheckLocal ());
            GameObject.Find ("Main_Canvas").SetActive (false);
        }

        public IEnumerator CheckLocal () {
            while (true) {
                yield return new WaitForSeconds (0.1f);
                if (myId.isLocalPlayer) {
                    //isactive to make it so once i click ready it disappears 
                    canvasOb.SetActive (isActive);
                    shape = GameObject.FindGameObjectWithTag ("shapeDrop").GetComponent<Dropdown> ().value;
                    color = GameObject.FindGameObjectWithTag ("colorDrop").GetComponent<Dropdown> ().value;
<<<<<<< HEAD
                    name = GameObject.FindGameObjectWithTag ("nameBox").GetComponent<InputField> ().text;
=======
                    name =  GameObject.FindGameObjectWithTag ("nameBox").GetComponent<InputField> ().text;
>>>>>>> refs/remotes/origin/master
                } else {
                    canvasOb.SetActive (false);
                }
            }
        }

        public override void handle_Message (string var, string value) {
            if (var == "READY" && myCore.isServer) {
                try {
<<<<<<< HEAD
                    tempcolor = int.Parse (value.Split (',') [0]);
                    shape = int.Parse (value.Split (',') [1]);
                    tempname = value.Split (new char[] { ',' }, 3) [2];
=======
                    tempcolor =     int.Parse (value.Split (',') [0]);
                    shape =             int.Parse (value.Split (',') [1]);
                    tempname =   value.Split (new char[] { ',' }, 3) [2];
>>>>>>> refs/remotes/origin/master
                    ready = true;
                } catch (System.FormatException e) {
                    Debug.Log ("Error in parsing ready: " + e);
                }
            }
        }

<<<<<<< HEAD


=======
>>>>>>> refs/remotes/origin/master
        public void readyCallback () {
            if (myId.isLocalPlayer) {
                sendCommand ("READY", "" + color + "," + shape + "," + name);
                //isactive set false to make canvas object disappear 
                isActive = false;
            }
        }
    }
}