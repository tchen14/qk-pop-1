using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	GoTween tween;
	GoTweenChain chain;
	void Start () {
		//var path = new GoSpline(Application.dataPath + "/Paths/CirclePath");
		//var config = new TweenConfig().positionPath(path, true).setIterations(-1);
		var points = new Vector3[] { new Vector3(-4, 0, 1.33f), new Vector3(-3.66f, 0, 5.7f), new Vector3(-5.88f, 0, 9.64f), new Vector3(-3.1f, 0, 12.32f) };
		var path = new GoSpline(points, true);
		//path.splineType = GoSplineStraightLineSolver
		//var path2 = new Go //GoSpline(points);
		//path.closePath();
		//Go.to(transform, 3f, new GoTweenConfig().positionPath(path, true));
		tween = new GoTween(transform, 4f, new GoTweenConfig().positionPath(path, true));
		chain = new GoTweenChain();
		chain.append(tween);
		
		
		//Go.addTween(tween);
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("a")){
			chain.restart();
		}
		if(Input.GetKeyDown("z")){
			chain.play();
		}
		if(Input.GetKeyDown("x")){
			chain.reverse();
		}
		
		if(Input.GetKeyDown("c")){
			chain.pause();
		}
	}
}
