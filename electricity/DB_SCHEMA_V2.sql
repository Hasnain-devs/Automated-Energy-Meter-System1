-- Normalized schema for Automated Energy Meter (AEM) backend
-- Safe to run multiple times.

CREATE DATABASE IF NOT EXISTS govaeb;
USE govaeb;

CREATE TABLE IF NOT EXISTS Users (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    LegacyCustomerId INT NULL,
    FullName VARCHAR(120) NOT NULL,
    Email VARCHAR(180) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'Customer',
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_users_email (Email),
    UNIQUE KEY uq_users_legacy_customer (LegacyCustomerId)
);

CREATE TABLE IF NOT EXISTS Meters (
    MeterPk INT AUTO_INCREMENT PRIMARY KEY,
    MeterId VARCHAR(64) NOT NULL,
    UserId INT NULL,
    DeviceKey VARCHAR(100) NOT NULL,
    SharedSecret VARCHAR(128) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    InstalledAtUtc DATETIME NULL,
    LastSeenAtUtc DATETIME NULL,
    CreatedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_meters_meter_id (MeterId),
    UNIQUE KEY uq_meters_device_key (DeviceKey),
    KEY idx_meters_user (UserId),
    CONSTRAINT fk_meters_users FOREIGN KEY (UserId) REFERENCES Users(UserId)
      ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS TariffPlans (
    TariffPlanId INT AUTO_INCREMENT PRIMARY KEY,
    PlanName VARCHAR(80) NOT NULL,
    RatePerUnit DECIMAL(12,4) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    EffectiveFromUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_tariff_plan_name (PlanName)
);

CREATE TABLE IF NOT EXISTS BillingCycles (
    CycleId INT AUTO_INCREMENT PRIMARY KEY,
    CycleStartUtc DATETIME NOT NULL,
    CycleEndUtc DATETIME NOT NULL,
    IsClosed TINYINT(1) NOT NULL DEFAULT 0,
    ClosedAtUtc DATETIME NULL,
    UNIQUE KEY uq_cycle_range (CycleStartUtc, CycleEndUtc)
);

CREATE TABLE IF NOT EXISTS Bills (
    BillId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    MeterPk INT NOT NULL,
    CycleId INT NOT NULL,
    UnitsConsumed DECIMAL(12,4) NOT NULL,
    RatePerUnit DECIMAL(12,4) NOT NULL,
    Amount DECIMAL(12,4) NOT NULL,
    DueDateUtc DATETIME NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Unpaid',
    CreatedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PaidAtUtc DATETIME NULL,
    KEY idx_bills_user (UserId),
    KEY idx_bills_meter (MeterPk),
    KEY idx_bills_cycle (CycleId),
    CONSTRAINT fk_bills_users FOREIGN KEY (UserId) REFERENCES Users(UserId)
      ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_bills_meters FOREIGN KEY (MeterPk) REFERENCES Meters(MeterPk)
      ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_bills_cycles FOREIGN KEY (CycleId) REFERENCES BillingCycles(CycleId)
      ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS Payments (
    PaymentId INT AUTO_INCREMENT PRIMARY KEY,
    BillId INT NOT NULL,
    Amount DECIMAL(12,4) NOT NULL,
    Provider VARCHAR(50) NULL,
    TxnRef VARCHAR(120) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    PaidAtUtc DATETIME NULL,
    CreatedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_payments_txn_ref (TxnRef),
    KEY idx_payments_bill (BillId),
    CONSTRAINT fk_payments_bills FOREIGN KEY (BillId) REFERENCES Bills(BillId)
      ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS MeterReadings (
    ReadingId BIGINT AUTO_INCREMENT PRIMARY KEY,
    MeterPk INT NOT NULL,
    SequenceNo BIGINT NOT NULL,
    UnitsDelta DECIMAL(12,4) NOT NULL,
    ReadingAtUtc DATETIME NOT NULL,
    ReceivedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    RawPayload MEDIUMTEXT NULL,
    Signature VARCHAR(128) NULL,
    UNIQUE KEY uq_meter_seq (MeterPk, SequenceNo),
    KEY idx_meter_reading_time (MeterPk, ReadingAtUtc),
    KEY idx_meter_received_time (ReceivedAtUtc),
    CONSTRAINT fk_readings_meters FOREIGN KEY (MeterPk) REFERENCES Meters(MeterPk)
      ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS AuditLogs (
    AuditLogId BIGINT AUTO_INCREMENT PRIMARY KEY,
    ActorType VARCHAR(30) NOT NULL,
    ActorId VARCHAR(100) NULL,
    Action VARCHAR(120) NOT NULL,
    EntityName VARCHAR(80) NULL,
    EntityId VARCHAR(80) NULL,
    Payload MEDIUMTEXT NULL,
    CreatedAtUtc DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY idx_audit_created (CreatedAtUtc)
);

-- Seed default tariff plans
INSERT INTO TariffPlans (PlanName, RatePerUnit, IsActive)
VALUES
    ('domestic', 1.0000, 1),
    ('commercial', 2.0000, 1),
    ('industrial', 3.0000, 1)
ON DUPLICATE KEY UPDATE RatePerUnit = VALUES(RatePerUnit), IsActive = VALUES(IsActive);

-- Legacy migration from existing tblCustomer (if present)
INSERT INTO Users (LegacyCustomerId, FullName, Email, PasswordHash, Role, IsActive)
SELECT c.cId, c.cName, c.Email, c.password, 'Customer', 1
FROM tblCustomer c
ON DUPLICATE KEY UPDATE
    FullName = VALUES(FullName),
    PasswordHash = VALUES(PasswordHash),
    IsActive = VALUES(IsActive);

INSERT INTO Meters (MeterId, UserId, DeviceKey, SharedSecret, IsActive)
SELECT c.metno,
       u.UserId,
       CONCAT('dev-', c.metno),
       LOWER(SHA2(CONCAT('secret-', c.metno), 256)),
       1
FROM tblCustomer c
JOIN Users u ON u.LegacyCustomerId = c.cId
ON DUPLICATE KEY UPDATE
    UserId = VALUES(UserId),
    IsActive = VALUES(IsActive);
