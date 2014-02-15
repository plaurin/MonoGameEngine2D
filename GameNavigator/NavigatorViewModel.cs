using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GameFramework;
using GameFramework.Screens;

namespace GameNavigator
{
    public class NavigatorViewModel : INotifyPropertyChanged
    {
        private readonly IScreen screen;
        private readonly List<NavigatorNode> nodes;
        private readonly ICommand refreshCommand;
        private readonly ICommand pausePlayCommand;
        private readonly ICommand oneFrameCommand;
        private readonly ICommand exitCommand;

        private string gameTime;
        private string status;

        private bool shouldPause;
        private bool shouldPlayOneFrame;
        private bool shouldExit;

        public NavigatorViewModel()
        {
            this.nodes = DesignTimeData.GetNodes();

            this.refreshCommand = new DelegateCommand(p => {});
            this.pausePlayCommand = new DelegateCommand(p => this.shouldPause = !this.shouldPause);
            this.oneFrameCommand = new DelegateCommand(p => this.shouldPlayOneFrame = true);
            this.exitCommand = new DelegateCommand(p => this.shouldExit = true);

            this.Status = "Hi!";
        }

        public NavigatorViewModel(IScreen screen) : this()
        {
            this.screen = screen;

            this.RefreshTree();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public ICommand PausePlayCommand
        {
            get { return this.pausePlayCommand; }
        }

        public ICommand OneFrameCommand
        {
            get { return this.oneFrameCommand; }
        }

        public ICommand ExitCommand
        {
            get { return this.exitCommand; }
        }

        public IEnumerable<NavigatorNode> Nodes
        {
            get { return this.nodes; }
        }

        public string Status
        {
            get
            {
                return this.status;
            }

            private set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int TotalNodes
        {
            get { return this.nodes.Count; }
        }

        public string GameTime
        {
            get
            {
                return this.gameTime;
            }

            private set
            {
                if (this.gameTime != value)
                {
                    this.gameTime = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public NavigatorMessage Update(IGameTiming gameTiming)
        {
            this.GameTime = gameTiming.TotalSeconds.ToString("f2");

            var result = new NavigatorMessage
            {
                ShouldPlay = !this.shouldPause || this.shouldPlayOneFrame,
                ShouldExit = this.shouldExit
            };

            this.shouldPlayOneFrame = false;

            return result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshTree()
        {
            this.nodes.Clear();
            this.nodes.AddRange(this.GetNavigatorNodes(this.screen));
        }

        private IEnumerable<NavigatorNode> GetNavigatorNodes(object sceneNode)
        {
            var composite = sceneNode as IComposite;
            if (composite != null)
            {
                foreach (var child in composite.Children)
                {
                    if (child != null)
                    {
                        IEnumerable<NavigatorNode> childNodes = null;
                        try
                        {
                            childNodes = this.GetNavigatorNodes(child);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                        if (childNodes != null)
                        {
                            yield return new NavigatorNode
                            {
                                Label = child.ToString(),
                                Nodes = childNodes
                            };
                        }
                    }
                }
            }
        }
    }
}
