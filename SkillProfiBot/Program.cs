using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SkillProfiBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var token = "6802768084:AAF9obJT9AUYAubhcAomwUUBvn5ir5hQdhg";
            var bot = new TelegramBotClient(token: token);

            using var cts = new CancellationTokenSource();

            List<string> cloud = new List<string>();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            bot.StartReceiving(
               HandleUpdateAsync,
               HandleErrorAsync,
               receiverOptions,
               cancellationToken: cts.Token);

            var nameBot = bot.GetMeAsync().Result;
            Console.WriteLine($"Запущен бот {nameBot}");
            Console.ReadLine();

            cts.Cancel();


            #region Обработка ошибок
            Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
            {
                var Erormessage = exception switch
                {
                    ApiRequestException apiRequestException
                    => $"Ошибка telegram api: \n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };
                Console.WriteLine(Erormessage);
                return Task.CompletedTask;
            }
            #endregion

            #region Проверка типов обновлений полученных с телеграмма
            async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
            {
                string path = "note.csv";
                var message = update.Message;
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    await sw.WriteLineAsync(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                    sw.Flush();
                    sw.Close();
                }

                if (update.Type == UpdateType.Message && message?.Text != null)
                {
                    await HandleMessage(bot, message);
                    return;
                }
                else if (update.Message == null)
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Что-то пошло не так!");
                }
                else
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Что-то пошло не так!");
                }
                #endregion

                #region Метод для ошибок
                async void Eror(ITelegramBotClient bot, Message message, Exception ex)
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Что-то пошло не так!");
                    Console.WriteLine($"Ошибка {ex}");
                }
                #endregion

                #region Метод для текстовых сообщений
                async Task HandleMessage(ITelegramBotClient bot, Message message)
                {
                    string txt = message.Text;
                   
                    if (string.IsNullOrEmpty(txt))
                    {
                        await bot.SendTextMessageAsync(message.Chat, "Не понимаю пустые сообщения!");
                    }
                    else
                    {
                        var handler = new MyMessageHandler(bot, message);
                        await handler.DoSomething();
                    }
                }
                #endregion
            }
        }
    }
}