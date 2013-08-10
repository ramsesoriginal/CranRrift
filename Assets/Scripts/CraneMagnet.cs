using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ConfigurableJoint))]
public class CraneMagnet : MonoBehaviour {
	
	public bool inputDropStuff;
	public bool enableTestInput;
	
	float disabledTimer; // if > 0, can't pick up stuff
	
	Dictionary<Rigidbody, FixedJoint> stuff;
	
	void Awake() {
		stuff = new Dictionary<Rigidbody, FixedJoint>();
	}
	
	void Update() {
		if (enableTestInput) {
			inputDropStuff = Input.GetKey(KeyCode.D);
		}
	}
	
	void FixedUpdate() {
		if (disabledTimer >= 0.0f) disabledTimer -= Time.deltaTime;
		if (inputDropStuff) DropStuff();
	}
	
	void OnCollisionEnter(Collision c) {
		if (disabledTimer > 0.0f) return;
		
		// Can't pick up static stuff
		if (!c.rigidbody) return;
		
		// Don't pick up the chain itself, duh.
		if (c.gameObject.GetComponent<ChainLink>()) return;
		
		PickUp(c.rigidbody);
	}
	
	void PickUp(Rigidbody rb) {
		if (stuff.ContainsKey(rb)) return; // Already picked up
		
		var j = gameObject.AddComponent<FixedJoint>();
		j.connectedBody = rb;
		stuff.Add(rb, j);
	}
	
	public void DropStuff() {
		foreach (var kv in stuff) {
			kv.Value.connectedBody = null;
			Destroy(kv.Value);
		}
		stuff.Clear();
		disabledTimer = 1.0f;
	}
	
}
