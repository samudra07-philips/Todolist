using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace todolist_mvvm.Bussiness_Layer.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        public static void SetBindPassword(DependencyObject dp, bool value)
            => dp.SetValue(BindPasswordProperty, value);

        public static bool GetBindPassword(DependencyObject dp)
            => (bool)dp.GetValue(BindPasswordProperty);

        public static string GetBoundPassword(DependencyObject dp)
            => (string)dp.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject dp, string value)
            => dp.SetValue(BoundPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
               
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (!(bool)passwordBox.GetValue(UpdatingPasswordProperty))
                {
                    passwordBox.Password = (string)e.NewValue;
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
                bool wasBound = (bool)(e.OldValue);
                bool needToBind = (bool)(e.NewValue);
                if (wasBound)
                {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }
                if (needToBind)
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.SetValue(UpdatingPasswordProperty, true);
                SetBoundPassword(passwordBox, passwordBox.Password);
                passwordBox.SetValue(UpdatingPasswordProperty, false);
            }
        }
    }
}
