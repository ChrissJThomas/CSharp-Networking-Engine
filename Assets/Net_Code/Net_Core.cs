using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//required for networking
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetCORE {
    public struct Net_Objects {
        public GameObject obj;
        public int type;
        public int owner;
        public int netId;

        public Net_Objects (GameObject np, int TYPE, int OWNER, int NETID) : this () {
            this.obj = np;
            this.type = TYPE;
            this.owner = OWNER;
            this.netId = NETID;
        }
    }

    public class Net_Core : MonoBehaviour {

        //We will need some sort of struct for game objects... more on this later
<<<<<<< HEAD
        public Dictionary<int, Net_Connection> Connections;
=======
        public Dictionary<int, Demo_Connection> Connections;
>>>>>>> refs/remotes/origin/master
        // We will need a list of prefabs we can build
        public Dictionary<int, Net_Objects> netObjs;
        public GameObject[] spawnPrefabs;

        //  public List<Net_Objects> netObjs;
        //We will need a list of Connections.
        // Use this for initialization
        //we will need a list of stats:
        //Max connections (server only)
        public int maxConnections;
        //IP address
        public string ipAddress;
        //Port
        public int port;
        //Are we a server?
        public bool isServer;
        //Are we a client?
        public bool isClient;
        //Are we connecting?
        public bool isConnected;

        //What is our local player id? (Used later)
        public int localPlayerId;
        public bool currentlyConnecting;
        //this stores all synced messages
        public string masterMessage;
        public bool acceptMorePlayers = true;
        //index for connections
        public int conCounter = 0;
        //index for objects
        public int objCounter = 0;
        public Socket listener;

        void Start () {
            isServer = false;
            isClient = false;
            isConnected = false;
            if (ipAddress == "") {
                ipAddress = "127.0.0.1"; //localhost
            }
            if (port == 0) {
                port = 6969;
            }
            //Initialize the list of connections.
<<<<<<< HEAD
            Connections = new Dictionary<int, Net_Connection> ();
            netObjs = new Dictionary<int, Net_Objects> ();
            DontDestroyOnLoad (this.gameObject);
=======
            Connections = new Dictionary<int, Demo_Connection> ();
            netObjs = new Dictionary<int, Net_Objects> ();

>>>>>>> refs/remotes/origin/master
        }

        /// <summary>
        /// Start of Server Code
        /// </summary>
        public void StartServer () {
            if (!isConnected) {
                StartCoroutine (Listen ());
                StartCoroutine (slowUpdate ());
                NetCreateObject (4, -1, Vector3.zero);
            }
        }
        public IEnumerator Listen () {
            //if listening, this is the server
            isServer = true;
            isConnected = true;
            isClient = false;
            localPlayerId = -1;
            //for server localplayer id is -1
            //initialize listen port
            IPAddress ip = (IPAddress.Parse (ipAddress));
            IPEndPoint endP = new IPEndPoint (ip, port);
            //tcp for this server
            listener = new Socket (ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind (endP);
            listener.Listen (maxConnections);
            while (acceptMorePlayers) {
                Debug.Log ("Now listening");
                listener.BeginAccept (new System.AsyncCallback (this.AcceptCallBack), listener);
                yield return new WaitUntil (() => currentlyConnecting || !acceptMorePlayers);

                Connections[conCounter - 1].Send (Encoding.ASCII.GetBytes ("PlayerID_" + Connections[conCounter - 1].playerId + "\n"));

                StartCoroutine (Connections[conCounter - 1].Receive ());
                currentlyConnecting = false;

                //spawn all network objects
                foreach (KeyValuePair<int, Net_Objects> entry in netObjs) {
                    Connections[conCounter - 1].Send (Encoding.ASCII.GetBytes ("CREATE_" + entry.Value.type +
                        "_" + entry.Value.owner + "_" + entry.Value.netId + "\n"));
                    entry.Value.obj.GetComponent<Net_Component> ().isDirty = true;
                }
                //create player object

                Debug.Log ("creating canvas");
                NetCreateObject (0, conCounter - 1, Vector3.zero);
                // GameObject.Find("Main_Canvas").SetActive(false);
            }

        }

        //This will create the Connection on the server.
        public void AcceptCallBack (System.IAsyncResult ar) {
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept (ar);
<<<<<<< HEAD
            Net_Connection temp = new Net_Connection ();
=======
            Demo_Connection temp = new Demo_Connection ();
>>>>>>> refs/remotes/origin/master
            temp.connection = handler;
            temp.playerId = conCounter;
            conCounter++;
            temp.myCore = this;
            Connections.Add (temp.playerId, temp);
            Debug.Log ("There are now " + Connections.Count + " Player(s) connected.");
            currentlyConnecting = true;

        }

        ////////////////////END OF SERVER CODE ////////////////////////

        ///////////////////////Start of Client Code////////////////////
        public void StartClient () {
            if (!isConnected) {
                StartCoroutine (ConnectClient ());
            }
        }
        public IEnumerator ConnectClient () {
            isClient = true;
            isServer = false;
            //set up socket
            IPAddress ip = (IPAddress.Parse (ipAddress));
            IPEndPoint endP = new IPEndPoint (ip, port);
            Socket clientSocket = new Socket (ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //connect the client
            clientSocket.BeginConnect (endP, OnConnected, clientSocket);
            //wait for client to connect
            yield return new WaitUntil (() => currentlyConnecting);

            StartCoroutine (Connections[0].Receive ());
            StartCoroutine (slowUpdate ());
        }

        public void OnConnected (System.IAsyncResult ar) {
<<<<<<< HEAD
            Net_Connection temp = new Net_Connection ();
=======
            Demo_Connection temp = new Demo_Connection ();
>>>>>>> refs/remotes/origin/master
            temp.connection = (Socket) ar.AsyncState;
            temp.connection.EndConnect (ar);
            currentlyConnecting = true;
            isConnected = true;
            temp.myCore = this;
            Connections.Add (0, temp);
        }
        //////////////////END OF CLIENT CONNECT CODE ////////////////////////////

        /// <summary>
        ///  Handles the buisness logic for the network
        ///  Such as gathering messages and sending them
        /// </summary>
        /// <returns></returns>
        /// 

        public void UpdateNetObjects (string msg) {
            try {
                string[] commands = msg.Split ('_');
                if (netObjs.ContainsKey (int.Parse (commands[1]))) {
                    netObjs[int.Parse (commands[1])].obj.GetComponent<Net_ID> ().Net_Update (commands[0], commands[2], commands[3]);
                }
            } catch (System.Exception) { }
        }
        public IEnumerator slowUpdate () {
            while (true) {
                //find all network objects
                Net_ID[] networkObjs = GameObject.FindObjectsOfType<Net_ID> ();
                //Get current messages - will work on server or client
                for (int i = 0; i < networkObjs.Length; i++) {
                    //add messages to the masterMessage that is sent
                    // Debug.Log(networkObjs[i].gameObjectMessages);
                    masterMessage += networkObjs[i].gameObjectMessages + "\n";
                    //clear game objects messages
                    networkObjs[i].gameObjectMessages = "";
                }
                //if master message is not empty, send message to all connections
                //will only be one for client, and all for the server
                masterMessage = masterMessage.Trim ();
                if (masterMessage != "") {
<<<<<<< HEAD
                    foreach (KeyValuePair<int, Net_Connection> entry in Connections) {
=======
                    foreach (KeyValuePair<int, Demo_Connection> entry in Connections) {
>>>>>>> refs/remotes/origin/master
                        try {
                            Connections[entry.Key].Send (Encoding.ASCII.GetBytes (masterMessage));
                        } catch (System.Exception) {
                            //assume client has disconnected
                            if (Connections.ContainsKey (entry.Key)) {
                                Disconnect (Connections[entry.Key]);
                            }
                        }
                    }
                    masterMessage = "";
                }
                yield return new WaitForSeconds (.1f);
            }

        }

<<<<<<< HEAD
        public void Disconnect (Net_Connection bad) {
=======
        public void Disconnect (Demo_Connection bad) {
>>>>>>> refs/remotes/origin/master
            if (isClient) {
                bad.connection.Shutdown (SocketShutdown.Both);
                bad.connection.Close ();
                this.isClient = false;
                this.isServer = false;
                this.isConnected = false;
                this.localPlayerId = -10;
                foreach (KeyValuePair<int, Net_Objects> obj in netObjs) {
                    Destroy (obj.Value.obj);
                }
                netObjs.Clear ();
                Connections.Clear ();
            }

            if (isServer) {
                if (bad.connection.Connected) {
                    bad.connection.Shutdown (SocketShutdown.Both);
                    bad.connection.Close ();
                }
                OnClientDisc (bad);
            }
        }

<<<<<<< HEAD
        public void OnClientDisc (Net_Connection bad) {
=======
        public void OnClientDisc (Demo_Connection bad) {
>>>>>>> refs/remotes/origin/master
            if (isServer) {
                Connections.Remove (bad.playerId);
                List<int> badObjs = new List<int> ();
                foreach (KeyValuePair<int, Net_Objects> obj in netObjs) {
                    if (obj.Value.owner == bad.playerId) {
                        badObjs.Add (obj.Key);
                        //add the key toa  temp list and delet it outside of the for loop
                    }
                }
                for (int i = 0; i < badObjs.Count; i++) {
                    NetDestroyObject (badObjs[i]);
                }
            }
        }

        public void LeaveGame () {
            if (isClient && isConnected) {
                Disconnect (Connections[0]);
            }

            if (isServer && isConnected) {
                isServer = false;
<<<<<<< HEAD
                foreach (KeyValuePair<int, Net_Connection> entry in Connections) {
=======
                foreach (KeyValuePair<int, Demo_Connection> entry in Connections) {
>>>>>>> refs/remotes/origin/master
                    Disconnect (entry.Value);
                }
                foreach (KeyValuePair<int, Net_Objects> obj in netObjs) {
                    Destroy (obj.Value.obj);
                }
                netObjs.Clear ();
                Connections.Clear ();
                listener.Close ();

                isClient = false;
                isConnected = false;
                StopAllCoroutines ();
            }
        }

        public void OnApplicationQuit () {
            LeaveGame ();
        }

        //////////////////////////////////////// Object spawning and destruction ////////////////////////////////////
        public void NetDestroyObject (int netId) {
            Destroy (netObjs[netId].obj);
<<<<<<< HEAD
            foreach (KeyValuePair<int, Net_Connection> entry in Connections) {
=======
            foreach (KeyValuePair<int, Demo_Connection> entry in Connections) {
>>>>>>> refs/remotes/origin/master
                entry.Value.connection.Send (Encoding.ASCII.GetBytes ("DELETE_" + netId + "\n"));
            }
            netObjs.Remove (netId);
        }

        public GameObject NetCreateObject (int type, int ownMe, Vector3 initPos) {
            GameObject np;
            lock (this) {
                np = GameObject.Instantiate (spawnPrefabs[type], initPos, Quaternion.identity);
                np.GetComponent<Net_ID> ().owner = ownMe;
                np.GetComponent<Net_ID> ().netId = objCounter;
                objCounter++;
                //CREATE_<TYPE>_<OWNER>_<NETID>
                string msg = "CREATE_" + type + "_" + ownMe +
                    "_" + (objCounter - 1) + "_" + initPos.x.ToString ("n2") + "_" + initPos.y.ToString ("n2") + "_" + initPos.z.ToString ("n2") + "\n";
                netObjs.Add (np.GetComponent<Net_ID> ().netId,
                    new Net_Objects (np, type, ownMe, objCounter - 1));
                masterMessage += msg;
            }
            return np;
        }
    }
}