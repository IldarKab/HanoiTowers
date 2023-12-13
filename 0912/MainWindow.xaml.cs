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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _0912
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stack<(StackPanel, StackPanel, StackPanel, int)> moveStack;
        private int moveCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            moveStack = new Stack<(StackPanel, StackPanel, StackPanel, int)>();
        }

        private void ShowRingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем стержни с прошлых попыток
            StackA.Children.Clear();
            StackB.Children.Clear();
            StackC.Children.Clear();
            moveCount = 0;

            // Получаем количество колец от пользователя
            if (int.TryParse(txtNumberOfRings.Text, out int numberOfRings) && numberOfRings > 0)
            {
                // Создаем и добавляем кольца на первый стек (StackA)
                for (int i = numberOfRings; i > 0; i--)
                {
                    Label ring = new Label
                    {
                        Content = i.ToString(),
                        Style = (Style)FindResource("RingStyle"),
                        Width = 145 - i * 10
                    };
                    StackA.Children.Add(ring);
                }
                MoveRingsButton.IsEnabled = true;


            }



            else
            {
                MessageBox.Show("Пожалуйста, введите положительное целое число.");
            }

        }


        private StackPanel GetSelectedStack(ComboBox comboBox)
        {
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string selectedStack = selectedItem.Content.ToString();

                switch (selectedStack)
                {
                    case "Stack A":
                        return StackA;
                    case "Stack B":
                        return StackB;
                    case "Stack C":
                        return StackC;
                    default:
                        return null;
                }
            }

            return null;
        }

        private void MoveRingsButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sourceStack = StackA; // Предполагаем, что кольца находятся на стержне A
            StackPanel targetStack = GetSelectedStack(cmbTargetStack);
            StackPanel auxiliaryStack = (targetStack == StackB) ? StackC : StackB;

            if (targetStack != null)
            {
                // Переместить кольца с исходного стержня на выбранный стержень, используя второй стержень как вспомогательный
                MoveTower(sourceStack, targetStack, auxiliaryStack, sourceStack.Children.Count);
            }

            MessageBox.Show($"Выполнено перемещений: {moveCount}");
        }

        private void MoveTower(StackPanel source, StackPanel target, StackPanel auxiliary, int n)
        {
            if (n > 0)
            {
                MoveTower(source, auxiliary, target, n - 1);
                MoveDisk(source, target);
                MoveTower(auxiliary, target, source, n - 1);
            }
        }

        private void MoveDisk(StackPanel source, StackPanel target)
        {
            if (source.Children.Count > 0)
            {
                UIElement disk = source.Children[source.Children.Count - 1];
                source.Children.Remove(disk);
                target.Children.Add(disk);

                // Увеличить счетчик перемещений
                moveCount++;

                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}