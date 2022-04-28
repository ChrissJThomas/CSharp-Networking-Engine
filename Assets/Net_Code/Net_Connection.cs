using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NetCORE {

    public class Net_Connection {
        public Socket connection;
        public int playerId;
        public byte[] buffer = new byte[1024];
        public StringBuilder sb = new StringBuilder ();
        public bool didReceive = false;
        public Net_Core myCore;
        // Use this for initialization
        void Start () {

        }
        //This actually sends the messages for this particular client
        public void Send (byte[] byteData) {
            connection.BeginSend (byteData, 0, byteData.Length, 0,
                new System.AsyncCallback (this.SendCallBack), connection);
        }

        private void SendCallBack (System.IAsyncResult ar) {
            try {

            } catch (System.Exception) {

            }
        }

        //This receives the messages for this particular client.
        public IEnumerator Receive () {
            //This is going to act as our "thread"
            while (true) {
                try {
                    connection.BeginReceive (buffer, 0, 1024, 0,
                        new System.AsyncCallback (ReceiveCallback), this);
                } catch (System.Exception e) {
                    Debug.Log (e.ToString ());
                    // myCore.Disconnect(this);
                }
                yield return new WaitUntil (() => didReceive);
                Debug.Log ("We got INFO");
                string response = sb.ToString ();
                if (response.Trim (' ') == "") {
                    Debug.Log ("We got nothing? - assuming socket went down");
                    //no empty strings wanter, problems will be caused
                    myCore.Disconnect (this);
                    yield break;
                }
// Debug.Log (response);
                //parse string
                string[] commands = response.Split ('\n');
                for (int i = 0; i < commands.Length; i++) {
                    if (commands[i].Trim (' ') == "") {
                        continue;
                    }
                    if (commands[i].Trim (' ') == "OK" && myCore.isClient) {
                        Debug.Log ("Client Received OK.");
                    } else if (commands[i].StartsWith ("PlayerID") && myCore.isClient) {
                        //gives client their player ID
                        playerId = int.Parse (commands[i].Split ('_') [1]);
                        myCore.localPlayerId = playerId;
                    } else if (commands[i].StartsWith ("DIRTY") && myCore.isServer) {
                        string[] args = commands[i].Split ('_');
                        Net_Component[] AllScripts = myCore.netObjs[int.Parse (args[1])].obj.GetComponents<Net_Component> ();
                        for (int j = 0; j < AllScripts.Length; j++) {
                            AllScripts[j].isDirty = true;
                        }
                    } else if (commands[i].StartsWith ("CREATE") && myCore.isClient) {
                        try {
                            string[] args = commands[i].Split ('_');
                            int type = int.Parse (args[1]);
                            int owner = int.Parse (args[2]);
                            int netid = int.Parse (args[3]);
                            GameObject np = GameObject.Instantiate (myCore.spawnPrefabs[type]);
                            np.GetComponent<Net_ID> ().owner = owner;
                            np.GetComponent<Net_ID> ().netId = netid;
                            myCore.netObjs.Add (netid, new Net_Objects (np, type, owner, netid));
                            myCore.masterMessage += "DIRTY_" + netid.ToString () + "\n";
                        } catch (System.Exception e) {
                            Debug.Log ("Exception Occured in object spawning - " + e.Message);
                        }
                    } else if (commands[i].StartsWith ("DELETE_") && myCore.isClient) {
                        int badId = int.Parse (commands[i].Split ('_') [1]);
                        GameObject.Destroy (myCore.netObjs[badId].obj);
                        myCore.netObjs.Remove (badId);
                    } else {
                        Debug.Log ("Are we here yet");
                        myCore.UpdateNetObjects (commands[i]);
                    }
                }
                sb.Length = 0;
                didReceive = false;
                yield return new WaitForSeconds (0.01f);
                //close while(true)
            }

        }

        private void ReceiveCallback(System.IAsyncResult ar)
        {
            try
            {
                Net_Connection temp = (Net_Connection)ar.AsyncState;
                int bytesRead = -1; 
                bytesRead = connection.EndReceive(ar); 
                if (bytesRead > 0)
            {
            temp.sb.Append(Encoding.ASCII.GetString(temp.buffer, 0, bytesRead));
            string ts = temp.sb.ToString();
            if(ts[ts.Length-1]!='\n')
            {
                connection.BeginReceive(buffer, 0, 1024, 0,new System.AsyncCallback(ReceiveCallback), this);
            }
            else
            {
                temp.didRecieve = true;
            }
        }
    }
}