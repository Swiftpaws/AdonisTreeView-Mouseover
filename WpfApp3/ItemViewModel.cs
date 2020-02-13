using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
   public class ViewModel
        : INotifyPropertyChanged
            , IDataErrorInfo
    {
        private readonly Dictionary<string, IList<string>> _validationErrors = new Dictionary<string, IList<string>>();

        public string this[string propertyName]
        {
            get
            {
                if (String.IsNullOrEmpty(propertyName))
                    return Error;

                if (_validationErrors.ContainsKey(propertyName))
                    return String.Join(Environment.NewLine, _validationErrors[propertyName]);

                return String.Empty;
            }
        }

        public string Error => String.Join(Environment.NewLine, GetAllErrors());

        private IEnumerable<string> GetAllErrors()
        {
            return _validationErrors.SelectMany(kvp => kvp.Value).Where(e => !String.IsNullOrEmpty(e));
        }

        public void AddValidationError(string propertyName, string errorMessage)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors.Add(propertyName, new List<string>());

            _validationErrors[propertyName].Add(errorMessage);
        }

        public void ClearValidationErrors(string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ItemViewModel
        : ViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;

                    ClearValidationErrors(nameof(Name));

                    if (String.IsNullOrEmpty(value))
                        AddValidationError(nameof(Name), "Name must not be null or empty.");

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private double _weight;

        public double Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;

                    RaisePropertyChanged(nameof(Weight));
                }
            }
        }

        private bool _flag;
        public bool Flag
        {
            get => _flag;
            set
            {
                if (_flag != value)
                {
                    _flag = value;

                    RaisePropertyChanged(nameof(Flag));
                }
            }
        }

        private readonly ObservableCollection<ItemViewModel> _children = new ObservableCollection<ItemViewModel>();

        public ReadOnlyObservableCollection<ItemViewModel> Children { get; set; }

        public ItemViewModel()
        {
            Children = new ReadOnlyObservableCollection<ItemViewModel>(_children);
        }

        public void AddChild(ItemViewModel child)
        {
            _children.Add(child);
        }
    }
}
