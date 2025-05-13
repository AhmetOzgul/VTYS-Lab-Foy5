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
using Foy5Wpf.Models;

namespace Foy5Wpf.Screens
{
    public partial class CourseWindow : Window
    {
        TextBox txtCourseName;
        Button btnAdd, btnUpdate, btnDelete;
        ListBox lstCourses;
        OkulContext db = new OkulContext();
        int selectedId = -1;

        public CourseWindow()
        {
            InitializeComponent();
            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            root.Children.Add(new TextBlock
            {
                Text = "Ders İşlemleri",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            txtCourseName = CreateTextBox("Ders Adı");
            root.Children.Add(txtCourseName);

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            btnAdd = new Button { Content = "Ekle", Width = 80, Margin = new Thickness(0, 0, 5, 0) };
            btnUpdate = new Button { Content = "Güncelle", Width = 80, Margin = new Thickness(0, 0, 5, 0) };
            btnDelete = new Button { Content = "Sil", Width = 80 };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            btnPanel.Children.Add(btnAdd);
            btnPanel.Children.Add(btnUpdate);
            btnPanel.Children.Add(btnDelete);
            root.Children.Add(btnPanel);

            lstCourses = new ListBox { Height = 180 };
            lstCourses.SelectionChanged += lstCourses_SelectionChanged;
            root.Children.Add(lstCourses);

            LoadCourses();
        }

        private TextBox CreateTextBox(string watermark)
        {
            var tb = new TextBox { Text = watermark, Margin = new Thickness(0, 0, 0, 5) };
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
            return tb;
        }

        private void LoadCourses()
        {
            lstCourses.ItemsSource = db.tDersler
                .Select(d => $"{d.dersID}: {d.dersAd}")
                .ToList();
        }

        private void btnAdd_Click(object s, RoutedEventArgs e)
        {
            if (txtCourseName.Text == "Ders Adı") return;
            db.tDersler.Add(new tDers { dersAd = txtCourseName.Text });
            db.SaveChanges();
            LoadCourses();
        }

        private void lstCourses_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (lstCourses.SelectedItem == null) return;
            selectedId = int.Parse(lstCourses.SelectedItem.ToString().Split(':')[0]);
            txtCourseName.Text = db.tDersler.Find(selectedId).dersAd;
        }

        private void btnUpdate_Click(object s, RoutedEventArgs e)
        {
            if (selectedId < 0) return;
            var c = db.tDersler.Find(selectedId);
            c.dersAd = txtCourseName.Text;
            db.SaveChanges();
            LoadCourses();
        }

        private void btnDelete_Click(object s, RoutedEventArgs e)
        {
            if (selectedId < 0) return;
            var c = db.tDersler.Find(selectedId);
            db.tDersler.Remove(c);
            db.SaveChanges();
            selectedId = -1;
            LoadCourses();
        }
    }
}