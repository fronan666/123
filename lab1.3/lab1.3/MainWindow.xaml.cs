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

namespace lab1._3
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private WeatherData _weatherData;

        public MainWindow()
        {
            InitializeComponent();
            _weatherData = new WeatherData();
            InitializeTimer();
            UpdateWeatherDisplay();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            DateTime now = DateTime.Now;
            string yesterdayTemp = _weatherData.YesterdayTemperature.ToString();
            TimeText.Text = $"Сейчас {now:HH:mm}. Вчера в это время {yesterdayTemp}°";
        }

        private void UpdateWeatherDisplay()
        {
            LocationText.Text = _weatherData.Location;
            TemperatureText.Text = _weatherData.Temperature.ToString();
            WindText.Text = $"{_weatherData.WindSpeed} м/с, {_weatherData.WindDirection}";
            HumidityText.Text = $"{_weatherData.Humidity}%";
            PressureText.Text = $"{_weatherData.Pressure} мм рт. ст.";
            UpdateTime();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new Window
            {
                Title = "Настройки погоды",
                Width = 300,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this
            };

            var grid = new Grid { Margin = new Thickness(10) };

            for (int i = 0; i < 9; i++)
                grid.RowDefinitions.Add(new RowDefinition() { Height = i == 8 ? new GridLength(1, GridUnitType.Star) : GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            var locationLabel = new TextBlock { Text = "Местоположение:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var locationTextBox = new TextBox { Text = _weatherData.Location, Margin = new Thickness(0, 0, 0, 5) };

            var tempLabel = new TextBlock { Text = "Температура:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var tempTextBox = new TextBox { Text = _weatherData.Temperature.ToString(), Margin = new Thickness(0, 0, 0, 5) };

            var windSpeedLabel = new TextBlock { Text = "Скорость ветра:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var windSpeedTextBox = new TextBox { Text = _weatherData.WindSpeed.ToString(), Margin = new Thickness(0, 0, 0, 5) };

            var windDirLabel = new TextBlock { Text = "Направление ветра:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var windDirComboBox = new ComboBox { Margin = new Thickness(0, 0, 0, 5), SelectedIndex = 0 };
            windDirComboBox.Items.Add("С");
            windDirComboBox.Items.Add("Ю");
            windDirComboBox.Items.Add("З");
            windDirComboBox.Items.Add("В");
            windDirComboBox.Items.Add("СЗ");
            windDirComboBox.Items.Add("СВ");
            windDirComboBox.Items.Add("ЮЗ");
            windDirComboBox.Items.Add("ЮВ");

            for (int i = 0; i < windDirComboBox.Items.Count; i++)
            {
                if (windDirComboBox.Items[i].ToString() == _weatherData.WindDirection)
                {
                    windDirComboBox.SelectedIndex = i;
                    break;
                }
            }

            var humidityLabel = new TextBlock { Text = "Влажность:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var humidityTextBox = new TextBox { Text = _weatherData.Humidity.ToString(), Margin = new Thickness(0, 0, 0, 5) };

            var pressureLabel = new TextBlock { Text = "Давление:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var pressureTextBox = new TextBox { Text = _weatherData.Pressure.ToString(), Margin = new Thickness(0, 0, 0, 5) };

            var yesterdayTempLabel = new TextBlock { Text = "Вчерашняя темп.:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 5) };
            var yesterdayTempTextBox = new TextBox { Text = _weatherData.YesterdayTemperature.ToString(), Margin = new Thickness(0, 0, 0, 5) };

            var saveButton = new Button { Content = "Сохранить", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
            var cancelButton = new Button { Content = "Отмена", Width = 80 };

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            buttonPanel.Children.Add(saveButton);
            buttonPanel.Children.Add(cancelButton);

            Grid.SetRow(locationLabel, 0); Grid.SetColumn(locationLabel, 0);
            Grid.SetRow(locationTextBox, 0); Grid.SetColumn(locationTextBox, 1);

            Grid.SetRow(tempLabel, 1); Grid.SetColumn(tempLabel, 0);
            Grid.SetRow(tempTextBox, 1); Grid.SetColumn(tempTextBox, 1);

            Grid.SetRow(windSpeedLabel, 2); Grid.SetColumn(windSpeedLabel, 0);
            Grid.SetRow(windSpeedTextBox, 2); Grid.SetColumn(windSpeedTextBox, 1);

            Grid.SetRow(windDirLabel, 3); Grid.SetColumn(windDirLabel, 0);
            Grid.SetRow(windDirComboBox, 3); Grid.SetColumn(windDirComboBox, 1);

            Grid.SetRow(humidityLabel, 4); Grid.SetColumn(humidityLabel, 0);
            Grid.SetRow(humidityTextBox, 4); Grid.SetColumn(humidityTextBox, 1);

            Grid.SetRow(pressureLabel, 5); Grid.SetColumn(pressureLabel, 0);
            Grid.SetRow(pressureTextBox, 5); Grid.SetColumn(pressureTextBox, 1);

            Grid.SetRow(yesterdayTempLabel, 6); Grid.SetColumn(yesterdayTempLabel, 0);
            Grid.SetRow(yesterdayTempTextBox, 6); Grid.SetColumn(yesterdayTempTextBox, 1);

            Grid.SetRow(buttonPanel, 8); Grid.SetColumnSpan(buttonPanel, 2);

            grid.Children.Add(locationLabel);
            grid.Children.Add(locationTextBox);
            grid.Children.Add(tempLabel);
            grid.Children.Add(tempTextBox);
            grid.Children.Add(windSpeedLabel);
            grid.Children.Add(windSpeedTextBox);
            grid.Children.Add(windDirLabel);
            grid.Children.Add(windDirComboBox);
            grid.Children.Add(humidityLabel);
            grid.Children.Add(humidityTextBox);
            grid.Children.Add(pressureLabel);
            grid.Children.Add(pressureTextBox);
            grid.Children.Add(yesterdayTempLabel);
            grid.Children.Add(yesterdayTempTextBox);
            grid.Children.Add(buttonPanel);

            settingsWindow.Content = grid;

            saveButton.Click += (s, e2) =>
            {
                if (ValidateInput(locationTextBox.Text, tempTextBox.Text, windSpeedTextBox.Text,
                    humidityTextBox.Text, pressureTextBox.Text, yesterdayTempTextBox.Text))
                {
                    _weatherData.Location = locationTextBox.Text;
                    _weatherData.Temperature = int.Parse(tempTextBox.Text);
                    _weatherData.WindSpeed = double.Parse(windSpeedTextBox.Text);
                    _weatherData.WindDirection = windDirComboBox.SelectedItem?.ToString() ?? "Ю";
                    _weatherData.Humidity = int.Parse(humidityTextBox.Text);
                    _weatherData.Pressure = int.Parse(pressureTextBox.Text);
                    _weatherData.YesterdayTemperature = int.Parse(yesterdayTempTextBox.Text);

                    UpdateWeatherDisplay();
                    settingsWindow.Close();
                }
            };

            cancelButton.Click += (s, e2) =>
            {
                settingsWindow.Close();
            };

            settingsWindow.ShowDialog();
        }

        private bool ValidateInput(string location, string temp, string windSpeed, string humidity, string pressure, string yesterdayTemp)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Введите местоположение");
                return false;
            }
            if (!int.TryParse(temp, out _))
            {
                MessageBox.Show("Температура должна быть числом");
                return false;
            }
            if (!double.TryParse(windSpeed, out _))
            {
                MessageBox.Show("Скорость ветра должна быть числом");
                return false;
            }
            if (!int.TryParse(humidity, out _))
            {
                MessageBox.Show("Влажность должна быть числом");
                return false;
            }
            if (!int.TryParse(pressure, out _))
            {
                MessageBox.Show("Давление должно быть числом");
                return false;
            }
            if (!int.TryParse(yesterdayTemp, out _))
            {
                MessageBox.Show("Вчерашняя температура должна быть числом");
                return false;
            }
            return true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class WeatherData
    {
        public string Location { get; set; } = "Район, Новосибирск";
        public int Temperature { get; set; } = -16;
        public double WindSpeed { get; set; } = 1.0;
        public string WindDirection { get; set; } = "Ю";
        public int Humidity { get; set; } = 95;
        public int Pressure { get; set; } = 766;
        public int YesterdayTemperature { get; set; } = -25;
    }
}
