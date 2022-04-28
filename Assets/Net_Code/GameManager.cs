using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
=======
>>>>>>> refs/remotes/origin/master

namespace NetCORE {

	public class GameManager : Net_Component {

		public bool gameStart = false;
<<<<<<< HEAD
		bool isScene2;
=======

>>>>>>> refs/remotes/origin/master
		public bool check () {
			NetworkPlayer[] myGames = GameObject.FindObjectsOfType<NetworkPlayer> ();
			if (myGames.Length == 0) {
				Debug.Log ("not ready, no players");
				return false;
			}
			for (int i = 0; i < myGames.Length; i++) {
				if (myGames[i].ready == false) {
					return false;
				}
			}
			return true;
		}
<<<<<<< HEAD
		public bool checkScene () {
			NetworkPlayer[] switchSceneGames = GameObject.FindObjectsOfType<NetworkPlayer> ();
			if (switchSceneGames.Length == 0) {
				return false;
			}
			for (int i = 0; i < switchSceneGames.Length; i++) {
				if (switchSceneGames[i].isScene2 == false) {
					return false;
				}
			}
			Debug.Log ("all switched scene");
			return true;
		}
=======
>>>>>>> refs/remotes/origin/master

		// Use this for initialization
		void Start () {
			StartCoroutine (SlowStart ());
<<<<<<< HEAD
			DontDestroyOnLoad (this.gameObject);
=======
>>>>>>> refs/remotes/origin/master
		}

		IEnumerator SlowStart () {
			yield return new WaitUntil (() => check ());
<<<<<<< HEAD

			sendUpdate ("SWITCH", "1");
			SwitchScene (1);

			NetworkPlayer[] readyGames = GameObject.FindObjectsOfType<NetworkPlayer> ();
			// yield return new WaitUntil (() => checkScene ());
			//phase 3

			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < readyGames.Length; i++) {

				GameObject temp = myCore.NetCreateObject (1 + readyGames[i].shape, readyGames[i].myId.owner, new Vector3 (Random.Range (-5, 5), 1, Random.Range (-5, 5)));
				temp.GetComponent<PlayerController> ().SetColor (readyGames[i].tempcolor);
				temp.GetComponent<PlayerController> ().setName (readyGames[i].tempname);
			}

=======
			NetworkPlayer[] readyGames = GameObject.FindObjectsOfType<NetworkPlayer> ();
			//phase 3

			for (int i = 0; i < readyGames.Length; i++) {
				GameObject temp = myCore.NetCreateObject (1 + readyGames[i].shape, readyGames[i].myId.owner, Vector3.zero);
				//these code blocks to to the playercontroller script
				temp.GetComponent<PlayerController> ().SetColor (readyGames[i].tempcolor);
				temp.GetComponent<PlayerController> ().setName (readyGames[i].tempname);
			}
>>>>>>> refs/remotes/origin/master
		}

		// Update is called once per frame
		void Update () {

		}

<<<<<<< HEAD
		public void SwitchScene (int val) {
			// if (myId.isLocalPlayer) {
			Debug.Log ("trying to switch scene");
			SceneManager.LoadScene (val);
			isScene2 = true;
			// }
		}

		public override void handle_Message (string var, string value) {
			if (var == "SWITCH" && myCore.isClient) {
				SwitchScene (int.Parse (value));
			}
			if (var == "SWITCH" && myCore.isServer) {

			}
=======
		public override void handle_Message (string var, string value) {

>>>>>>> refs/remotes/origin/master
		}
	}
}