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
    /// Base view model that implements INotifyPropertyChanged
    /// </summary>
    public class NotifierBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event when a property changed 
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.NotifyPropertyChanged(PropertyChanged, propertyName);
        }

        /// <summary>
        /// Notifies that a property changed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVariable">The type of the variable.</typeparam>
        /// <param name="e">The e.</param>
        protected void OnPropertyChanged<TVariable>(Expression<Func<TVariable>> e)
        {
            this.NotifyPropertyChanged(e, PropertyChanged);
        }
    }
}
