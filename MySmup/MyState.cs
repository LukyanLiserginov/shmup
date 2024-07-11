using MySmup;
using System;
using System.Collections.Generic;
using System.Text;
using Urho3DNet;

namespace MySmup
{
    internal class MyState : ApplicationState
    {
        protected readonly Scene _scene;
        private readonly UrhoPluginApplication _app;
        private readonly Node _cameraNode;
        private readonly Camera _camera;
        private readonly Viewport _viewport;
        private Random _random = new Random();

        public MyState(UrhoPluginApplication app) : base (app.Context)
        {
            MouseMode = MouseMode.MmRelative;
            IsMouseVisible = false;

            _app = app;
            _scene = Context.CreateObject<Scene>();
            _scene.Load("Scenes/Scene.scene");
            _cameraNode = _scene.FindChild("CameraNode", true);
            _camera = _cameraNode.GetComponent<Camera>();
            _viewport = Context.CreateObject<Viewport>();
            _viewport.Scene = _scene;
            _viewport.Camera = _camera;
            SetViewport(0, _viewport);
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);
            if (!MyTools.IsRunning)
            {
                MyTools.IsRunning = true;
                _app.PlayerDeath();
            }
            SpawnStar();
        }

        private void SpawnStar()
        {
            var star = _scene.CreateChild();
            star.Position = new Vector3(_random.Next(-90, 90)*0.1f, -2, 14);
            star.Direction = new Vector3(0, 0, -1);
            var starScript = star.CreateComponent<Bullet>();
            starScript.Speed = _random.Next(10) * 0.01f;
            var model = star.CreateComponent<StaticModel>();
            model.SetModel(Context.ResourceCache.GetResource<Model>("Models/Box.mdl"));
            star.SetScale(_random.Next(10) * 0.005f);
        }

        public override void Activate(StringVariantMap bundle)
        {
            SubscribeToEvent(E.KeyUp, HandleKeyUp);

            _scene.IsUpdateEnabled = true;

            _app.Settings.Apply(_scene.GetComponent<RenderPipeline>());

            base.Activate(bundle);
        }

        public override void Deactivate()
        {
            _scene.IsUpdateEnabled = false;
            UnsubscribeFromEvent(E.KeyUp);
            base.Deactivate();
        }

        protected override void Dispose(bool disposing)
        {
            _scene?.Dispose();

            base.Dispose(disposing);
        }

        private void HandleKeyUp(VariantMap args)
        {
            var key = (Key)args[E.KeyUp.Key].Int;
            switch (key)
            {
                case Key.KeyEscape:
                case Key.KeyBackspace:
                    _app.HandleBackKey();
                    return;
            }
        }
    }
}
