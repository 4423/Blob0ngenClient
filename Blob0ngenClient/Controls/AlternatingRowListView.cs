using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Blob0ngenClient.Controls
{
    public class AlternatingRowListView : ListView
    {
        public AlternatingRowListView()
        {
            DefaultStyleKey = typeof(ListView);
            Items.VectorChanged += OnItemsVectorChanged;
        }
        

        private void OnItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
        {
            if (args.CollectionChange == CollectionChange.ItemRemoved)
            {
                var removedItemIndex = (int)args.Index;
                for (var i = removedItemIndex; i < Items.Count; i++)
                {
                    if (ContainerFromIndex(i) is ListViewItem listViewItem)
                    {
                        listViewItem.Background = i % 2 == 0 ? EvenColor : OddColor;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is ListViewItem listViewItem)
            {
                var i = IndexFromContainer(element);
                listViewItem.Background = i % 2 == 0 ? EvenColor : OddColor;
            }
        }
        

        public SolidColorBrush EvenColor
        {
            get { return (SolidColorBrush)GetValue(EvenColorProperty); }
            set { SetValue(EvenColorProperty, value); }
        }        
        public static readonly DependencyProperty EvenColorProperty =
            DependencyProperty.Register("EvenColor", typeof(SolidColorBrush), typeof(AlternatingRowListView), new PropertyMetadata(0));
        

        public SolidColorBrush OddColor
        {
            get { return (SolidColorBrush)GetValue(OddColorProperty); }
            set { SetValue(OddColorProperty, value); }
        }        
        public static readonly DependencyProperty OddColorProperty =
            DependencyProperty.Register("OddColor", typeof(SolidColorBrush), typeof(AlternatingRowListView), new PropertyMetadata(0));
    }
}
