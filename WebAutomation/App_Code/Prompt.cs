using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebAutomation.App_Code
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, string value, bool isPassword)
        {
            Form prompt = new Form();
            prompt.Width = 264;
            prompt.Height = 140;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 12, Top = 22, Text = text, Width = 210 };
            TextBox textBox = new TextBox() { Text = value, Left = 12, Top = 52, Width = 230 };
            if (isPassword)
            {
                textBox = new TextBox() { Text = value, Left = 12, Top = 52, Width = 230, PasswordChar = '*' };
            }
            else
            {
                textBox = new TextBox() { Text = value, Left = 12, Top = 52, Width = 230 };
            }

            textBox.Focus();
            textBox.KeyDown += (sender, e) =>
            {
                if (e.KeyValue != 13)
                    return;

                prompt.Close();
            };
            Button confirmation = new Button() { Text = "Ok", Left = 142, Width = 100, Top = 78 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}
