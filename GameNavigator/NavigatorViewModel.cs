using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using GameFramework;
using GameFramework.Screens;

namespace GameNavigator
{
    public class NavigatorViewModel : ViewModelBase
    {
        private readonly IScreen screen;
        private readonly List<NavigatorNode> nodes;
        private readonly ICommand refreshCommand;
        private readonly ICommand pausePlayCommand;
        private readonly ICommand oneFrameCommand;
        private readonly ICommand exitCommand;

        private bool shouldPause;
        private bool shouldPlayOneFrame;
        private bool shouldExit;

        private string gameTime;
        private string status;
        private string updateFps;
        private string drawFps;
        private string frameCount;
        private int frameCounter;

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

        public NavigatorNode CurrentSelection { get; set; }

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

        public string UpdateFps
        {
            get
            {
                return this.updateFps;
            }

            private set
            {
                if (this.updateFps != value)
                {
                    this.updateFps = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string DrawFps
        {
            get
            {
                return this.drawFps;
            }

            private set
            {
                if (this.drawFps != value)
                {
                    this.drawFps = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string FrameCount
        {
            get
            {
                return this.frameCount;
            }

            private set
            {
                if (this.frameCount != value)
                {
                    this.frameCount = value;
                    this.OnPropertyChanged();
                }
            }
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
            this.frameCounter++;

            this.UpdateFps = "Updt:" + gameTiming.UpdateFps;
            this.DrawFps = "Draw:" + gameTiming.DrawFps;
            this.FrameCount = "#" + this.frameCounter;
            this.GameTime = gameTiming.TotalSeconds.ToString("f2");

            var result = new NavigatorMessage
            {
                ShouldPlay = !this.shouldPause || this.shouldPlayOneFrame,
                ShouldExit = this.shouldExit
            };

            this.shouldPlayOneFrame = false;

            return result;
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

                        //if (childNodes != null)
                        //{
                        //    yield return this.CreateNavigatorNode(child, childNodes);
                        yield return this.CreateNavigatorNode(child, childNodes);
                        //}
                    }
                }
            }
        }

        private NavigatorNode CreateNavigatorNode(object child, IEnumerable<NavigatorNode> childNodes)
        {
            NavigatorNode node;

            var metadataProvider = child as INavigatorMetadataProvider;
            if (metadataProvider != null)
            {
                var metadata = metadataProvider.GetMetadata();
                node = new NavigatorNode
                {
                    Label = metadata.Name,
                    Nodes = childNodes
                };
            }
            else
            {
                node = new NavigatorNode
                {
                    Label = child.ToString(),
                    Nodes = childNodes
                };
            }

            node.PropertyChanged += this.NodePropertyChanged;

            return node;
        }

        private void NodePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "IsSelected")
            {
                var node = sender as NavigatorNode;
                if (node != null)
                    this.CurrentSelection = node;
            }
        }
    }
}
