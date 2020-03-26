using HillLabTestEntities;
using System;
using System.Collections.Generic;
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
using WPFUI.UIComponents;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow cw = new ProductWindow();
            cw.CallbackAction = async () => { await LoadData(); };
            cw.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Product c = ((FrameworkElement)sender).DataContext as Product;
            ProductWindow cw = new ProductWindow();
            cw.SetProductId(c.ProductId);
            cw.CallbackAction = async () => { await LoadData(); };
            cw.Show();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Product c = ((FrameworkElement)sender).DataContext as Product;
            if (MessageBox.Show(string.Format("Are you sure to delete the following product?\r\n{0}", c.ProductName), this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                await wrapper.Product.DeleteProduct(c.ProductId);
                await LoadData();

            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
            var result = await wrapper.Product.GetAllProducts();
            grdProduct.ItemsSource = result;

        }
    }
}
