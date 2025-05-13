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
    public partial class StudentWindow : Window
    {
        TextBox txtFirstName, txtLastName, txtDepartmentId;
        Button btnAdd, btnUpdate, btnDelete;
        ListBox lstStudents;
        OkulContext db = new OkulContext();
        int selectedStudentId = -1;

        public StudentWindow()
        {
            InitializeComponent();
            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            root.Children.Add(new TextBlock
            {
                Text = "Öğrenci İşlemleri",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            txtFirstName = CreateTextBox("Ad");
            txtLastName = CreateTextBox("Soyad");
            txtDepartmentId = CreateTextBox("Bölüm ID");
            root.Children.Add(txtFirstName);
            root.Children.Add(txtLastName);
            root.Children.Add(txtDepartmentId);

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

            lstStudents = new ListBox { Height = 200 };
            lstStudents.SelectionChanged += lstStudents_SelectionChanged;
            root.Children.Add(lstStudents);

            LoadStudents();
        }

        private TextBox CreateTextBox(string watermark)
        {
            var tb = new TextBox { Text = watermark, Margin = new Thickness(0, 0, 0, 5) };
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
            return tb;
        }

        private void LoadStudents()
        {
            lstStudents.ItemsSource = db.tOgrenciler
                .Select(o => $"{o.ogrenciID}: {o.ad} {o.soyad} (Bölüm:{o.bolumID})")
                .ToList();
        }

        private void btnAdd_Click(object s, RoutedEventArgs e)
        {
            if (txtFirstName.Text == "Ad" || txtLastName.Text == "Soyad" || txtDepartmentId.Text == "Bölüm ID")
                return;
            db.tOgrenciler.Add(new tOgrenci
            {
                ad = txtFirstName.Text,
                soyad = txtLastName.Text,
                bolumID = int.Parse(txtDepartmentId.Text)
            });
            db.SaveChanges();
            LoadStudents();
        }

        private void lstStudents_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (lstStudents.SelectedItem == null) return;
            selectedStudentId = int.Parse(lstStudents.SelectedItem.ToString().Split(':')[0]);
            var o = db.tOgrenciler.Find(selectedStudentId);
            txtFirstName.Text = o.ad;
            txtLastName.Text = o.soyad;
            txtDepartmentId.Text = o.bolumID.ToString();
        }

        private void btnUpdate_Click(object s, RoutedEventArgs e)
        {
            if (selectedStudentId < 0) return;
            var o = db.tOgrenciler.Find(selectedStudentId);
            o.ad = txtFirstName.Text;
            o.soyad = txtLastName.Text;
            o.bolumID = int.Parse(txtDepartmentId.Text);
            db.SaveChanges();
            LoadStudents();
        }

        private void btnDelete_Click(object s, RoutedEventArgs e)
        {
            if (selectedStudentId < 0) return;
            var o = db.tOgrenciler.Find(selectedStudentId);
            db.tOgrenciler.Remove(o);
            db.SaveChanges();
            selectedStudentId = -1;
            LoadStudents();
        }
    }
}