using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    interface IBuilder
    {
        void BuildGameObject();

        GameObject GetResult();
    }
}
