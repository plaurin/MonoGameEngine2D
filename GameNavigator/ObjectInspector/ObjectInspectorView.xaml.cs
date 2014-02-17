using System;
using GameNavigator.Navigator;

namespace GameNavigator.ObjectInspector
{
    public partial class ObjectInspectorView
    {
        public ObjectInspectorView(NavigatorViewModel navigatorViewModel)
        {
            this.InitializeComponent();

            this.DataContext = navigatorViewModel;

            navigatorViewModel.RefreshInspector += (s, e) => this.PropertyGrid.Update();
        }
    }
}
