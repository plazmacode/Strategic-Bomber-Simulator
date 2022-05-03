using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public abstract class Factory
    {
        public abstract GameObject Create(object generic);
    }
}
