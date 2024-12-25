using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProductListApp;

namespace ProductListApp
{
    public partial class MainWindow : Window
    {
        private List<Product> _products;
        private List<Product> _filteredProducts;
        private bool _isSortAscending = true;

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
            SetupFilters();
            UpdateProductList();
        }

        private void LoadProducts()
        {
            _products = new List<Product>
            {
                new Product { Name = "Щетка-варежка", Description = "Щетка-варежка True Touch для ухода за шерстью", Manufacturer = "True Touch", Price = 259.00, Stock = 0, ImagePath = "Images/brush_glove.png" },
                new Product { Name = "Миска", Description = "Миска Triol 9002/KIDP3211/30", Manufacturer = "Triol", Price = 385.00, Stock = 17, ImagePath = "Images/bowl.png" },
                new Product { Name = "Товар 3", Description = "Описание 3", Manufacturer = "Производитель A", Price = 150, Stock = 8, ImagePath = "" }
            };
            _filteredProducts = new List<Product>(_products);
        }

        private void SetupFilters()
        {
            ManufacturerFilter.Items.Add("Все производители");

            foreach (var manufacturer in _products.Select(p => p.Manufacturer).Distinct())
            {
                ManufacturerFilter.Items.Add(manufacturer);
            }

            ManufacturerFilter.SelectedIndex = 0;
        }

        private void UpdateProductList()
        {
            ProductList.ItemsSource = null;
            ProductList.ItemsSource = _filteredProducts;
        }

        private void ApplyFilters()
        {
            string searchText = SearchBox.Text.ToLower();
            string selectedManufacturer = ManufacturerFilter.SelectedItem?.ToString();

            _filteredProducts = _products.Where(p =>
                (string.IsNullOrEmpty(searchText) || p.Name.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText)) &&
                (selectedManufacturer == "Все производители" || p.Manufacturer == selectedManufacturer)).ToList();

            UpdateProductList();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ManufacturerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _isSortAscending = !_isSortAscending;
            _filteredProducts = _isSortAscending
                ? _filteredProducts.OrderBy(p => p.Price).ToList()
                : _filteredProducts.OrderByDescending(p => p.Price).ToList();

            UpdateProductList();
        }
    }
}