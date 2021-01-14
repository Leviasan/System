using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    /// <summary>
    /// Represents the notify property.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    public sealed class NotifyProperty<T> : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// The property value.
        /// </summary>
        private T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyProperty{T}"/> class.
        /// </summary>
        public NotifyProperty() : this(default) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyProperty{T}"/> class with specified value.
        /// </summary>
        /// <param name="value"></param>
        public NotifyProperty(T value) => _value = value;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                OnPropertyChanging();
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// PropertyChanging event handler.
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanging([CallerMemberName] string name = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }
        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
