using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.OtherClasses
{

    /// <summary>
    /// Dynamically data-binds XAML to objects.
    /// Taken from https://www.youtube.com/watch?v=tKfpvs7ZIyo
    /// </summary>
    public abstract class ObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }
    }
}
