using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]
    internal class BaseShip : LogicComponent
    {
        public float Speed {  get; set; }
        public int Structure { get; set; }
        public BaseShip(Context context) : base(context)
        {
        }

        public override void DelayedStart()
        {
            base.DelayedStart();
            var trigger = Node.CreateComponent<BaseTrigger>();
            trigger.Ship = this;
        }


        public virtual void Fire()
        {
            Fire(Node.Direction);
        }

        public virtual void Fire (Vector3 direction)
        {
            var bullet = Scene.CreateChild();
            bullet.Position = Node.Position;
            bullet.Direction = direction;
            var bulletScript = bullet.CreateComponent<Bullet>();
            var model = bullet.CreateComponent<StaticModel>();
            var collider = bullet.CreateComponent<CollisionShape>();
            collider.SetSphere(1);
            var rb = bullet.CreateComponent<RigidBody>();
            rb.IsTrigger = true;
            rb.UseGravity = false;
            rb.Mass = 1;
            model.SetModel(Context.ResourceCache.GetResource<Model>("Models/Sphere.mdl"));

            if (Node.Name.Contains("Red"))
            {
                rb.SetCollisionLayerAndMask(2, 1023);
                model.SetMaterial(Context.ResourceCache.GetResource<Material>("Materials/DefaultRed.xml"));
            }
            else model.SetMaterial(Context.ResourceCache.GetResource<Material>("Materials/DefaultBlue.xml"));
            bullet.SetScale(0.1f);

        }

        public virtual void Destroy()
        {
            Node.Remove();
        }

    }
}
