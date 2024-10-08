using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;

namespace SimpleDXApp
{
	public class InputHandler
	{
		private DirectInput directInput;
		private Keyboard keyboard;
		private KeyboardState keyboardState;
		public bool Left { get; private set; }
		public bool Right { get; private set; }
		public bool Up { get; private set; }
		public bool Down { get; private set; }

		public InputHandler()
		{
			directInput = new DirectInput();
			keyboard = new Keyboard(directInput);
			keyboard.Acquire();
		}

		public void Update()
		{
			keyboardState = keyboard.GetCurrentState();

			if (keyboardState.IsPressed(Key.W))
				Up = true;
			else
				Up = false;
			if (keyboardState.IsPressed(Key.A))
				Left = true;
			else
				Left = false;
			if (keyboardState.IsPressed(Key.D))
				Right = true;
			else
				Right = false;
			if (keyboardState.IsPressed(Key.S))
				Down = true;
			else
				Down = false;
		}
	}
}

