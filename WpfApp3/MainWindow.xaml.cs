using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        public ReadOnlyObservableCollection<ItemViewModel> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Items = new ReadOnlyObservableCollection<ItemViewModel>(_items);

            AddDummyItems(50);

            tvitems.ItemsSource = Items;
        }

        private void AddDummyItems(int count)
        {
            foreach (ItemViewModel item in CreateDummyItems(count, 0.25, new Random()))
            {
                _items.Add(item);
            }
        }

        private IEnumerable<ItemViewModel> CreateDummyItems(int count, double childCreationProbability, Random random)
        {
            for (int i = 0; i < count; i++)
            {
                var item = new ItemViewModel
                {
                    Name = CreateItemName(i + 1),
                    Weight = random.NextDouble(),
                };

                if (random.NextDouble() <= childCreationProbability)
                {
                    foreach (ItemViewModel child in CreateDummyItems(random.Next(count), childCreationProbability, random))
                    {
                        item.AddChild(child);
                    }
                }

                yield return item;
            }
        }

        public ItemViewModel CreateItemInItems()
        {
            var newItem = new ItemViewModel
            {
                Name = GetNextUniqueItemName(),
                Weight = new Random().NextDouble(),
            };

            _items.Add(newItem);
            return newItem;
        }

        private string GetNextUniqueItemName()
        {
            int iteration = 1;
            string itemName = CreateItemName(iteration);

            while (_items.Any(item => item.Name == itemName))
            {
                iteration++;
                itemName = CreateItemName(iteration);
            }

            return itemName;
        }

        private string CreateItemName(int itemId)
        {
            return $"Item {itemId}";
        }

    }
}
