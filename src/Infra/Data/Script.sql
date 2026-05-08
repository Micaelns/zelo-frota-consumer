Create Table Idempodencies(
    EventId UNIQUEIDENTIFIER PRIMARY KEY,
    MessageType VARCHAR(50),
    Payload NVARCHAR(MAX),
    ProcessStartAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ProcessEndAt DATETIME2 NULL
)