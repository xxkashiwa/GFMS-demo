using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace GFMS.Converters
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string type)
            {
                switch (type)
                {
                    case "Reward":
                        // 绿色表示奖励
                        return new SolidColorBrush(Colors.Green);
                    case "Punishment":
                        // 红色表示惩罚
                        return new SolidColorBrush(Colors.OrangeRed);
                    default:
                        // 默认灰色
                        return new SolidColorBrush(Colors.Gray);
                }
            }
            
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}