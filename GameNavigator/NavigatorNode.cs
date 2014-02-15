using System.Collections.Generic;

namespace GameNavigator
{
    public class NavigatorNode : ViewModelBase
    {
        private bool isSelected;

        public IEnumerable<NavigatorNode> Nodes { get; set; }

        public string Label { get; set; }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}