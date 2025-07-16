using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace GFMS.Converters
{
    /// <summary>
    /// ����״̬����ɫ��ת����
    /// </summary>
    public class FileStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status switch
                {
                    "�����" => new SolidColorBrush(Colors.Green),
                    "���ϴ�" => new SolidColorBrush(Colors.Orange),
                    "����" => new SolidColorBrush(Colors.Red),
                    "δ�ϴ�" => new SolidColorBrush(Colors.Gray),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}