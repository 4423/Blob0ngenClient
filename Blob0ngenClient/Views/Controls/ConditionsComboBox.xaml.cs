using Blob0ngenClient.Models;
using Blob0ngenClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Blob0ngenClient.Views.Controls
{
    public interface IConditionsComboBoxItem { }

    public class ConditionsComboBoxItemList : List<IConditionsComboBoxItem> { }


    public sealed partial class ConditionsComboBox : UserControl
    {
        public ConditionsComboBox()
        {
            this.InitializeComponent();
        }
        

        public ConditionsComboBoxItemList OrderComboBoxItems
        {
            get { return (ConditionsComboBoxItemList)GetValue(OrderComboBoxItemsProperty); }
            set { SetValue(OrderComboBoxItemsProperty, value); }
        }
        public static readonly DependencyProperty OrderComboBoxItemsProperty =
            DependencyProperty.Register("OrderComboBoxItems", typeof(ConditionsComboBoxItemList), typeof(ConditionsComboBox), new PropertyMetadata(null));

        public ConditionsComboBoxItemList FilterComboBoxItems
        {
            get { return (ConditionsComboBoxItemList)GetValue(FilterComboBoxItemsProperty); }
            set { SetValue(FilterComboBoxItemsProperty, value); }
        }
        public static readonly DependencyProperty FilterComboBoxItemsProperty =
            DependencyProperty.Register("FilterComboBoxItems", typeof(ConditionsComboBoxItemList), typeof(ConditionsComboBox), new PropertyMetadata(null));
        

        public IConditionsComboBoxItem SelectedOrderComboBoxItem
        {
            get { return (IConditionsComboBoxItem)GetValue(SelectedOrderComboBoxItemProperty); }
            set { SetValue(SelectedOrderComboBoxItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedOrderComboBoxItemProperty =
            DependencyProperty.Register("SelectedOrderComboBoxItem", typeof(IConditionsComboBoxItem), typeof(ConditionsComboBox), new PropertyMetadata(null));
        
        public IConditionsComboBoxItem SelectedFilterComboBoxItem
        {
            get { return (IConditionsComboBoxItem)GetValue(SelectedFilterComboBoxItemProperty); }
            set { SetValue(SelectedFilterComboBoxItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedFilterComboBoxItemProperty =
            DependencyProperty.Register("SelectedFilterComboBoxItem", typeof(IConditionsComboBoxItem), typeof(ConditionsComboBox), new PropertyMetadata(null));
    }
}
