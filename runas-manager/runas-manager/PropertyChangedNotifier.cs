using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace runas_manager
{
    /// <summary>
    /// Method extension class
    /// </summary>
    public static class PropertyChangedNotifier
    {
        /// <summary>
        /// Notifies that the property has changed.
        /// The calling type has to implement INotifyPropertyChanged.
        /// The function checks that the event handler is not null.
        /// It has to be called this way:
        /// this.NotifyPropertyChanged(() => this.AnyVariable, PropertyChanged);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVariable">The type of the variable.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="e">Lambda expression with no input parameter that outputs the variable.</param>
        /// <param name="PropertyChanged">The property changed.</param>
        public static void NotifyPropertyChanged<T, TVariable>(this T source, Expression<Func<TVariable>> e, PropertyChangedEventHandler? PropertyChanged)
            where T : INotifyPropertyChanged
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(((MemberExpression)e.Body).Member.Name));
        }

        /// <summary>
        /// Notifies that the property has changed.
        /// The calling type has to implement INotifyPropertyChanged.
        /// The function checks that the event handler is not null.
        /// It has to be called this way:
        /// this.NotifyPropertyChanged(PropertyChanged); //will use implicitly the name of the calling member
        /// this.NotifyPropertyChanged(PropertyChanged,"membername");
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="PropertyChanged">The property changed.</param>
        /// <param name="memberName">Name of the member.</param>
        public static void NotifyPropertyChanged<T>(this T source, PropertyChangedEventHandler? PropertyChanged, [CallerMemberName] string? memberName = null)
           where T : INotifyPropertyChanged
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(memberName));
        }
    }
}
