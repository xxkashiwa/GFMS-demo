using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFMS.Converters
{
    /// <summary>
    /// ת��״̬ö�ٵ��ַ����б��ת����
    /// </summary>
    public class TransferStateToStringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new List<string> { "����Ԥ����", "ת����", "�����" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// ת��״̬���ַ�����ת����
    /// </summary>
    public class TransferStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Models.TransferState state)
            {
                return state switch
                {
                    Models.TransferState.����Ԥ���� => "����Ԥ����",
                    Models.TransferState.ת���� => "ת����",
                    Models.TransferState.����� => "�����",
                    _ => "����Ԥ����"
                };
            }
            return "����Ԥ����";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stateString)
            {
                return stateString switch
                {
                    "����Ԥ����" => Models.TransferState.����Ԥ����,
                    "ת����" => Models.TransferState.ת����,
                    "�����" => Models.TransferState.�����,
                    _ => Models.TransferState.����Ԥ����
                };
            }
            return Models.TransferState.����Ԥ����;
        }
    }
}