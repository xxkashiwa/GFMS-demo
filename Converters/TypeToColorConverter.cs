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
                        // ��ɫ��ʾ����
                        return new SolidColorBrush(Colors.Green);
                    case "Punishment":
                        // ��ɫ��ʾ�ͷ�
                        return new SolidColorBrush(Colors.OrangeRed);
                    default:
                        // Ĭ�ϻ�ɫ
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