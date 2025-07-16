using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace GFMS.Converters
{
    /// <summary>
    /// 档案状态到颜色的转换器
    /// </summary>
    public class FileStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status switch
                {
                    "已审核" => new SolidColorBrush(Colors.Green),
                    "已上传" => new SolidColorBrush(Colors.Orange),
                    "驳回" => new SolidColorBrush(Colors.Red),
                    "未上传" => new SolidColorBrush(Colors.Gray),
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