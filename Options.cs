using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public class Options
    {
        private static Options instance;
        public static bool GenerateNewWorld { get; set; } = false;
        public bool GridEnabled { get; set; } = false;

        public bool GamePaused { get; set; } = true;

        /// <summary>
        /// The Zoom property does not work probrably, Using ZoomCommand instead
        /// </summary>
        public float Zoom { get; set; } = 1.0f;

        private Options()
        {

        }

        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Options();
                }
                return instance;
            }
        }


        public void Update()
        {
            if (GenerateNewWorld && !World.Instance.WorldRemoved)
            {
                //Remove world in first Update() call
                World.Instance.RemoveWorld();
                World.Instance.WorldNumber++;
            }
            else if (GenerateNewWorld && World.Instance.WorldRemoved )
            {
                //Create World in second Update() call
                GenerateNewWorld = false;
                World.Instance.CreateWorld();
            }
            if (GamePaused)
            {
                World.CurrentGameState = GameState.PAUSE;
            } else
            {
                World.CurrentGameState = GameState.PLAY;
            }
        }

        public void ChangeBool(string propName)
        {
            PropertyInfo pinfo = typeof(Options).GetProperty(propName);
            object value = pinfo.GetValue(Options.Instance, null);

            if ((bool)value == true)
            {
                value = false;
            } else
            {
                value = true;
            }
            pinfo.SetValue(Options.Instance, value);
        }

        public void ChangeValue<T>(string propName, float amount)
        {
            PropertyInfo pinfo = typeof(Options).GetProperty(propName);
            object value = pinfo.GetValue(Options.Instance, null);

            //Change bool
            if (typeof(T) == typeof(bool))
            {
                if ((bool)value == true)
                {
                    value = false;
                }
                else
                {
                    value = true;
                }
            }

            if (typeof(T) == typeof(int))
            {
                int a = Convert.ToInt32(value);
                a += (int)amount;
                value = a;
            }

            if (typeof(T) == typeof(float))
            {
                float a = Convert.ToInt32(value);
                a += amount;
                value = a;
            }
            pinfo.SetValue(Options.Instance, Convert.ChangeType(value, pinfo.PropertyType), null);
        }
    }
}
