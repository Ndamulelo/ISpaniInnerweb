﻿namespace ISpaniInnerweb.Domain.Interfaces.Settings
{
    public interface IFileUploadSettings
    {
        string StorageName { get; }
        string ConnectionString { get; }
    }
}
