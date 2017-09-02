using UnityEngine;
using System.Collections;

public class BaseAction : MonoBehaviour {
    public AbilityDescriptor ability;
    public bool _actionStarted;
    public bool actionStarted {
        get {
            return _actionStarted;
        }
        set {
            // Set only once to started. When started cannot be stopped.
            if (!_actionStarted) {
                _actionStarted = value;
                if (_actionStarted) {
                    actor.OnStartAction();
                }
            }
        }
    }

    public PitchCharacter actor { get; set; }

    //// //// //// //// //// //// //// //// //// 
    ///  Action specific stuff
    public virtual void Init (AbilityDescriptor ability) {
        _actionStarted = false;
        this.ability = ability;
        this.actor = gameObject.GetComponent<PitchCharacter> ();
    }

    public T BindAbility<T> (AbilityDescriptor ability) where T : AbilityDescriptor {
        T a = ability as T;
        if (!a) {
            throw new UnityException("Cannot bind ability to the correct derived type.");
        }
        return a;
    }

    public virtual void Reset () {
    }

    public virtual bool OnClickDown(Vector3 mouseWorldPos) {
        return true;
    }    
    public virtual void OnClickDrag(Vector3 mouseWorldPos) {
    }    
    public virtual void OnClickUp(Vector3 mouseWorldPos){
    }
    public virtual void OnWorldStabilize() {
    }
    public virtual void EndAction (bool cancelled = false) {
        actor.OnEndAction (cancelled);
    }

    public virtual void OnGUI () {
        /*
        var actionInfoRect = GameManager.Instance.actionInfoRect;
        GUILayout.BeginArea (actionInfoRect);

        GUILayout.Label (ability.Name);

        GUILayout.EndArea ();
        */

        // GUI.Label (new Rect(), msg);
        // Debug stuff
        // GUI.Label (new Rect(300, 300, 300, 40), name);
    }


    //////////////////////////////////////////////
    public void CancelAction () {
        EndAction (true);
    }

}