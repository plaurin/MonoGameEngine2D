using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using GameFramework;
using GameFramework.Screens;

namespace GameNavigator.Navigator
{
    public class NavigatorViewModel : ViewModelBase
    {
        private readonly IScreen screen;
        private readonly GameResourceManager gameResourceManager;
        private readonly List<NavigatorNode> nodes;
        private readonly DelegateCommand refreshCommand;
        private readonly DelegateCommand pausePlayCommand;
        private readonly DelegateCommand oneFrameCommand;
        private readonly DelegateCommand quitCommand;

        private bool shouldPause;
        private bool shouldPlayOneFrame;
        private bool shouldExit;
        private float lastTimeUpdateInspector;

        private string gameTime;
        private string status;
        private string updateFps;
        private string drawFps;
        private string frameCount;
        private int frameCounter;
        private NavigatorNode currentSelection;
        private string playButtonLabel = "Pause";

        public NavigatorViewModel()
        {
            this.nodes = DesignTimeData.GetNodes();

            this.refreshCommand = new DelegateCommand(
                p => this.RefreshNode(this.CurrentSelection), p => this.CurrentSelection != null);

            this.pausePlayCommand = new DelegateCommand(p =>
            {
                this.shouldPause = !this.shouldPause;
                this.PlayButtonLabel = this.shouldPause ? "Play" : "Pause";
                this.oneFrameCommand.RaiseCanExecuteChanged();
            });
            this.oneFrameCommand = new DelegateCommand(p => this.shouldPlayOneFrame = true, p => this.shouldPause);
            this.quitCommand = new DelegateCommand(p => this.shouldExit = true);

            this.Status = "Hi!";
        }

        public NavigatorViewModel(IScreen screen, GameResourceManager gameResourceManager)
            : this()
        {
            this.screen = screen;
            this.gameResourceManager = gameResourceManager;

            this.RefreshTree();
        }

        public event EventHandler RefreshInspector;

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

        public ICommand QuitCommand
        {
            get { return this.quitCommand; }
        }

        public IEnumerable<NavigatorNode> Nodes
        {
            get { return this.nodes; }
        }

        public NavigatorNode CurrentSelection
        {
            get
            {
                return this.currentSelection;
            }

            set
            {
                this.currentSelection = value;
                this.OnPropertyChanged();
                
                this.refreshCommand.RaiseCanExecuteChanged();
                this.RefreshChildNodes(this.CurrentSelection);
            }
        }

        public string PlayButtonLabel
        {
            get
            {
                return this.playButtonLabel;
            }

            private set
            {
                if (this.playButtonLabel != value)
                {
                    this.playButtonLabel = value;
                    this.OnPropertyChanged();
                }
            }
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

            if (gameTiming.TotalSeconds - this.lastTimeUpdateInspector > 1)
            {
                this.OnRefreshInspector();
                this.lastTimeUpdateInspector = gameTiming.TotalSeconds;
            }

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

            this.nodes.Add(this.CreateNavigatorNode(this.gameResourceManager, this.GetNavigatorNodes(this.gameResourceManager)));
            this.nodes.AddRange(this.GetNavigatorNodes(this.screen));
        }

        private void RefreshNode(NavigatorNode navigatorNode)
        {
            var childNodes = this.GetNavigatorNodes(navigatorNode.SceneNode);
            // TODO: Should remove event handler
            navigatorNode.UpdateNodes(childNodes);
        }

        private void RefreshChildNodes(NavigatorNode navigatorNode)
        {
            if (navigatorNode.Kind != NodeKind.Unknown)
                this.RefreshNode(navigatorNode);
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

                        yield return this.CreateNavigatorNode(child, childNodes);
                    }
                }
            }
        }

        private NavigatorNode CreateNavigatorNode(object sceneObject, IEnumerable<NavigatorNode> childNodes)
        {
            NavigatorNode node;

            var metadataProvider = sceneObject as INavigatorMetadataProvider;
            if (metadataProvider != null)
            {
                var metadata = metadataProvider.GetMetadata();
                node = new NavigatorNode(sceneObject, metadata.Name, metadata.Kind, childNodes);
            }
            else
            {
                node = new NavigatorNode(sceneObject, sceneObject.ToString(), childNodes);
            }

            node.PropertyChanged += this.NodePropertyChanged;

            return node;
        }

        private void NodePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "IsSelected")
            {
                var node = sender as NavigatorNode;
                if (node != null && node.IsSelected)
                {
                    this.CurrentSelection = node;
                }
            }

            if (eventArgs.PropertyName == "IsExpanded")
            {
                var node = sender as NavigatorNode;
                if (node != null && node.IsExpanded)
                {
                    this.RefreshChildNodes(node);
                }
            }
        }

        private void OnRefreshInspector()
        {
            var handler = this.RefreshInspector;
            if (handler != null) handler.Invoke(this, EventArgs.Empty);
        }
    }
}
