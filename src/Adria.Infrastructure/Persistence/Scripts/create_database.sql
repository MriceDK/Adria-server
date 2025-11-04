/* 
ADD ALL NECESSARY TABLES, SEED DATA and POC DATA 
HINT: start with dropping tables if they exist
See example server for reference
*/

DROP TABLE IF EXISTS `OrderSupplementDetails`;
DROP TABLE IF EXISTS `HealthAnalyses`;
DROP TABLE IF EXISTS `FoodCompositions`;
DROP TABLE IF EXISTS `Nutrients`;
DROP TABLE IF EXISTS `Scans`;
DROP TABLE IF EXISTS `Orders`;
DROP TABLE IF EXISTS `Supplements`;
DROP TABLE IF EXISTS `Foods`;
DROP TABLE IF EXISTS `Users`;
DROP TABLE IF EXISTS `Subscriptions`;

-- Create Subscriptions table
CREATE TABLE `Subscriptions`
(
    `SubscriptionId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`           VARCHAR(100)            NOT NULL,
    `PricePerMonth` DOUBLE NOT NULL,
    `Advantages`     TEXT
);

-- Create Users table
CREATE TABLE `Users`
(
    `AdrianId`       VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`           VARCHAR(255)            NOT NULL,
    `Job`            VARCHAR(255),
    `SubscriptionId` VARCHAR(36)             NOT NULL,
    FOREIGN KEY (`SubscriptionId`) REFERENCES `Subscriptions` (`SubscriptionId`)
);

-- Create Foods table
CREATE TABLE `Foods`
(
    `FoodId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`   VARCHAR(255)            NOT NULL,
    `Type`   VARCHAR(100)            NOT NULL,
    `Edible` BOOLEAN                 NOT NULL DEFAULT TRUE
);

-- Create Supplements table
CREATE TABLE `Supplements`
(
    `SupplementId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`         VARCHAR(255)            NOT NULL,
    `Type`         VARCHAR(100)            NOT NULL,
    `Price` DOUBLE NOT NULL,
    `Stock`        INT                     NOT NULL DEFAULT 0
);

-- Create Orders table
CREATE TABLE `Orders`
(
    `OrderId`    VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`   VARCHAR(36)             NOT NULL,
    `Date`       DATETIME                NOT NULL,
    `TotalPrice` INT                     NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `Users` (`AdrianId`)
);

-- Create Nutrients table
CREATE TABLE `Nutrients`
(
    `NutrientId`        VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`              VARCHAR(100)            NOT NULL,
    `RecommendedAmount` VARCHAR(50)             NOT NULL
);

-- Create Scans table
CREATE TABLE `Scans`
(
    `ScanId`   VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId` VARCHAR(36)             NOT NULL,
    `DateTime` DATETIME                NOT NULL,
    `Result`   TEXT,
    `FoodId`   VARCHAR(36)             NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `Users` (`AdrianId`),
    FOREIGN KEY (`FoodId`) REFERENCES `Foods` (`FoodId`)
);

-- Create FoodCompositions table (junction table)
CREATE TABLE `FoodCompositions`
(
    `FoodId`     VARCHAR(36) NOT NULL,
    `NutrientId` VARCHAR(36) NOT NULL,
    `Amount`     VARCHAR(50) NOT NULL,
    PRIMARY KEY (`FoodId`, `NutrientId`),
    FOREIGN KEY (`FoodId`) REFERENCES `Foods` (`FoodId`),
    FOREIGN KEY (`NutrientId`) REFERENCES `Nutrients` (`NutrientId`)
);

-- Create HealthAnalyses table
CREATE TABLE `HealthAnalyses`
(
    `AnalyseId`      VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`       VARCHAR(36)             NOT NULL,
    `DateTime`       DATETIME                NOT NULL,
    `Status`         VARCHAR(100)            NOT NULL,
    `Recommendation` TEXT,
    FOREIGN KEY (`AdrianId`) REFERENCES `Users` (`AdrianId`)
);

-- Create OrderSupplementDetails table (junction table)
CREATE TABLE `OrderSupplementDetails`
(
    `OrderId`      VARCHAR(36) NOT NULL,
    `SupplementId` VARCHAR(36) NOT NULL,
    `Amount`       VARCHAR(50) NOT NULL,
    PRIMARY KEY (`OrderId`, `SupplementId`),
    FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`OrderId`),
    FOREIGN KEY (`SupplementId`) REFERENCES `Supplements` (`SupplementId`)
);