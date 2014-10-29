using UnityEngine;
using System.Collections.Generic;

public class KeyboardInputManager : InputManager {
	private string typedString;
	private char[] array;
	private float timeHeld = 0f;
	bool delete = true;
	
	
	// Use this for initialization
	void Start () 
	{
		typedString = string.Empty;
	}
	
	// Update is called once per frame
	void Update ()
	{
		print (typedString);
		if(Input.GetKeyDown("`"))
		{
			if(shiftPressed())
				typedString = typedString + '~';
			else
				typedString = typedString + '`';
		}
		if(Input.GetKeyDown("q"))
		{
			if(shiftPressed())
				typedString = typedString + 'Q';
			else
				typedString = typedString + 'q';
		}
		if(Input.GetKeyDown("w"))
		{
			if(shiftPressed())
				typedString = typedString + 'W';
			else
				typedString = typedString + 'w';
		}
		if(Input.GetKeyDown("e"))
		{
			if(shiftPressed())
				typedString = typedString + 'E';
			else
				typedString = typedString + 'e';
		}
		if(Input.GetKeyDown("r"))
		{
			if(shiftPressed())
				typedString = typedString + 'R';
			else
				typedString = typedString + 'r';
		}
		if(Input.GetKeyDown("t"))
		{
			if(shiftPressed())
				typedString = typedString + 'T';
			else
				typedString = typedString + 't';
		}
		if(Input.GetKeyDown("y"))
		{
			if(shiftPressed())
				typedString = typedString + 'Y';
			else
				typedString = typedString + 'y';
		}
		if(Input.GetKeyDown("u"))
		{
			if(shiftPressed())
				typedString = typedString + 'U';
			else
				typedString = typedString + 'u';
		}
		if(Input.GetKeyDown("i"))
		{
			if(shiftPressed())
				typedString = typedString + 'I';
			else
				typedString = typedString + 'i';
		}
		if(Input.GetKeyDown("o"))
		{
			if(shiftPressed())
				typedString = typedString + 'O';
			else
				typedString = typedString + 'o';
		}
		if(Input.GetKeyDown("p"))
		{
			if(shiftPressed())
				typedString = typedString + 'P';
			else
				typedString = typedString + 'p';
		}
		if(Input.GetKeyDown("a"))
		{
			if(shiftPressed())
				typedString = typedString + 'A';
			else
				typedString = typedString + 'a';
		}
		if(Input.GetKeyDown("s"))
		{
			if(shiftPressed())
				typedString = typedString + 'S';
			else
				typedString = typedString + 's';
		}
		if(Input.GetKeyDown("d"))
		{
			if(shiftPressed())
				typedString = typedString + 'D';
			else
				typedString = typedString + 'd';
		}
		if(Input.GetKeyDown("f"))
		{
			if(shiftPressed())
				typedString = typedString + 'F';
			else
				typedString = typedString + 'f';
		}
		if(Input.GetKeyDown("g"))
		{
			if(shiftPressed())
				typedString = typedString + 'G';
			else
				typedString = typedString + 'g';
		}
		if(Input.GetKeyDown("h"))
		{
			if(shiftPressed())
				typedString = typedString + 'H';
			else
				typedString = typedString + 'h';
		}
		if(Input.GetKeyDown("j"))
		{
			if(shiftPressed())
				typedString = typedString + 'J';
			else
				typedString = typedString + 'j';
		}
		if(Input.GetKeyDown("k"))
		{
			if(shiftPressed())
				typedString = typedString + 'K';
			else
				typedString = typedString + 'k';
		}
		if(Input.GetKeyDown("l"))
		{
			if(shiftPressed())
				typedString = typedString + 'L';
			else
				typedString = typedString + 'l';
		}
		if(Input.GetKeyDown("z"))
		{
			if(shiftPressed())
				typedString = typedString + 'Z';
			else
				typedString = typedString + 'z';
		}
		if(Input.GetKeyDown("x"))
		{
			if(shiftPressed())
				typedString = typedString + 'X';
			else
				typedString = typedString + 'x';
		}
		if(Input.GetKeyDown("c"))
		{
			if(shiftPressed())
				typedString = typedString + 'C';
			else
				typedString = typedString + 'c';
		}
		if(Input.GetKeyDown("v"))
		{
			if(shiftPressed())
				typedString = typedString + 'V';
			else
				typedString = typedString + 'v';
		}
		if(Input.GetKeyDown("b"))
		{
			if(shiftPressed())
				typedString = typedString + 'B';
			else
				typedString = typedString + 'b';
		}
		if(Input.GetKeyDown("n"))
		{
			if(shiftPressed())
				typedString = typedString + 'N';
			else
				typedString = typedString + 'n';
		}
		if(Input.GetKeyDown("m"))
		{
			if(shiftPressed())
				typedString = typedString + 'M';
			else
				typedString = typedString + 'm';
		}
		if(Input.GetKeyDown("1"))
		{
			if(shiftPressed())
				typedString = typedString + '!';
			else
				typedString = typedString + '1';
		}
		if(Input.GetKeyDown("2"))
		{
			if(shiftPressed())
				typedString = typedString + '@';
			else
				typedString = typedString + '2';
		}
		if(Input.GetKeyDown("3"))
		{
			if(shiftPressed())
				typedString = typedString + '#';
			else
				typedString = typedString + '3';
		}
		if(Input.GetKeyDown("4"))
		{
			if(shiftPressed())
				typedString = typedString + '$';
			else
				typedString = typedString + '4';
		}
		if(Input.GetKeyDown("5"))
		{
			if(shiftPressed())
				typedString = typedString + '%';
			else
				typedString = typedString + '5';
		}
		if(Input.GetKeyDown("6"))
		{
			if(shiftPressed())
				typedString = typedString + '^';
			else
				typedString = typedString + '6';
		}
		if(Input.GetKeyDown("7"))
		{
			if(shiftPressed())
				typedString = typedString + '&';
			else
				typedString = typedString + '7';
		}
		if(Input.GetKeyDown("8"))
		{
			if(shiftPressed())
				typedString = typedString + '*';
			else
				typedString = typedString + '8';
		}
		if(Input.GetKeyDown("9"))
		{
			if(shiftPressed())
				typedString = typedString + '(';
			else
				typedString = typedString + '9';
		}
		if(Input.GetKeyDown("0"))
		{
			if(shiftPressed())
				typedString = typedString + ')';
			else
				typedString = typedString + '0';
		}
		if(Input.GetKeyDown("["))
		{
			if(shiftPressed())
				typedString = typedString + '{';
			else
				typedString = typedString + '[';
		}
		if(Input.GetKeyDown("]"))
		{
			if(shiftPressed())
				typedString = typedString + '}';
			else
				typedString = typedString + ']';
		}
		if(Input.GetKeyDown("\\"))
		{
			if(shiftPressed())
				typedString = typedString + '|';
			else
				typedString = typedString + '\\';
		}
		if(Input.GetKeyDown(";"))
		{
			if(shiftPressed())
				typedString = typedString + ':';
			else
				typedString = typedString + ';';
		}
		if(Input.GetKeyDown("'"))
		{
			if(shiftPressed())
				typedString = typedString + '"';
			else
				typedString = typedString + '\'';
		}
		if(Input.GetKeyDown(","))
		{
			if(shiftPressed())
				typedString = typedString + '<';
			else
				typedString = typedString + ',';
		}
		if(Input.GetKeyDown("."))
		{
			if(shiftPressed())
				typedString = typedString + '>';
			else
				typedString = typedString + '.';
		}
		if(Input.GetKeyDown("/"))
		{
			if(shiftPressed())
				typedString = typedString + '?';
			else
				typedString = typedString + '/';
		}
		if(Input.GetKeyDown("space"))
		{
			typedString = typedString + ' ';
		}
		if(Input.GetKey("backspace"))
		{
			timeHeld = Time.deltaTime*5 + timeHeld;
		}
		else
		{
			if(typedString.Length > 0)
			{
				if(timeHeld > 0f && delete)
				{
					if(typedString.Length-(int)Mathf.Ceil(timeHeld) < 0)
					{
						array = typedString.ToCharArray(0,0);
						typedString = new string(array);
					}
					else
					{
						array = typedString.ToCharArray(0,typedString.Length-(int)Mathf.Ceil(timeHeld));
						typedString = new string(array);
						delete = false;
					}
				}
			}
			timeHeld = 0;
			delete = true;
		}
		if(Input.GetKeyDown("return"))
		{
			//TO DO; INPUT IS FINISHED
		}
	}
	
	private bool shiftPressed()
	{
		if(Input.GetKey("left shift") || Input.GetKey("right shift"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}