using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Blob0ngenClient.ViewModels
{
    public class ControlStoryboardAction : DependencyObject, IAction
    {
        public Storyboard Storyboard
        {
            get { return (Storyboard)GetValue(StoryboardProperty); }
            set { SetValue(StoryboardProperty, value); }
        }
        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(ControlStoryboardAction), new PropertyMetadata(0));
        

        public object Execute(object sender, object parameter)
        {
            Storyboard?.Begin();
            return null;
        }
    }
}
