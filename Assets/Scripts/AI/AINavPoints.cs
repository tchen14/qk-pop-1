using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AINavPoints : MonoBehaviour {

    /*
    STEPS TO CREATE A PATH AND ASSIGN TO THE AI! 
1. Create empty game object and place in the AICheckPoint manager
2. attach the AINavPoints script
3. For the number of points on the path, change AICheckpoints size to match
4. indicate the type of path by an int. 0 = to a point, 1 = loop, 2 = back and forth
5. if the path is going to be looped specify the number of times in NofLoops
6. That is all to create a path
7. More than one path can be added to an AI if you want to make it go to point then loop different points then move somewhere else after x number of loops. etc.
8 create any other scripts you may want in the same manner
9. Look at the AI you want to attach the path(s) to
10. navigate to the pathways array and change the size to the number of paths you have.
11. Drag the paths into the array IN ORDER!
12. You are all done now. AI Should be working properly contact Alex Glaeser if that doesn't work.
*/

    public List<Vector3> AiCheckpoints;
    public int PathType;
    public int NofLoops;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
}
