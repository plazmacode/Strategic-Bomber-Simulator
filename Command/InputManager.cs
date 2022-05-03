using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    class InputManager
    {
        private static InputManager instance;
        private KeyboardState keyState = Keyboard.GetState();
        private KeyboardState oldkeyState = Keyboard.GetState();

        private MouseState mouseState = Mouse.GetState();
        private  MouseState oldMouseState;

        private CameraZoomCommand zoomIn;
        private CameraZoomCommand zoomOut;


        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        private Dictionary<KeyInfo, ICommand> keysDown = new Dictionary<KeyInfo, ICommand>();
        private Dictionary<KeyInfo, ICommand> keysClicked = new Dictionary<KeyInfo, ICommand>();

        private ButtonEvent buttonEvent = new ButtonEvent();

        private InputManager()
        {

            //buttonEvent.Attach(player);

            keysDown.Add(new KeyInfo(Keys.Left), new MoveCameraCommand(new Vector2(-1, 0)));
            keysDown.Add(new KeyInfo(Keys.Right), new MoveCameraCommand(new Vector2(1, 0)));
            keysDown.Add(new KeyInfo(Keys.Up), new MoveCameraCommand(new Vector2(0, -1)));
            keysDown.Add(new KeyInfo(Keys.Down), new MoveCameraCommand(new Vector2(0, 1)));
            keysDown.Add(new KeyInfo(Keys.A), new TurnPlaneCommand("left"));
            keysDown.Add(new KeyInfo(Keys.D), new TurnPlaneCommand("right"));
            keysDown.Add(new KeyInfo(Keys.Space), new DropBombCommand());
            keysClicked.Add(new KeyInfo(Keys.G), new OptionCommand<bool>("GridEnabled"));
            keysClicked.Add(new KeyInfo(Keys.H), new OptionCommand<bool>("GenerateNewWorld"));
            keysClicked.Add(new KeyInfo(Keys.P), new OptionCommand<bool>("GamePaused"));
            //keysClicked.Add(new KeyInfo(Keys.Y), new OptionCommand<float>("Zoom", +0.2f)); //Moved to CameraZoomCommand
            //keysClicked.Add(new KeyInfo(Keys.H), new OptionCommand<float>("Zoom", -0.2f));

            keysDown.Add(new KeyInfo(Keys.Subtract), new CameraZoomCommand(0.95f));
            keysDown.Add(new KeyInfo(Keys.Add), new CameraZoomCommand(1.05f));
            keysDown.Add(new KeyInfo(Keys.OemMinus), new CameraZoomCommand(0.95f));
            keysDown.Add(new KeyInfo(Keys.OemPlus), new CameraZoomCommand(1.05f));

            zoomIn = new CameraZoomCommand(1.1f);
            zoomOut = new CameraZoomCommand(0.9f);
        }

        public void Update()
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //Mouse Zoom if mousewheel is used later make some mouse position checks perhaps a Rect Dictionary for each mouse action area
            //yuck
            if (mouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
            {
                zoomIn.Execute();
            }
            else if (mouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
            {
                zoomOut.Execute();
            }

            //Check if key is down
            foreach (KeyInfo keyInfo in keysDown.Keys)
            {
                if (keyState.IsKeyDown(keyInfo.Key))
                {
                    keysDown[keyInfo].Execute();
                    buttonEvent.Notify(keyInfo.Key, BUTTONSTATE.DOWN);
                    keyInfo.IsDown = true;

                }
                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown == true)
                {
                    buttonEvent.Notify(keyInfo.Key, BUTTONSTATE.UP);
                    keyInfo.IsDown = false;
                }
            }
            //Check if key is clicked
            foreach (KeyInfo keyInfo in keysClicked.Keys)
            {
                if (keyState.IsKeyUp(keyInfo.Key) && oldkeyState.IsKeyDown(keyInfo.Key))
                {
                    keysClicked[keyInfo].Execute();
                    buttonEvent.Notify(keyInfo.Key, BUTTONSTATE.DOWN);
                    keyInfo.IsDown = true;

                }
                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown == true)
                {
                    buttonEvent.Notify(keyInfo.Key, BUTTONSTATE.UP);
                }
            }
            oldkeyState = keyState;
            oldMouseState = mouseState;

            Options.Instance.Update();
        }
    }

    public class KeyInfo
    {
        public bool IsDown { get; set; }

        public Keys Key { get; set; }

        public KeyInfo(Keys key)
        {
            this.Key = key;
        }
    }
}


