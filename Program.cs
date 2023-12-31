﻿using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

Console.WriteLine("Please Enter Telegram Bot API");
var botAPI = Console.ReadLine();

var botClient = new TelegramBotClient(botAPI);
var cts = new CancellationTokenSource();
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
    CancellationToken cancellationToken)
{
    if (update.ChannelPost != null)
    {
        var chatId2 = update.ChannelPost.Chat.Id;
        Console.WriteLine($"Received a '{update.ChannelPost.Text}' message in chat {chatId2}.");
    }
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;
    var chatId = message.Chat.Id;
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Your chatID: " + chatId,
        cancellationToken: cancellationToken);
    Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
    CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}\nPress any key to Eliminate Program");
Console.ReadKey();