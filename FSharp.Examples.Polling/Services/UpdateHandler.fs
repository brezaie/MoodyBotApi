// Update Handler class inherited from IUpdateHandler
//
// This file simply forms that "link" between the .NET
// class to the actual implementation of the handlers
// as seen in the module
// FSharp.Examples.Polling.Services.Internal.UpdateHandlerFuncs
//
// Copyright (c) 2023 Arvind Devarajan
// Licensed to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace FSharp.Examples.Polling.Services

open System
open System.Threading
open Microsoft.Extensions.Logging
open Telegram.Bot
open Telegram.Bot.Polling

open FSharp.Examples.Polling.Services.Internal

type UpdateHandler(botClient: ITelegramBotClient, logger: ILogger<UpdateHandler>) =
  interface IUpdateHandler with
    member __.HandleUpdateAsync( _ , update, cancellation) =
      UpdateHandlerFuncs.handleUpdateAsync botClient logger cancellation update |> Async.StartAsTask :> Tasks.Task

    member __.HandlePollingErrorAsync( _ , ex: Exception, cancellationToken: CancellationToken) =
      UpdateHandlerFuncs.handlePollingErrorAsync botClient logger cancellationToken ex |> Async.StartAsTask :> Tasks.Task
