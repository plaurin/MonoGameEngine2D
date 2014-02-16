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
        private readonly DelegateCommand refreshCommand;
        private readonly DelegateCommand pausePlayCommand;
        private readonly DelegateCommand oneFrameCommand;
        private readonly DelegateCommand quitCommand;

        private bool shouldPause;
        private bool shouldPlayOneFrame;
        private bool shouldExit;

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

        public NavigatorViewModel(IScreen screen)
            : this()
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
                this.refreshCommand.RaiseCanExecuteChanged();
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

        private void RefreshNode(NavigatorNode navigatorNode)
        {
            var childNodes = this.GetNavigatorNodes(navigatorNode.SceneNode);
            // TODO: Should remove event handler
            navigatorNode.UpdateNodes(childNodes);
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
                node = new NavigatorNode(sceneObject, metadata.Name, childNodes, GetIconFromKind(metadata.Kind));
            }
            else
            {
                node = new NavigatorNode(sceneObject, sceneObject.ToString(), childNodes, GetIconFromKind(NodeKind.Unknown));
            }

            node.PropertyChanged += this.NodePropertyChanged;

            return node;
        }

        private static string GetIconFromKind(NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.ScreenState:
                    return "Icons/ScreenState.png";
                case NodeKind.Screen:
                    return "Icons/Screen.png";
                case NodeKind.Scene:
                    return "Icons/Scene.png";
                case NodeKind.Layer:
                    return "Icons/Layer.png";
                case NodeKind.Entity:
                    return "Icons/Entity.png";
                case NodeKind.Utility:
                    return "Icons/Utility.png";
                case NodeKind.Unknown:
                    return "Icons/Unknown.png";
                default: 
                    throw new InvalidOperationException(kind + " not yet implemented");
            }
        }

        private void NodePropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == "IsSelected")
            {
                var node = sender as NavigatorNode;
                if (node != null)
                {
                    this.CurrentSelection = node;
                    this.Status = node.Label;
                }
            }
        }
    }
}
