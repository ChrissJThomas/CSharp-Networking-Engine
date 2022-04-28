using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetCORE {
	public class Bad_Guy_Move : Net_Component {

		private Rigidbody myRig;
		private float x;

		// Use this for initialization
		void Start () {
			myRig = gameObject.GetComponent<Rigidbody> ();
			StartCoroutine (slowUpdate ());
			x = 4;
		}

		public IEnumerator slowUpdate () {
			while (true) {
				if (myCore.isServer) {
					myRig.velocity = new Vector3 (x, 0, 0);
				}
				yield return new WaitForSeconds (0.1f);
			}
		}

		public override void handle_Message (string var, string value) { }

		void OnCollisionEnter (Collision collider) {
			Debug.Log ("bad_guy hit");
			if (myCore.isServer) {
				x *= -1;
				myRig.velocity = new Vector3 (x, 0, 0);
			}
		}
	}
}