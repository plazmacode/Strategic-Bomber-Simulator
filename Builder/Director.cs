using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    /// <summary>
    /// Use Construct() to return a list of builds
    /// </summary>
    class Director
    {
        private List<IBuilder> builders = new List<IBuilder>();

        internal List<IBuilder> Builders { get => builders; set => builders = value; }

        public List<GameObject> Construct()
        {
            List<GameObject> builds = new List<GameObject>();
            for (int i = 0; i < Builders.Count; i++)
            {
                builders[i].BuildGameObject();
                builds.Add(builders[i].GetResult());
            }

            return builds;
        }
    }
}
