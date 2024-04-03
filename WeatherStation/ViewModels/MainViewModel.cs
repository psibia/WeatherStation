using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherStation.Services;
using WeatherStation.Utilities;

namespace WeatherStation.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ArduinoService _arduinoService;
        private readonly TelegramBotService _telegramBotService;

        private int _temperature;
        public int Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private int _humidity;
        public int Humidity
        {
            get => _humidity;
            set
            {
                _humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        public ICommand SendTemperatureCommand { get; }

        public MainViewModel()
        {
            // Инициализация сервисов
            _arduinoService = new ArduinoService("COM4");
            _telegramBotService = new TelegramBotService("6595562772:AAF2T0L8ULoWm1u8HNJMHPBBl3TtKgjyaRs", 897988198);

            // Подписка на события
            _arduinoService.OnDataReceived += ArduinoService_OnDataReceived;
            _telegramBotService.OnMessageReceived += TelegramBotService_OnMessageReceived;

            // Запуск сервисов
            _telegramBotService.Start();

            SendTemperatureCommand = new RelayCommand(SendTemperature, CanSendTemperature);
        }

        private bool CanSendTemperature(object parameter)
        {
            // Проверка, что есть данные для отправки
            return Temperature > 0 && Humidity > 0;
        }

        private async void SendTemperature(object parameter)
        {
            string message = $"Температура: {Temperature}, Влажность: {Humidity}";
            await _telegramBotService.SendMessageAsync(message);
        }

        private void ArduinoService_OnDataReceived(object sender, ArduinoService.DataReceivedEventArgs e)
        {
            // Обработка данных с Arduino
            // Предполагаем, что данные приходят в формате "t:h"
            var data = e.Data.Split(':');
            if (data.Length == 2)
            {
                if (int.TryParse(data[0], out int temp) && int.TryParse(data[1], out int hum))
                {
                    Temperature = temp;
                    Humidity = hum;
                }
            }
        }

        private void TelegramBotService_OnMessageReceived(object sender, TelegramBotService.MessageReceivedEventArgs e)
        {
            // Обработка команд от Telegram
            if (e.Message == "/temperature")
            {
                SendTemperature(null);
            }
        }
    }
}
