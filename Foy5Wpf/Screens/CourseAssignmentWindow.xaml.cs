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
    public partial class CourseAssignmentWindow : Window
    {
        TextBox txtStudentId, txtCourseId, txtYear, txtSemester;
        Button btnAssign, btnDeleteAssignment, btnUpdateAssignment;
        ListBox lstAssignments;
        OkulContext db = new OkulContext();
        int selectedAssignmentId = -1;
        public CourseAssignmentWindow()
        {
            InitializeComponent();
            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            txtStudentId = CreateTextBox("Öğrenci ID");
            txtCourseId = CreateTextBox("Ders ID");
            txtYear = CreateTextBox("Yıl");
            txtSemester = CreateTextBox("Yarıyıl");
            root.Children.Add(txtStudentId);
            root.Children.Add(txtCourseId);
            root.Children.Add(txtYear);
            root.Children.Add(txtSemester);

            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            btnAssign = new Button { Content = "Ata", Width = 80, Margin = new Thickness(0, 0, 5, 0) };
            btnDeleteAssignment = new Button { Content = "Sil", Width = 80, Margin = new Thickness(0, 0, 5, 0) };
            btnUpdateAssignment = new Button { Content = "Güncelle", Width = 80 };
            btnAssign.Click += btnAssign_Click;
            btnDeleteAssignment.Click += btnDeleteAssignment_Click;
            btnUpdateAssignment.Click += btnUpdateAssignment_Click;
            btnPanel.Children.Add(btnAssign);
            btnPanel.Children.Add(btnDeleteAssignment);
            btnPanel.Children.Add(btnUpdateAssignment);
            root.Children.Add(btnPanel);

            lstAssignments = new ListBox { Height = 200 };
            lstAssignments.SelectionChanged += lstAssignments_SelectionChanged;
            root.Children.Add(lstAssignments);

            LoadAssignments();
        }

        private TextBox CreateTextBox(string watermark)
        {
            var tb = new TextBox { Text = watermark, Margin = new Thickness(0, 0, 0, 5) };
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
            return tb;
        }

        private void LoadAssignments()
        {
            lstAssignments.ItemsSource = db.tOgrenciDersler
                .Select(x => $"{x.ID}: ÖğrID={x.ogrenciID}, DersID={x.dersID}, {x.yil}-{x.yariyil}")
                .ToList();
        }

        private void btnAssign_Click(object s, RoutedEventArgs e)
        {
            if (txtStudentId.Text == "Öğrenci ID" || txtCourseId.Text == "Ders ID") return;
            db.tOgrenciDersler.Add(new tOgrenciDers
            {
                ogrenciID = int.Parse(txtStudentId.Text),
                dersID = int.Parse(txtCourseId.Text),
                yil = int.Parse(txtYear.Text),
                yariyil = txtSemester.Text
            });
            db.SaveChanges();
            LoadAssignments();
        }

        private void lstAssignments_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (lstAssignments.SelectedItem == null) return;
            selectedAssignmentId = int.Parse(lstAssignments.SelectedItem.ToString().Split(':')[0]);
            var a = db.tOgrenciDersler.Find(selectedAssignmentId);
            txtStudentId.Text = a.ogrenciID.ToString();
            txtCourseId.Text = a.dersID.ToString();
            txtYear.Text = a.yil.ToString();
            txtSemester.Text = a.yariyil;
        }

        private void btnDeleteAssignment_Click(object s, RoutedEventArgs e)
        {
            if (selectedAssignmentId < 0) return;
            var a = db.tOgrenciDersler.Find(selectedAssignmentId);
            db.tOgrenciDersler.Remove(a);
            db.SaveChanges();
            selectedAssignmentId = -1;
            LoadAssignments();
        }

        private void btnUpdateAssignment_Click(object s, RoutedEventArgs e)
        {
            if (selectedAssignmentId < 0) return;
            var a = db.tOgrenciDersler.Find(selectedAssignmentId);
            a.ogrenciID = int.Parse(txtStudentId.Text);
            a.dersID = int.Parse(txtCourseId.Text);
            a.yil = int.Parse(txtYear.Text);
            a.yariyil = txtSemester.Text;
            db.SaveChanges();
            LoadAssignments();
        }
    }
}