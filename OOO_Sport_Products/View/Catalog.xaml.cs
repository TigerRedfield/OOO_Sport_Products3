using OOO_Sport_Products.Classes;
using OOO_Sport_Products.Model;
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
using System.Windows.Shapes;

namespace OOO_Sport_Products.View
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public Catalog()
        {
            InitializeComponent();
        }

        private void buttonNavigation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmAddInOrder.Visibility = Visibility.Hidden;
            butViewOrder.Visibility = Visibility.Hidden;
            if (Helper.user == null || Helper.user.UserRole == 1)
            {
                cmAddInOrder.Visibility = Visibility.Visible;
            }

            // Получили все данные о товарах
            List<Model.Category> categories = new List<Model.Category>();

            Model.Category category = new Model.Category();
            category.CategoryID = 0;
            category.CategoryName = "Все категории";
            categories.Add(category);
            categories.AddRange(Helper.DB.Category.ToList());

            cbFilterCateg.DisplayMemberPath = "CategoryName";
            cbFilterCateg.SelectedValuePath = "CategoryID";
            cbFilterCateg.ItemsSource = categories;

            cbFilter.SelectedIndex = 0;
            cbFilterDisc.SelectedIndex = 0;
            cbFilterCateg.SelectedIndex = 0;

            ShowProducts();


             
            //List<Model.Product> products = new List<Model.Product>();
            //products = Helper.DB.Product.ToList();
            //List<Classes.ProductExtended> productExtendeds = new List<Classes.ProductExtended>();
            //foreach(var item in products)
            //{
            //    Classes.ProductExtended productExtended = new Classes.ProductExtended();
            //    productExtended.Product = item;
            //    productExtendeds.Add(productExtended);
            //}
            //ListBoxProduct.ItemsSource = productExtendeds;

            //foreach(var item in products)
            //{
            //    Classes.ProductExtended productExtended = new Classes.ProductExtended();
            //    productExtended.ProductCost = item.ProductCost;
            //    productExtended.ProductDiscount = item.ProductDiscount;

            //    double discont = (double)item.ProductCost * (double)item.ProductDiscount/100;
            //    productExtended.ProductCostWithDiscount = (double)item.ProductCost - discont;
            //    productExtendeds.Add(productExtended);
            //}

            //ListBoxProduct.ItemsSource = productExtendeds;
            //List<Model.Product> products = new List<Model.Product>();
            //products = Helper.DB.Product.ToList();
            //ListBoxProduct.ItemsSource = products;
        }

        private void ShowProducts()
        {
            List<ProductExtended> productExtendeds =
                Helper.DB.Product.ToList().ConvertAll<ProductExtended>(p => new ProductExtended(p));
           
            int totalcount = productExtendeds.Count;    

            //Сортировка по цене
            switch(cbFilter.SelectedIndex)
            {
                case 0:
                    productExtendeds = productExtendeds.OrderBy(pr => pr.Product.ProductCost).ToList();
                    break;
                case 1:
                    productExtendeds = productExtendeds.OrderByDescending(pr => pr.Product.ProductCost).ToList();
                    break;
            }

            int min = 0, max = 100;

            //фильтр по скидке
            if(cbFilterDisc.SelectedIndex > 0)
            {
                switch(cbFilterDisc.SelectedIndex)
                {
                    case 1:
                        min = 0; max = 9;
                        break;
                    case 2:
                        min = 10; max = 14;
                        break;
                    case 3:
                        min = 15; max = 100;
                        break;
                }
                productExtendeds = productExtendeds.Where(pr => pr.Product.ProductDiscountMax >= min && pr.Product.ProductDiscountMax <= max).ToList();
            }

            if(cbFilterCateg.SelectedIndex > 0) {
                productExtendeds = productExtendeds.Where(pr => pr.Product.ProductCategory == (int)cbFilterCateg.SelectedValue).ToList();    
            }


            //поиск по названию товаров

            string search = tbSearch.Text;
            if (!string.IsNullOrEmpty(search))
            {
                productExtendeds = productExtendeds.Where(pr => pr.Product.ProductName.ToLower().Contains(search.ToLower())).ToList();
            }


            //string search = tbSearch.Text;
            //if(!string.IsNullOrEmpty(search))
            //{
            //    productExtendeds = productExtendeds.Where(pr => pr.Product.ProductName.Contains(search)).ToList();
            //}

            int filterCount = productExtendeds.Count;
            tbcount.Text = "Количество " + filterCount.ToString() + " из " + totalcount.ToString();
            ListBoxProduct.ItemsSource = productExtendeds;
        }

        /// <summary>
        /// Выбор направления сортировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProducts();
        }

        /// <summary>
        /// Фильтрация по скидке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFilterDisc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProducts();
        }

        /// <summary>
        /// Фильтрация по категориям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFilterCateg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProducts();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowProducts();
        }

        private void miAddInOrder_Click(object sender, RoutedEventArgs e)
        {
            butViewOrder.Visibility = Visibility.Visible;
        }
    }
}
