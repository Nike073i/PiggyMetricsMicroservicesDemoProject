using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SenderService.Domain;

namespace SenderService.Services
{
    public class EmailMessageHandler : MessageHandler
    {
        private readonly IEmailService _emailService;

        public EmailMessageHandler(IEmailService emailService, MessageHandler nextHandler)
            : base(nextHandler)
        {
            _emailService = emailService;
        }

        public override async Task HandleMessageAsync(MessageToSend Letter)
        {
            if (!AddressIsEmail(Letter.RecipientAddress))
            {
                await _nextHandler?.HandleMessageAsync(Letter);
                return;
            }

            await _emailService.SendAsync(Letter);
        }

        private bool AddressIsEmail(string recipientAddress)
        {
            return recipientAddress.Contains("@"); // dummy test
        }
    }
}
