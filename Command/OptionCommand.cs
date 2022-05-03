using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    /// <summary>
    /// An option command allows the game to changea value of a specific type using keyInput.
    /// If the type is a bool it will simply flip between true and false.
    /// The flip is done b the Options class.
    /// If the value was instead an int the option class would have to use some range passed to it from OptionCommand
    /// Alternatively limit the get and set of a variable in the Options class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    class OptionCommand<T> : ICommand
    {
        private string property;
        private float amount;


        public OptionCommand(string property)
        {
            this.property = property;
        }

        //Increase or decrease an option
        public OptionCommand(string property, float amount)
        {
            this.property = property;
            this.amount = amount;
        }

        public void Execute()
        {
            Options.Instance.ChangeValue<T>(property, amount);
        }
    }
}
