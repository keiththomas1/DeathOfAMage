using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacialBehavior : MonoBehaviour {
    private Vector3 _movementVector = new Vector3(0, 0, 0);
    private float _speed = 3.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(this._movementVector * this._speed / 100.0f);	
	}

    public void SetMovementVector(Vector3 moveVector)
    {
        this._movementVector = moveVector;
    }
}
