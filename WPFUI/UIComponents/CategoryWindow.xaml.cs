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
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        private int _CategoryId = -1;
        public Action CallbackAction  { get; set;}

        public CategoryWindow()
        {
            this.SourceInitialized += (object sender, EventArgs e) =>
            {
                MainWindow.Window_SourceInitialized(sender, e);
            };

            DataContext = new Category();

            InitializeComponent();
        }
        public async void SetCategoryId(int categoryId)
        {
            _CategoryId = categoryId;
            if (_CategoryId > 0)
            {
                ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                var result = await wrapper.Category.GetCategoryById(_CategoryId);
                DataContext = result;
            }
        }
        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category c = DataContext as Category;
                if (c != null)
                {
                    if (string.IsNullOrEmpty(c.CategoryName))
                    {
                        MessageBox.Show("The category name cannot be empty!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (_CategoryId > 0)
                    {
                        ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                        await wrapper.Category.UpdateCategory(c);
                    }
                    else
                    {
                        ProductHttpClientWrapper wrapper = new ProductHttpClientWrapper();
                        await wrapper.Category.CreateCategory(c);
                    }
                    Close();
                    if (CallbackAction != null)
                        CallbackAction();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Failed to save the category!\r\nError Message:\r\n{0}", ex) , this.Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
