using ISpaniInnerweb.Domain.Interfaces.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Infrastructure.Settings
{
    public class EmailSettings : IEmailSettings
    {
        IConfiguration configuration;
        public EmailSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Server => configuration.GetValue<string>("Host");

        public int Port => configuration.GetValue<int>("Port");

        public string FromEmail => configuration.GetValue<string>("FromEmail");

        public string Username => configuration.GetValue<string>("Username");

        public string Password => configuration.GetValue<string>("Password");
    }
}
