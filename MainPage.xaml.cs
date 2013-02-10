using DevExpress.UI.Xaml.Layout;
using System;
using Windows.UI.Xaml.Data;
namespace WhatsThatSound
{
    public sealed partial class MainPage : DXPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
namespace WhatsThatSound.View
{
    public class FormatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(parameter.ToString(), value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
