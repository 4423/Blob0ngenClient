using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Blob0ngenClient.Converters
{
    public class NullToDefaultCoverArtUriConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                return new Uri(s);
            }
            if (value == null)
            {
                return new Uri(DefaultCoverArtUri);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        
        public string DefaultCoverArtUri
        {
            get { return (string)GetValue(DefaultCoverArtUriProperty); }
            set { SetValue(DefaultCoverArtUriProperty, value); }
        }
        public static readonly DependencyProperty DefaultCoverArtUriProperty =
            DependencyProperty.Register("DefaultCoverArtUri", typeof(string), typeof(NullToDefaultCoverArtUriConverter), new PropertyMetadata(null));
    }
}
