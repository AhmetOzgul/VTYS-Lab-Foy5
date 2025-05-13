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
using Microsoft.EntityFrameworkCore;
using Foy5Wpf.Models;

namespace Foy5Wpf.Screens

{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var panel = new StackPanel
            {
                Margin = new Thickness(20),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            this.Content = panel;

            panel.Children.Add(CreateButton("Fakülte İşlemleri", (s, e) => new FacultyWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Bölüm İşlemleri", (s, e) => new DepartmentWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Ders İşlemleri", (s, e) => new CourseWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Öğrenci İşlemleri", (s, e) => new StudentWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Ders Atama", (s, e) => new CourseAssignmentWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Öğrenci Sorgula", (s, e) => new ListingWindow().ShowDialog()));
            panel.Children.Add(CreateButton("Not Girişi", (s, e) => new GradeEntryWindow().ShowDialog()));
        }

        private Button CreateButton(string text, RoutedEventHandler handler)
        {
            var btn = new Button
            {
                Content = text,
                Width = 200,
                Height = 35,
                Margin = new Thickness(0, 5, 0, 5)
            };
            btn.Click += handler;
            return btn;
        }
    }
}
