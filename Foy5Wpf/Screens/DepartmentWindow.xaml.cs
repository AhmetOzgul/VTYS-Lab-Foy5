using Foy5Wpf.Models;
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

namespace Foy5Wpf.Screens
{

    public partial class DepartmentWindow : Window
    {
        TextBox txtDepartmentName, txtFacultyId;
        Button btnAdd, btnUpdate, btnDelete;
        ListBox lstDepartments;
        OkulContext db = new OkulContext();
        int selectedId = -1;
        public DepartmentWindow()
        {
            InitializeComponent();
            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            root.Children.Add(new TextBlock
            {
                Text = "Bölüm İşlemleri",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            txtDepartmentName = CreateTextBox("Bölüm Adı");
            txtFacultyId = CreateTextBox("Fakülte ID");
            root.Children.Add(txtDepartmentName);
            root.Children.Add(txtFacultyId);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 10)
            };
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

            lstDepartments = new ListBox { Height = 180 };
            lstDepartments.SelectionChanged += lstDepartments_SelectionChanged;
            root.Children.Add(lstDepartments);

            LoadDepartments();
        }

        private TextBox CreateTextBox(string watermark)
        {
            var tb = new TextBox { Text = watermark, Margin = new Thickness(0, 0, 0, 5) };
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
            return tb;
        }

        private void LoadDepartments()
        {
            lstDepartments.ItemsSource = db.tBolumler
                .Select(b => $"{b.bolumID}: {b.bolumAd} (Fakülte:{b.fakulteID})")
                .ToList();
        }

        private void btnAdd_Click(object s, RoutedEventArgs e)
        {
            if (txtDepartmentName.Text == "Bölüm Adı" || txtFacultyId.Text == "Fakülte ID") return;
            db.tBolumler.Add(new tBolum
            {
                bolumAd = txtDepartmentName.Text,
                fakulteID = int.Parse(txtFacultyId.Text)
            });
            db.SaveChanges();
            LoadDepartments();
        }

        private void lstDepartments_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (lstDepartments.SelectedItem == null) return;
            selectedId = int.Parse(lstDepartments.SelectedItem.ToString().Split(':')[0]);
            var dept = db.tBolumler.Find(selectedId);
            txtDepartmentName.Text = dept.bolumAd;
            txtFacultyId.Text = dept.fakulteID.ToString();
        }

        private void btnUpdate_Click(object s, RoutedEventArgs e)
        {
            if (selectedId < 0) return;
            var dept = db.tBolumler.Find(selectedId);
            dept.bolumAd = txtDepartmentName.Text;
            dept.fakulteID = int.Parse(txtFacultyId.Text);
            db.SaveChanges();
            LoadDepartments();
        }

        private void btnDelete_Click(object s, RoutedEventArgs e)
        {
            if (selectedId < 0) return;
            var dept = db.tBolumler.Find(selectedId);
            db.tBolumler.Remove(dept);
            db.SaveChanges();
            selectedId = -1;
            LoadDepartments();
        }
    }
}