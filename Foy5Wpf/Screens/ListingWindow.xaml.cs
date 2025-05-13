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
    public partial class ListingWindow : Window
    {
        TextBox txtStudentId, txtYear, txtSemester;
        Button btnGetStudentCourses, btnGetCourseCounts;
        DataGrid dgResults;
        OkulContext db = new OkulContext();
        public ListingWindow()
        {
            InitializeComponent();

            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            // Title
            root.Children.Add(new TextBlock
            {
                Text = "Listeleme",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // Student-based
            var panel1 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            txtStudentId = new TextBox { Width = 100, Text = "Öğrenci ID", Margin = new Thickness(0, 0, 5, 0) };
            AddPlaceholder(txtStudentId, "Öğrenci ID");
            btnGetStudentCourses = new Button { Content = "Getir", Width = 75 };
            btnGetStudentCourses.Click += btnGetStudentCourses_Click;
            panel1.Children.Add(txtStudentId);
            panel1.Children.Add(btnGetStudentCourses);
            root.Children.Add(panel1);

            // Term-based
            var panel2 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            txtYear = new TextBox { Width = 60, Text = "Yıl", Margin = new Thickness(0, 0, 5, 0) };
            AddPlaceholder(txtYear, "Yıl");
            txtSemester = new TextBox { Width = 60, Text = "Yarıyıl", Margin = new Thickness(0, 0, 0, 5) };
            AddPlaceholder(txtSemester, "Yarıyıl");
            btnGetCourseCounts = new Button { Content = "Getir", Width = 75 };
            btnGetCourseCounts.Click += btnGetCourseCounts_Click;
            panel2.Children.Add(txtYear);
            panel2.Children.Add(txtSemester);
            panel2.Children.Add(btnGetCourseCounts);
            root.Children.Add(panel2);

            dgResults = new DataGrid
            {
                AutoGenerateColumns = true,
                IsReadOnly = true,
                Height = 250
            };
            root.Children.Add(dgResults);
        }

        private void AddPlaceholder(TextBox tb, string watermark)
        {
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
        }

        private void btnGetStudentCourses_Click(object s, RoutedEventArgs e)
        {
            if (!int.TryParse(txtStudentId.Text, out int id)) return;
            var list = db.tOgrenciDersler
                .Where(od => od.ogrenciID == id)
                .Join(db.tDersler,
                      od => od.dersID,
                      d => d.dersID,
                      (od, d) => new { d.dersAd, od.yil, od.yariyil })
                .ToList();
            dgResults.ItemsSource = list;
        }

        private void btnGetCourseCounts_Click(object s, RoutedEventArgs e)
        {
            if (!int.TryParse(txtYear.Text, out int year)) return;
            var sem = txtSemester.Text;
            var counts = db.tOgrenciDersler
                .Where(od => od.yil == year && od.yariyil == sem)
                .GroupBy(od => od.dersID)
                .Select(g => new { CourseId = g.Key, Count = g.Count() })
                .Join(db.tDersler,
                      grp => grp.CourseId,
                      d => d.dersID,
                      (grp, d) => new { d.dersAd, grp.Count })
                .ToList();
            dgResults.ItemsSource = counts;
        }
    }
}