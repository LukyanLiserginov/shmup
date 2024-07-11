using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]
    internal class Bullet : LogicComponent
    {
        private int _timer = 300;
        public float Speed = 0.05f;

        public Bullet(Context context) : base(context)
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(float timestep)
        {
            base.Update(timestep);
            Node.Position += Node.Direction*Speed;
            if (_timer < 1) Node.Remove();
            else _timer--;
        }
    }
}
