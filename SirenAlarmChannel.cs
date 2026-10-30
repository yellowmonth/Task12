using System;

namespace NotificationGateway
{
    public class SirenAlarmChannel : NotificationChannel
    {
        private readonly int _soundRelayLevel;

        public override bool IsOnline => _soundRelayLevel > 50;

        public SirenAlarmChannel(int soundRelayLevel = 80) : base("Звуковая Сирена")
        {
            _soundRelayLevel = soundRelayLevel;
        }

        // СОКРЫТИЕ (new) базового виртуального метода
        public new string FormatPayload(string rawMessage)
        {
            return "[ТРЕВОГА МЧС]: " + rawMessage.ToUpper();
        }

        public override bool Deliver(string recipient, string message)
        {
            if (!IsOnline)
            {
                Console.WriteLine($"[{ChannelName}] Уровень реле ниже порога ({_soundRelayLevel}). Сирена не сработала.");
                return false;
            }

            DeliveredCount++;
            Console.WriteLine($"[{ChannelName}] !!! ТРЕВОГА !!!");
            Console.WriteLine(">>> \a ВНИМАНИЕ! СИРЕНА АКТИВИРОВАНА \a <<<");
            Console.WriteLine(message.ToUpper());
            Console.WriteLine();
            return true;
        }
    }
}