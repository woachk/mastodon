using System;
using Windows.UI.Xaml.Data;

namespace MastodonUWA {
    public class DivisionConverter : IValueConverter

    {

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            // Default to 0. You may want to handle divide by zero 
            // and other issues differently than this.
            double result = 0;

            // Not the best code ever, but you get the idea.
            if (value != null && parameter != null)
            {
                try
                {
                    double numerator = (double)value;
                    double denominator = double.Parse(parameter.ToString());

                    if (denominator != 0)
                    {
                        result = numerator / denominator;
                    }
                    else
                    {
                        // TODO: Handle divide by zero senario.
                    }
                }
                catch (Exception e)
                {
                    // TODO: Handle casting exceptions.
                }
            }

            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            // Default to 0. You may want to handle divide by zero 
            // and other issues differently than this.
            double result = 0;

            // Not the best code ever, but you get the idea.
            if (value != null && parameter != null)
            {
                try
                {
                    double numerator = (double)value;
                    double denominator = double.Parse(parameter.ToString());
                    result = numerator * denominator;
                }
                catch (Exception e)
                {
                    // TODO: Handle casting exceptions.
                }
            }

            return result;
        }
    }
}