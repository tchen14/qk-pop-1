using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	private bool isPaused = false;
	public GameHUD GHud;
    public float speed = 2.0f;

	void Update () {                                // Pause must be on Update() if put on FixedUpdate()                                            
        //Debug.Log(Time.timeScale);                  // The game will get stuck and will have to reset


		if(Input.GetKeyDown(KeyCode.Escape)) {      // This will need to be changed to call inputManager  
			
			if(!isPaused) {                         
				pauseGame();
			} else if(isPaused) {
				unPauseGame();	
			}
		}
        if(Input.GetKeyDown(KeyCode.T)) {           // Feature where it allows to slow down time or speed it up
                                                    
            GHud.timeManipulate(speed);
        }

	}


	public void pauseGame() {                       // Pauses the game and brings up the Pause Menu
		isPaused = true;
		Time.timeScale = 0f;
		GHud.showPauseMenu();
	}

	public void unPauseGame() {                     // Unpauses the game and hides menu
		isPaused = false;
		Time.timeScale = 1f;
		GHud.hidePauseMenu();
	}
	public void unPauseGameBtt() {                  // When button is used to unpause this function is called
		isPaused = false;
		Time.timeScale = 1f;
		
	}

    public void setPause() {                        // It is called if game needs to be paused w/o Menu
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void setTimeNormal() {                   // It is called to set the time back to normal .. to unpause the game
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void manipulateTime(float speed) {       // Manipulate time if needed by getting a float and setting it
        Time.timeScale = speed;                     // to Time.timeScale ... Slows down Time or Speeds up Time
    }

    }
