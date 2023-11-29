using Newtonsoft.Json;
using SkillProfiClasses.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace SkillProfiBot
{
    public class MyMessageHandler
    {
        private ITelegramBotClient Bot;
        private Message TelegramMessage;
        ReplyKeyboardMarkup KeyboardMenuBack = new(new[]
                    {
                        new KeyboardButton[] { "Назад"},
                    })
        {
            ResizeKeyboard = true
        };
        ReplyKeyboardMarkup KeyboardMenuCreate = new(new[]
                {
                        new KeyboardButton[] { "Создать заявку"},
                    })
        {
            ResizeKeyboard = true
        };

        public MyMessageHandler(ITelegramBotClient bot, Message message)
        {
            Bot = bot;
            TelegramMessage = message;
        }

        public async Task DoSomething()
        {
           

            var id = TelegramMessage.Chat.Id;
            var workerWithFile = new WorkWithFile(id);
            if (TelegramMessage.Text.ToLower() == "/start")
            {
                await Hello(); 
                await workerWithFile.SetPosition(0, false);
            }

            int position = await workerWithFile.GetPosition();

            if (TelegramMessage.Text.ToLower() == "назад")
            {
                if (position == 0)
                {
                    position = -2;
                }
                else
                {
                    position = position - 2;
                }
            }

            switch (position)
            {
                case -2:
                    await Hello();
                    await workerWithFile.SetPosition(0, false);
                    break;
                case -1:
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "К сожалению время создания заявки прошло! Попробуйте еще раз!");
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Напишите ваше имя!", replyMarkup: KeyboardMenuBack);
                    break;
                case 0:
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Напишите ваше имя!", replyMarkup: KeyboardMenuBack);
                    await workerWithFile.SetPosition(1, true);
                    break;
                case 1:
                    await workerWithFile.SetPosition(2, (new DataInFile
                    {
                        Date = DateTime.Now,
                        Name = TelegramMessage.Text,
                        Email = string.Empty,
                        Text = string.Empty
                    }));
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Напишите ваш email!", replyMarkup: KeyboardMenuBack);
                    break;
                case 2:
                    if (!await IsValidEmail(TelegramMessage.Text))
                    {
                        await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Некорректный email!");
                        await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Напишите ваш email!", replyMarkup: KeyboardMenuBack);
                        break;
                    }
                    await workerWithFile.SetPosition(3, (new DataInFile
                    {
                        Date = DateTime.Now,
                        Name = string.Empty,
                        Email = TelegramMessage.Text,
                        Text = string.Empty
                    }));
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Напишите ваше сообщение!", replyMarkup: KeyboardMenuBack);
                    break;
                case 3:
                    var dataFromFile = await workerWithFile.GetData();
                    dataFromFile.Text = TelegramMessage.Text;
                    await CreateNewRequest(dataFromFile);
                    await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Успешно!");//Метод
                    await Hello();
                    await workerWithFile.SetPosition(0, false);
                    break;
                default:
                    break;
            }
        }

        public async  Task CreateNewRequest(DataInFile data)
        {
            var request = new Request()
            {
                Date = DateTime.Now,
                Name = data.Name,
                Email = data.Email,
                Text = data.Text,
                RequestTypeNum = 0,
                RequestStatusNum = 0
            };
            var json = JsonConvert.SerializeObject(request);
            Console.WriteLine(json);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await client.PostAsync("https://localhost:7276/api/CreateNewRequest", content);
            }
        }

        private async Task<bool> IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Простая проверка на соответствие формату email
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public async Task Hello()
        {
            await Bot.SendTextMessageAsync(TelegramMessage.Chat, "Доброго времени суток! Через меня вы можете создать заявку, чтобы менеджеры связались с вами! Для создания заявки нажмите на кнопку ниже!", replyMarkup: KeyboardMenuCreate);
        }

    }
}
