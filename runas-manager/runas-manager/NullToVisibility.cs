using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace runas_manager
{
    /// <summary>
    /// Returns Collapsed if the value is null, Visible otherwise.
    /// </summary>
    public class NullToVisibility : IValueConverter
    {
        /// <summary>
        /// Convertit une valeur.
        /// </summary>
        /// <param name="value">Valeur produite par la source de liaison.</param>
        /// <param name="targetType">Type de la propriété de cible de liaison.</param>
        /// <param name="parameter">Paramètre de convertisseur à utiliser.</param>
        /// <param name="culture">Culture à utiliser dans le convertisseur.</param>
        /// <returns>
        /// Une valeur convertie.Si la méthode retourne null, la valeur Null valide est utilisée.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int p = 0;
            if (parameter == null || int.TryParse(parameter.ToString(), out p))
            {
                switch (p)
                {
                    case 0:
                        return value == null ? Visibility.Collapsed : Visibility.Visible;
                    case 1:
                        return value == null ? Visibility.Visible : Visibility.Collapsed;
                    case 2:
                        return value == null ? Visibility.Hidden : Visibility.Visible;
                    case 3:
                        return value == null ? Visibility.Visible : Visibility.Hidden;
                }
            }
            return Visibility.Visible;
        }

        /// <summary>
        /// Convertit une valeur.
        /// </summary>
        /// <param name="value">Valeur produite par la cible de liaison.</param>
        /// <param name="targetType">Type dans lequel convertir.</param>
        /// <param name="parameter">Paramètre de convertisseur à utiliser.</param>
        /// <param name="culture">Culture à utiliser dans le convertisseur.</param>
        /// <returns>
        /// Une valeur convertie.Si la méthode retourne null, la valeur Null valide est utilisée.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

