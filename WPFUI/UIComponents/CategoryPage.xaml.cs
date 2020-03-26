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
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        public CategoryPage()
        {
            InitializeComponent();

        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow cw = new CategoryWindow();
            cw.CallbackAction = async () => { await LoadData(); };
            cw.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Category c = ((FrameworkElement)sender).DataContext as Category;
            CategoryWindow cw = new CategoryWindow();
            cw.SetCategoryId(c.CategoryId);
            cw.CallbackAction = async () => { await LoadData(); };
            cw.Show();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Category c = ((FrameworkElement)sender).DataContext as Category;
            if(MessageBox.Show(string.Format("Are you sure to delete the following category?\r\n{0}", c.CategoryName), this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                await wrapper.Category.DeleteCategory(c.CategoryId);
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
            var result = await wrapper.Category.GetAllCategories();
            grdCategory.ItemsSource = result;

        }
    }
}
