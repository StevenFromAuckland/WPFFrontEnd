using HillLabTestEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private int _ProductId = -1;
        public Action CallbackAction  { get; set;}

        public ProductWindow()
        {
            this.SourceInitialized += (object sender, EventArgs e) =>
            {
                MainWindow.Window_SourceInitialized(sender, e);
            };

            DataContext = new Product();

            InitializeComponent();
        }
        public async void SetProductId(int productId)
        {
            _ProductId = productId;
            if (_ProductId > 0)
            {
                ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                var result = await wrapper.Product.GetProductById(_ProductId);
                DataContext = result;
            }
        }
        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product c = DataContext as Product;
                if (c != null)
                {
                    if (string.IsNullOrEmpty(c.ProductName))
                    {
                        MessageBox.Show("The product name cannot be empty!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else if (c.Quantity <= 0)
                    {
                        MessageBox.Show("The product quantity cannot be negative!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else if (c.CategoryId <= 0)
                    {
                        MessageBox.Show("Please select one category!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (_ProductId > 0)
                    {
                        ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                        await wrapper.Product.UpdateProduct(c);
                    }
                    else
                    {
                        ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                        await wrapper.Product.CreateProduct(c);
                    }
                    Close();
                    if (CallbackAction != null)
                        CallbackAction();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Failed to save the product!\r\nError Message:\r\n{0}", ex) , this.Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
            var result = await wrapper.Category.GetAllCategories();
            List<Category> lstCategory = new List<Category>(result);
            lstCategory.Insert(0, new Category() { CategoryId = 0, CategoryName = "--- Please select one category ---" });
            cbCategory.ItemsSource = lstCategory;
        }
    }
}
