using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strategic_Bomber_Simulator
{
    public class GameObject : ICloneable
    {
        private List<Component> components = new List<Component>();

        public Transform Transform { get; set; } = new Transform();

        public string Tag { get; set; }

        public Component AddComponent(Component component)
        {
            component.GameObject = this;

            components.Add(component);

            return component;
        }

        public Component GetComponent<T>() where T : Component
        {
            return components.Find(x => x.GetType() == typeof(T));
        }

        public bool HasComponent<T>() where T : Component
        {
            Component c = components.Find(x => x.GetType() == typeof(T));

            return c != null;
        }

        public void Awake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Awake();
            }
        }

        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        public void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(spriteBatch);
            }
        }

        public object Clone()
        {
            GameObject go = new GameObject();

            foreach (Component component in components)
            {
                go.AddComponent(component.Clone() as Component);
            }

            return go;
        }

        public void SetPosition(float x, float y)
        {
            Transform.Position = new Vector2(x, y);
        }

        public void SetPosition(int x, int y)
        {
            Transform.Position = new Vector2(x, y);
        }

        public void TranslatePosition(Vector2 vector)
        {
            Transform.Translate(vector);
        }
    }
}
