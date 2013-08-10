using UnityEngine;
using System.Collections;

public class ChainController : MonoBehaviour {
	
	public bool inputUp, inputDown;
	public bool enableTestInput;
	public ChainLink chainLinkPrefab;
	public float chainLinkSize;
	public float chainSpeed;
	
	ChainLink topChainLink;
	float currentPos; // [0, chainLinkSize], controllable through inputs
	
	void Reset() {
		chainLinkSize = 2.0f; // standard capsule height
		chainSpeed = 5.0f;
	}
	
	void Update() {
		if (enableTestInput) {
			inputUp = Input.GetKey(KeyCode.Q);
			inputDown = Input.GetKey(KeyCode.A);
		}
	}
	
	void FixedUpdate() {
		
		if (topChainLink) {
			var j = topChainLink.GetComponent<ConfigurableJoint>();
			topChainLink.transform.position = transform.position - transform.up * currentPos;
			topChainLink.transform.rotation = transform.rotation;
			j.anchor = j.anchor;
		}
		
		if (inputDown) {
			currentPos += chainSpeed * Time.deltaTime;
			if (currentPos > chainLinkSize) {
				AddChainLink();
				currentPos -= chainLinkSize;
			}
		}
		
		if (inputUp && topChainLink) {
			currentPos -= chainSpeed * Time.deltaTime;
			if (currentPos < 0.0f) {
				RemoveChainLink();
				currentPos += chainLinkSize;
			}
		}
		
	}
	
	void AddChainLink() {
		var pos = transform.position;
		var rot = transform.rotation;
		if (topChainLink) {
			pos = topChainLink.transform.position + topChainLink.transform.up * chainLinkSize;
			rot = topChainLink.transform.rotation;
		}
		var cl = (ChainLink) Instantiate(chainLinkPrefab, pos, rot);
		
		cl.next = topChainLink;
		if (topChainLink) {
			var j = topChainLink.GetComponent<ConfigurableJoint>();
			j.connectedBody = cl.rigidbody;
		}
		
		topChainLink = cl;
	}
	
	void RemoveChainLink() {
		if (!topChainLink) return;
		
		if (topChainLink.next) {
			var j = topChainLink.next.GetComponent<ConfigurableJoint>();
			j.connectedBody = null;
		}
		
		Destroy(topChainLink.gameObject);
		topChainLink = topChainLink.next;
	}
	
}
