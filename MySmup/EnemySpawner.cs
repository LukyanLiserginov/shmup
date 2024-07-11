using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    [ObjectFactory(Category = "Component/Game")]
    [Preserve(AllMembers = true)]

    internal class EnemySpawner : LogicComponent
    {
        private int _spawnTimer = 190;
        private Random _random = new Random();
        public EnemySpawner(Context context) : base(context)
        {
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            if (_spawnTimer > 0) _spawnTimer--;
            else
            {
                if (_random.Next(10) < 3)
                {
                    var ship = Scene.CreateChild();
                    ship.Name = "RedShip2";
                    ship.Position = new Vector3(_random.Next(-6, 6), 0, 8);
                    ship.Direction = Node.Direction;
                    ship.CreateComponent<PrefabReference>()
                        .SetPrefab(Context.ResourceCache.GetResource<PrefabResource>("Prefabs/RedShip2.prefab"));

                }
                else 
                {
                    var ship = Scene.CreateChild();
                    ship.Name = "RedShip1";
                    ship.Position = new Vector3(_random.Next(-6, 6), 0, 8);
                    ship.Direction = Node.Direction;
                    ship.CreateComponent<PrefabReference>()
                        .SetPrefab(Context.ResourceCache.GetResource<PrefabResource>("Prefabs/RedShip1.prefab"));
                }
                _spawnTimer = 190;
            }


        }
    }
}
