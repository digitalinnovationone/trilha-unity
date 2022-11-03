using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    public readonly string name;

    protected State(string name) {
        this.name = name;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
    public virtual void LateUpdate() {}
    public virtual void FixedUpdate() {}

}
