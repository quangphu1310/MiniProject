﻿namespace MiniProject_API.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
    }
}
