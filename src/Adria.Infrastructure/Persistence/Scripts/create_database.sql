/* 
ADD ALL NECESSARY TABLES, SEED DATA and POC DATA 
HINT: start with dropping tables if they exist
See example server for reference
*/

DROP TABLE IF EXISTS `OrderSupplementDetails`;
DROP TABLE IF EXISTS `HealthAnalyse`;
DROP TABLE IF EXISTS `FoodComposition`;
DROP TABLE IF EXISTS `Nutrients`;
DROP TABLE IF EXISTS `Scan`;
DROP TABLE IF EXISTS `Order`;
DROP TABLE IF EXISTS `Supplements`;
DROP TABLE IF EXISTS `Food`;
DROP TABLE IF EXISTS `User`;
DROP TABLE IF EXISTS `Subscription`;

-- Create Subscription table
CREATE TABLE `Subscription`
(
    `SubscriptionId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`           VARCHAR(100)            NOT NULL,
    `PricePerMonth` DOUBLE NOT NULL,
    `Advantages`     TEXT
);

-- Create User table
CREATE TABLE `User`
(
    `AdrianId`       VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`           VARCHAR(255)            NOT NULL,
    `Job`            VARCHAR(255),
    `SubscriptionId` VARCHAR(36)             NOT NULL,
    FOREIGN KEY (`SubscriptionId`) REFERENCES `Subscription` (`SubscriptionId`)
);

-- Create Food table
CREATE TABLE `Food`
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

-- Create Order table
CREATE TABLE `Order`
(
    `OrderId`    VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`   VARCHAR(36)             NOT NULL,
    `Date`       DATETIME                NOT NULL,
    `TotalPrice` INT                     NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `User` (`AdrianId`)
);

-- Create Nutrients table
CREATE TABLE `Nutrients`
(
    `NutrientId`        VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`              VARCHAR(100)            NOT NULL,
    `RecommendedAmount` VARCHAR(50)             NOT NULL
);

-- Create Scan table
CREATE TABLE `Scan`
(
    `ScanId`   VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId` VARCHAR(36)             NOT NULL,
    `DateTime` DATETIME                NOT NULL,
    `Result`   TEXT,
    `FoodId`   VARCHAR(36)             NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `User` (`AdrianId`),
    FOREIGN KEY (`FoodId`) REFERENCES `Food` (`FoodId`)
);

-- Create FoodComposition table (junction table)
CREATE TABLE `FoodComposition`
(
    `FoodId`     VARCHAR(36) NOT NULL,
    `NutrientId` VARCHAR(36) NOT NULL,
    `Amount`     VARCHAR(50) NOT NULL,
    PRIMARY KEY (`FoodId`, `NutrientId`),
    FOREIGN KEY (`FoodId`) REFERENCES `Food` (`FoodId`),
    FOREIGN KEY (`NutrientId`) REFERENCES `Nutrients` (`NutrientId`)
);

-- Create HealthAnalyse table
CREATE TABLE `HealthAnalyse`
(
    `AnalyseId`      VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`       VARCHAR(36)             NOT NULL,
    `DateTime`       DATETIME                NOT NULL,
    `Status`         VARCHAR(100)            NOT NULL,
    `Recommendation` TEXT,
    FOREIGN KEY (`AdrianId`) REFERENCES `User` (`AdrianId`)
);

-- Create OrderSupplementDetails table (junction table)
CREATE TABLE `OrderSupplementDetails`
(
    `OrderId`      VARCHAR(36) NOT NULL,
    `SupplementId` VARCHAR(36) NOT NULL,
    `Amount`       VARCHAR(50) NOT NULL,
    PRIMARY KEY (`OrderId`, `SupplementId`),
    FOREIGN KEY (`OrderId`) REFERENCES `Order` (`OrderId`),
    FOREIGN KEY (`SupplementId`) REFERENCES `Supplements` (`SupplementId`)
);
