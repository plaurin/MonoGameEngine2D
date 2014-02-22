﻿using System;
using MonoGameImplementation;
using SamplesBrowser;
using SamplesBrowser.Sandbox;
using SamplesBrowser.ShootEmUp;

namespace WindowsSamples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SamplesBrowserWin : MonoGameBase
    {
        //public SamplesBrowserWin() : base(new SandboxScreen(new SampleBrowserScreenNavigation()))
        //public SamplesBrowserWin() : base(new HubScreen())
        //public SamplesBrowserWin() : base(new ShootEmUpScreen(new SampleBrowserScreenNavigation()))
        public SamplesBrowserWin() : base(new SampleBrowserScreenNavigation())
        {
        }
    }
}
