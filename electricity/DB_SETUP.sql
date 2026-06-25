CREATE DATABASE IF NOT EXISTS govaeb;
USE govaeb;

CREATE TABLE IF NOT EXISTS tblCustomer (
    cId INT AUTO_INCREMENT PRIMARY KEY,
    cName VARCHAR(100) NOT NULL,
    metno VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Address VARCHAR(255) NULL,
    phone VARCHAR(30) NULL,
    con_type VARCHAR(50) NULL,
    password VARCHAR(100) NOT NULL,
    units DOUBLE NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS tblBill (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    metno VARCHAR(50) NOT NULL,
    amount DOUBLE NOT NULL DEFAULT 0,
    status CHAR(1) NOT NULL DEFAULT 'N',
    PaidDate VARCHAR(50) NULL,
    INDEX idx_tblBill_metno (metno),
    CONSTRAINT fk_tblBill_tblCustomer_metno FOREIGN KEY (metno)
      REFERENCES tblCustomer(metno)
      ON UPDATE CASCADE
      ON DELETE CASCADE
);

-- IOTController integration schema
ALTER TABLE tblBill
    ADD COLUMN IF NOT EXISTS BillDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP;

CREATE TABLE IF NOT EXISTS tblRates (
    con_type VARCHAR(50) NOT NULL PRIMARY KEY,
    cost DOUBLE NOT NULL
);

CREATE TABLE IF NOT EXISTS readings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cid INT NOT NULL,
    data DOUBLE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_readings_cid (cid),
    INDEX idx_readings_created (created_at),
    CONSTRAINT fk_readings_tblCustomer_cid FOREIGN KEY (cid)
      REFERENCES tblCustomer(cId)
      ON UPDATE CASCADE
      ON DELETE CASCADE
);

INSERT INTO tblRates (con_type, cost)
VALUES
    ('domestic', 1.0),
    ('commercial', 2.0),
    ('industrial', 3.0)
ON DUPLICATE KEY UPDATE cost = VALUES(cost);
