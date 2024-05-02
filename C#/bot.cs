
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using System.Collections.Generic;

class Program
{
    static TelegramBotClient bot;
    static int mode = 0;
    static string helloMessage = "Данный бот предназначен для добавлений сообщений, медиа, текста и отправки их в разные соцсети \nЧтобы добавить файлы для отправки в такие соцсети как телеграмм, твиттер, то нужно активировать команду /social \nЧтобы добавить видео для отправки в ютуб, то нужно активировать команду /youtube \nЧтобы очистить загруженные боту сообщения, активируйте команду /remove \nПо завершению добавлению файлов нужно активировать команду /send \nПриятного использования!";

    static List<BotCommand> commands = new List<BotCommand>
    {
        new BotCommand("/social", "Режим отправки для соцсетей"),
        new BotCommand("/youtube", "Режим отправки для Youtube"),
        new BotCommand("/remove", "Очистка отправленных боту файлов из его хранилища"),
        new BotCommand("/help", "Мини-инструкция по пользованию ботом"),
        new BotCommand("/send", "Отправка сообщений в ранее выбраный режим"),
        new BotCommand("/quit", "Завершение работы бота")
    };

    static void Main(string[] args)
    {
        Console.WriteLine("Replycator BOT 0.01");

        bot = new TelegramBotClient("YOUR_BOT_API_TOKEN");
        bot.OnMessage += Bot_OnMessage;
        bot.StartReceiving();

        Console.ReadLine();
    }

    private static async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        var message = e.Message;

        if (message.Type == MessageType.Text)
        {
            switch (message.Text.ToLower())
            {
                case "/start":
                    await bot.SendTextMessageAsync(message.Chat.Id, helloMessage);
                    break;
                case "/quit":
                    await bot.SendTextMessageAsync(message.Chat.Id, "Выключение бота");
                    Environment.Exit(0);
                    break;
                case "/help":
                    await bot.SendTextMessageAsync(message.Chat.Id, helloMessage);
                    break;
                case "/social":
                    if (mode == 0 || mode == 2)
                    {
                        mode = 1;
                        Directory.DeleteFiles("files/");
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим social");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Данный режим уже выбран");
                    }
                    break;
                case "/youtube":
                    if (mode == 0 || mode == 1)
                    {
                        mode = 2;
                        Directory.DeleteFiles("files/");
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим youtube");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Данный режим уже выбран");
                    }
                    break;
                case "/remove":
                    Directory.DeleteFiles("files/");
                    await bot.SendTextMessageAsync(message.Chat.Id, "Хранилище очищено");
                    break;
                case "/send":
                    int fileCount = Directory.GetFiles("files/").Length;
                    if (fileCount == 0)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Нечего отправлять, папка пуста");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Идёт отправка {string.Join(", ", Directory.GetFiles("files/"))}");
                        bool sendResult = SendMessage(mode, File.Exists("text.txt"));
                        await bot.SendTextMessageAsync(message.Chat.Id, sendResult ? "Сообщения отправлены" : "Сообщения не отправлены");
                    }
                    break;
                default:
                    if (mode == 0)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Не выбран режим");
                    }
                    else if (mode == 1)
                    {
                        File.AppendAllText("files/text.txt", $"{message.Text}\n");
                        Console.WriteLine($"Принят текст: {message.Text}");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим youtube");
                    }
                    break;
            }
        }
        else if (message.Type == MessageType.Audio)
        {
            if (mode == 0)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Не выбран режим");
            }
            else if (mode == 1)
            {
                var fileId = message.Audio.FileId;
                var fileInfo = await bot.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                var fileName = $"files/mus{new Random().Next(1, 999999)}.mp3";
                await bot.DownloadFileAsync(filePath, fileName);
                Console.WriteLine("Принято аудио");
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим youtube");
            }
        }
        else if (message.Type == MessageType.Photo)
        {
            if (mode == 0)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Не выбран режим");
            }
            else if (mode == 1)
            {
                var fileId = message.Photo.LastOrDefault()?.FileId;
                var fileInfo = await bot.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                var fileName = $"files/img{new Random().Next(1, 999999)}.jpg";
                await bot.DownloadFileAsync(filePath, fileName);
                Console.WriteLine("Принято изображение");
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим youtube");
            }
        }
        else if (message.Type == MessageType.Document)
        {
            if (mode == 0)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Не выбран режим");
            }
            else if (mode == 1)
            {
                var fileId = message.Document.FileId;
                var fileInfo = await bot.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                var fileName = $"files/{message.Document.FileName}";
                await bot.DownloadFileAsync(filePath, fileName);
                Console.WriteLine("Получен документ");
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим youtube");
            }
        }
        else if (message.Type == MessageType.Video)
        {
            if (mode == 0)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Не выбран режим");
            }
            else if (mode == 1)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Выбран режим social");
            }
            else
            {
                var fileId = message.Video.FileId;
                var fileInfo = await bot.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                var fileName = $"files/vid{new Random().Next(1, 999999)}.mp4";
                await bot.DownloadFileAsync(filePath, fileName);
                Console.WriteLine("Принято видео");
            }
        }
    }

    static bool SendMessage(int mode, bool hasText)
    {
        if (mode == 1)
        {
            // replycator_api.send('social');
            return true;
        }
        else if (mode == 2)
        {
            // replycator_api.send('youtube-social');
            return true;
        }
        return false;
    }
}