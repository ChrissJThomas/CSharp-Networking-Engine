using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetCORE {
	public class PlayerController : Net_Component {

		public Material[] colorP;
		public string playerName;
		public int dropColor;

		void Start () {
			StartCoroutine (SlowUpdate ());
		}

<<<<<<< HEAD
=======
		//these functions are called from networkPlayer in order for this script to get name and color
>>>>>>> refs/remotes/origin/master
		public void SetColor (int c) {
			if (myCore.isServer) {
				dropColor = c;
			}
		}
<<<<<<< HEAD
		
=======
>>>>>>> refs/remotes/origin/master
		public void setName (string n) {
			if (myCore.isServer) {
				playerName = n;
			}
		}

		public IEnumerator SlowUpdate () {
			while (true) {
				if (myCore.isServer && isDirty) {
					//send info back to client
					sendUpdate ("COLOR", dropColor.ToString ());
					sendUpdate ("NAME", playerName);
					isDirty = false;
				}
				yield return new WaitForSeconds (0.1f);
			}
		}

		public override void handle_Message (string var, string value) {
			if (var == "COLOR") {
				try {
					dropColor = int.Parse (value);
					this.gameObject.GetComponent<Renderer> ().material = colorP[dropColor];
				} catch (System.FormatException e) {
					Debug.Log ("Error in parsing color: " + e);
				}
			}

			if (var == "NAME") {
				try {
					playerName = value;
					this.gameObject.transform.GetChild (0).GetComponent<TextMesh> ().text = playerName;
				} catch (System.FormatException e) {
					Debug.Log ("errror getting name: " + e);
				}
			}
		}
	}
}