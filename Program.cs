using System;

namespace NotificationGateway
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Мультиканальный шлюз уведомлений и тревог ===\n");

            // === Шаг 5.1. Полиморфный массив ===
            NotificationChannel[] channels = new NotificationChannel[3];
            channels[0] = new EmailChannel("Email SMTP", smtpAvailable: true);
            channels[1] = new TelegramChannel("Telegram Bot", botToken: "12345:ABC");
            channels[2] = new SirenAlarmChannel(soundRelayLevel: 90);

            // === Шаг 5.2. Демонстрация позднего связывания (override) ===
            Console.WriteLine("--- Демонстрация полиморфного вызова Deliver (override) ---\n");

            string testMsg = "Внимание: регламентные технические работы в серверной!";

            channels[0].Deliver("admin@company.ru", testMsg);      // Email: валидный адрес
            channels[1].Deliver("@sys_admin", testMsg);            // Telegram: валидный юзернейм
            channels[2].Deliver("ANY", testMsg);                   // Сирена: получатель игнорируется

            // Демонстрация неудачных отправок (валидация)
            Console.WriteLine("--- Проверка валидации ---\n");
            channels[0].Deliver("invalid-address", "тест");        // нет @
            channels[1].Deliver("sys_admin", "тест");              // нет @
            channels[1].Deliver("@user", new string('X', 150));    // > 100 символов

            // === Шаг 5.3. Разница между override и new ===
            Console.WriteLine("\n--- Демонстрация override vs new ---\n");

            SirenAlarmChannel sirenDirect = (SirenAlarmChannel)channels[2];
            NotificationChannel sirenAsBase = channels[2];

            // Через ссылку базового типа — сработает ВИРТУАЛЬНЫЙ метод базового класса
            Console.WriteLine("Через ссылку базового типа NotificationChannel:");
            Console.WriteLine("  " + sirenAsBase.FormatPayload("тест сокрытия"));

            // Через прямую ссылку SirenAlarmChannel — сработает метод, скрытый через new
            Console.WriteLine("\nЧерез прямую ссылку SirenAlarmChannel:");
            Console.WriteLine("  " + sirenDirect.FormatPayload("тест сокрытия"));

            // === Шаг 5.4. Полиморфный аудит ===
            Console.WriteLine("\n--- Итоговый аудит по каналам ---\n");
            foreach (var channel in channels)
            {
                channel.PrintAudit();
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}