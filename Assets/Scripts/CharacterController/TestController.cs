using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public GoDummyPath go;
	void Start () {
		//var path = new GoSpline(Application.dataPath + "/Paths/CirclePath");
		//var config = new TweenConfig().positionPath(path, true).setIterations(-1);
		var points = new Vector3[] { new Vector3(-4, 0, 1.33f), new Vector3(-3.66f, 0, 5.7f), new Vector3(-5.88f, 0, 9.64f), new Vector3(-3.1f, 0, 12.32f) };
		var path = new GoSpline(points);
		path.closePath();
		//Go.to(transform, 3f, new GoTweenConfig().positionPath(path, true));
		var tween = new GoTween(transform, 4f, new GoTweenConfig().positionPath(path, true));
		Go.addTween(tween);
		tween.play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
