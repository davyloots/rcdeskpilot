using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Core.Interfaces;
using Bonsai.Core;
using Bonsai.Objects;
using Bonsai.Objects.Meshes;

namespace RCSim
{
    internal class Game : IFrameworkCallback, IDisposable
    {
        #region Protected fields
        protected GameType currentGameType = GameType.Racing;
        protected Program owner = null;
        protected Race race = null;
        protected ScareCrow scarecrow = null;
        protected Bombing bombing = null;
        protected Birds birds = null;
        //protected GameObject corn = new GameObject();
        #endregion

        #region Public enums
        public enum GameType
        {
            None,
            Racing,
            ScareCrow,
            Bombing,
            Editor
        }
        #endregion

        #region Public properties
        public GameType CurrentGameType
        {
            get { return currentGameType; }
            set 
            { 
                currentGameType = value;
                Cleanup();
                switch (currentGameType)
                {
                    case GameType.Racing:
                        race = new Race(owner);
                        owner.CenterHud.RestartButtonVisible = true;
                        break;
                    case GameType.Bombing:
                        bombing = new Bombing(owner);
                        break;
                    case GameType.ScareCrow:
                        scarecrow = new ScareCrow();
                        owner.CenterHud.RestartButtonVisible = true;
                        break;
                    default:
                        owner.CenterHud.RestartButtonVisible = false;
                        break;
                }
            }
        }
        #endregion

        #region Public constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner"></param>
        public Game(Program owner)
        {
            this.owner = owner;
            birds = new Birds(2);
            birds.Random = true;
            //XMesh cornMesh = new XMesh("data/corn.x");
            //corn.Mesh = cornMesh;
            //corn.Position = new Microsoft.DirectX.Vector3(-20, 0, 20);
            //corn.Scale = new Microsoft.DirectX.Vector3(2f, 2f, 2f);
            //owner.TransparentObjectManager.Objects.Add(corn);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            
        }
        #endregion

        #region Public methods
        public void Restart()
        {
            if (race != null)
            {
                race.Restart();
            }            
            if (scarecrow != null)
            {
                scarecrow.Restart();
            }
        }
        #endregion

        #region private methods
        private void Cleanup()
        {
            if (race != null)
            {
                race.Dispose();
                race = null;
            }
            if (bombing != null)
            {
                bombing.Dispose();
                bombing = null;
            }
            if (scarecrow != null)
            {
                scarecrow.Dispose();
                scarecrow = null;
            }
        }
        #endregion

        #region IFrameworkCallback Members
        public void OnFrameMove(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            switch (currentGameType)
            {
                case GameType.None:
                    break;
                case GameType.Racing:
                    if (race != null)
                        race.OnFrameMove(device, totalTime, elapsedTime);
                    break;
                case GameType.Bombing:
                    if (bombing != null)
                        bombing.OnFrameMove(device, totalTime, elapsedTime);
                    break;
                case GameType.ScareCrow:
                    if (scarecrow != null)
                        scarecrow.OnFrameMove(device, totalTime, elapsedTime);
                    break;
            }
            birds.OnFrameMove(device, totalTime, elapsedTime);
        }

        public void OnFrameRender(Microsoft.DirectX.Direct3D.Device device, double totalTime, float elapsedTime)
        {
            switch (currentGameType)
            {
                case GameType.None:
                    break;
                case GameType.Racing:
                    if (race != null)
                        race.OnFrameRender(device, totalTime, elapsedTime);
                    break;
                case GameType.Bombing:
                    if (bombing != null)
                        bombing.OnFrameRender(device, totalTime, elapsedTime);
                    break;
                case GameType.ScareCrow:
                    if (scarecrow != null)
                        scarecrow.OnFrameRender(device, totalTime, elapsedTime);
                    break;
            }
            birds.OnFrameRender(device, totalTime, elapsedTime);
        }
        #endregion

        
    }
}
