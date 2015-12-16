using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

#if UNITY_ENGINE
using UnityEditor;
#endif

/*
 * Class for triggering camera events
 * Attach this script to invisible trigger object that player will collide with
 * Camera Position and Focus Vector3 handles appear 
 */
public class PoPCameraEvent : MonoBehaviour {

    [System.Serializable]
    public class EventFocus
    {
        public Vector3 focus = Vector3.zero;
        public float time = 0f;

        public EventFocus(Vector3 foc, float t) {
            focus = foc;
            time = t;
        }
    }

	public List<Vector3> eventPath = new List<Vector3>();
    [SerializeField] public List<EventFocus> focusPoints = new List<EventFocus>();
    GoSpline spline;
	public float pathLength = 3f;
	public bool multipleTargets = false;
	public Vector3 singleTargetPos = Vector3.zero;
	private bool firstRun = true;
	private float focusTimer = 0f;
	private int focusIndex = 0;

	void Start()
    {
		if(eventPath.Count == 0) {
			Debug.Warning("camera", "Warning: Camera Event " + this.gameObject.name + " has no path");
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.name == "_Player") {
			PoPCamera.instance.eventTrigger = this;
			PoPCamera.State = Camera_2.CameraState.CamEvent;
		}
	}

    public void Initialize()
    {
		eventPath.Add(gameObject.transform.position + (Vector3.forward * 2));
		eventPath.Add(gameObject.transform.position + (Vector3.forward * 4));
		eventPath.Add(gameObject.transform.position + (Vector3.forward * 6));
		eventPath.Add(gameObject.transform.position + (Vector3.forward * 8));

        focusPoints.Add(new EventFocus(gameObject.transform.position + (Vector3.forward * 4) + (Vector3.right * 4), 3f));
    }

    public void AddNode()
    {
		eventPath.Add(eventPath[eventPath.Count - 1]);
    }

    public void DeleteNode(int index)
    {
        eventPath.RemoveAt(index);
    }

	public void DumpPath() {
		for (int i = eventPath.Count - 1; i >= 0; i--) {
			eventPath.RemoveAt (i);
		}
        for (int i = focusPoints.Count - 1; i >= 0; i--) {
            focusPoints.RemoveAt(i);
        }
    }

    public void AddFocus()
    {
        if(focusPoints.Count == 0)
            focusPoints.Add(new EventFocus(gameObject.transform.position + (Vector3.forward * 4) + (Vector3.right * 4), 3f));
        else
            focusPoints.Add(new EventFocus(focusPoints[focusPoints.Count - 1].focus, 3f));
    }

	public void DeleteFocus(int index) 
	{
		focusPoints.RemoveAt(index);
	}

	public void Event()
	{
		if(firstRun) {
			firstRun = false;
            spline = new GoSpline(eventPath);
            var pathProperty = new PositionPathTweenProperty(spline);

            var config = new GoTweenConfig();
            config.addTweenProperty(pathProperty);

            var tween = new GoTween(PoPCamera.instance.transform, pathLength, config);
            Go.addTween(tween);
			if(multipleTargets) {
				focusTimer = focusPoints[0].time;
			}
		}

		if(multipleTargets) {
			if(focusTimer >= focusPoints[focusIndex].time && focusIndex != focusPoints.Count - 1) {
				focusIndex++;
			}
			Quaternion rotation = Quaternion.LookRotation(focusPoints[focusIndex].focus - PoPCamera.instance.transform.position);
			PoPCamera.instance.transform.rotation = Quaternion.Slerp(PoPCamera.instance.transform.rotation, rotation, 0.4f);
		} else {
			Quaternion rotation = Quaternion.LookRotation(singleTargetPos - PoPCamera.instance.transform.position);
			PoPCamera.instance.transform.rotation = Quaternion.Slerp(PoPCamera.instance.transform.rotation, rotation, 0.4f);
		}

		if(pathLength < 0) {
			PoPCamera.State = Camera_2.CameraState.Normal;
			Destroy(this.gameObject);
		} else
			pathLength -= Time.deltaTime;
	}

    void OnDrawGizmos()
    {
		if(eventPath.Count > 0) {
			var spline = new GoSpline(eventPath);
			Gizmos.color = Color.magenta;
			spline.drawGizmos(50f);
		}
    }
}
