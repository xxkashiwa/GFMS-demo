using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFMS.Converters
{
    /// <summary>
    /// 转递状态枚举到字符串列表的转换器
    /// </summary>
    public class TransferStateToStringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new List<string> { "档案预备中", "转递中", "已完成" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 转递状态到字符串的转换器
    /// </summary>
    public class TransferStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Models.TransferState state)
            {
                return state switch
                {
                    Models.TransferState.档案预备中 => "档案预备中",
                    Models.TransferState.转递中 => "转递中",
                    Models.TransferState.已完成 => "已完成",
                    _ => "档案预备中"
                };
            }
            return "档案预备中";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stateString)
            {
                return stateString switch
                {
                    "档案预备中" => Models.TransferState.档案预备中,
                    "转递中" => Models.TransferState.转递中,
                    "已完成" => Models.TransferState.已完成,
                    _ => Models.TransferState.档案预备中
                };
            }
            return Models.TransferState.档案预备中;
        }
    }
}