using MySmup.Utils;
using Urho3DNet;

namespace MySmup
{
    /// <summary>
    ///     This class represents an Urho3D plugin application.
    /// </summary>
    [LoadablePlugin]
    [Preserve(AllMembers = true)]
    public class UrhoPluginApplication : PluginApplication
    {
        /// <summary>
        ///     Safe pointer to game screen.
        /// </summary>
        private SharedPtr<GameState> _gameState;

        private SharedPtr<MyState> _myState;

        /// <summary>
        ///     Safe pointer to menu screen.
        /// </summary>
        private SharedPtr<MainMenuState> _mainMenuState;

        private SharedPtr<MyMenuState> _myMenuState;

        /// <summary>
        ///     Safe pointer to settings screen.
        /// </summary>
        private SharedPtr<SettingsMenuState> _settingsMenuState;

        /// <summary>
        ///     Application state manager.
        /// </summary>
        private StateStack _stateStack;

        private SharedPtr<ConfigFileContainer<GameSettings>> _settings;


        public UrhoPluginApplication(Context context) : base(context)
        {
        }

        /// <summary>
        ///     Game settings
        /// </summary>
        public GameSettings Settings => _settings.Ptr.Value;

        /// <summary>
        ///     Gets a value indicating whether the game is running.
        /// </summary>
        public bool IsGameRunning = false;

        protected override void Load()
        {
            Context.RegisterFactories(GetType().Assembly);
        }

        protected override void Unload()
        {
            Context.RemoveFactories(GetType().Assembly);
        }

        protected override void Suspend(Archive output)
        {
            base.Suspend(output);
        }

        protected override void Resume(Archive input, bool differentVersion)
        {
            base.Resume(input, differentVersion);
        }

        public override bool IsMain()
        {
            return true;
        }

        protected override void Start(bool isMain)
        {
            // Load settings.
            ResetSettings();

            _stateStack = new StateStack(Context.GetSubsystem<StateManager>());

            _myMenuState = new MyMenuState(this);

            // Setup state manager.
            var stateManager = Context.GetSubsystem<StateManager>();
            stateManager.FadeInDuration = 0.1f;
            stateManager.FadeOutDuration = 0.1f;

            // Setup end enqueue splash screen.
            using (SharedPtr<SplashScreen> splash = new SplashScreen(Context))
            {
                splash.Ptr.Duration = 0.5f;
                splash.Ptr.BackgroundImage = Context.ResourceCache.GetResource<Texture2D>("Images/Background.png");
                splash.Ptr.ForegroundImage = Context.ResourceCache.GetResource<Texture2D>("Images/Splash.png");
                stateManager.EnqueueState(splash);
            }


            // Crate end enqueue main menu screen.
            _stateStack.Push(_myMenuState);

            base.Start(isMain);
        }

        protected override void Stop()
        {
            _mainMenuState?.Dispose();
            _myState?.Dispose();

            base.Stop();
        }

        /// <summary>
        ///     Transition to settings menu
        /// </summary>
        public void ToSettings()
        {
            _settingsMenuState = _settingsMenuState ?? new SettingsMenuState(this);
            _stateStack.Push(_settingsMenuState);
        }

        /// <summary>
        ///     Transition to game
        /// </summary>
        public void ToNewGame()
        {
            _myState?.Dispose();
            _myState = new MyState(this);
            _stateStack.Push(_myState);
        }

        public void ToOldGame()
        {
            _gameState?.Dispose();
            _gameState = new GameState(this);
            _stateStack.Push(_gameState);
        }

        /// <summary>
        ///     Transition to game
        /// </summary>
        public void ContinueGame()
        {
            if (_myState) _stateStack.Push(_myState);
        }

        public void Quit()
        {
            Context.Engine.Exit();
        }

        public void HandleBackKey()
        {
            if (_stateStack.State == _myMenuState.Ptr)
            {
                if (IsGameRunning)
                    ContinueGame();
                else
                    Quit();
            }
            else
            {
                if (_stateStack.Count > 1)
                    _stateStack.Pop();
            }
        }

        public void PlayerDeath()
        {
            if (_stateStack.Count > 1)
                _stateStack.Pop();
        }

        public void SaveSettings()
        {
            _settings.Ptr.SaveConfig();
        }

        public void ResetSettings()
        {
            _settings?.Dispose();
            _settings = ConfigFileContainer<GameSettings>.LoadConfig(Context);
        }
    }
}