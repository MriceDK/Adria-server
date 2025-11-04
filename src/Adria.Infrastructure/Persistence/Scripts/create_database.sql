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

/* ========== SEED DATA ========== */

-- Insert Subscriptions
INSERT INTO `Subscriptions` (`SubscriptionId`, `Type`, `PricePerMonth`, `Advantages`)
VALUES ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Basic', 9.99, 'Access to food database, Basic scanning features');

INSERT INTO `Subscriptions` (`SubscriptionId`, `Type`, `PricePerMonth`, `Advantages`)
VALUES ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Premium', 19.99,
        'Unlimited scans, Health analysis, Personalized recommendations, Priority support');

INSERT INTO `Subscriptions` (`SubscriptionId`, `Type`, `PricePerMonth`, `Advantages`)
VALUES ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Free', 0.00, 'Limited food database access, 5 scans per month');

-- Insert Users
INSERT INTO `Users` (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'Adrian Martinez', 'Software Engineer',
        'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO `Users` (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'Sarah Johnson', 'Fitness Trainer',
        'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO `Users` (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'Michael Chen', 'Student', 'c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f');

INSERT INTO `Users` (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'Emma Wilson', 'Nutritionist', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO `Users` (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'David Brown', 'Chef', 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d');

-- Insert Nutrients
INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'Protein', '50g per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('d0e1f2a3-b4c5-4d5e-7f8a-0b1c2d3e4f5a', 'Carbohydrates', '300g per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', 'Fiber', '25g per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', 'Vitamin C', '90mg per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', 'Calcium', '1000mg per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('b4c5d6e7-f8a9-4b5c-1d2e-4f5a6b7c8d9e', 'Iron', '18mg per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('c5d6e7f8-a9b0-4c5d-2e3f-5a6b7c8d9e0f', 'Vitamin D', '600IU per day');

INSERT INTO `Nutrients` (`NutrientId`, `Type`, `RecommendedAmount`)
VALUES ('d6e7f8a9-b0c1-4d5e-3f4a-6b7c8d9e0f1a', 'Omega-3', '250mg per day');

-- Insert Foods
INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'Apple', 'Fruit', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'Banana', 'Fruit', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'Chicken Breast', 'Protein', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'Brown Rice', 'Grain', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'Broccoli', 'Vegetable', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'Salmon', 'Protein', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'Milk', 'Dairy', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'Spinach', 'Vegetable', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'Almonds', 'Nuts', TRUE);

INSERT INTO `Foods` (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('b6c7d8e9-f0a1-4b5c-3d4e-6f7a8b9c0d1e', 'Plastic Wrapper', 'Packaging', FALSE);

-- Insert FoodCompositions
-- Apple composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'd0e1f2a3-b4c5-4d5e-7f8a-0b1c2d3e4f5a', '25g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', '4.4g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', '8mg per 100g');

-- Banana composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'd0e1f2a3-b4c5-4d5e-7f8a-0b1c2d3e4f5a', '27g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', '2.6g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', '10mg per 100g');

-- Chicken Breast composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', '31g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'b4c5d6e7-f8a9-4b5c-1d2e-4f5a6b7c8d9e', '1mg per 100g');

-- Brown Rice composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'd0e1f2a3-b4c5-4d5e-7f8a-0b1c2d3e4f5a', '77g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', '3.5g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'b4c5d6e7-f8a9-4b5c-1d2e-4f5a6b7c8d9e', '1.5mg per 100g');

-- Broccoli composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', '2.6g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', '89mg per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', '47mg per 100g');

-- Salmon composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', '20g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'c5d6e7f8-a9b0-4c5d-2e3f-5a6b7c8d9e0f', '570IU per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'd6e7f8a9-b0c1-4d5e-3f4a-6b7c8d9e0f1a', '2260mg per 100g');

-- Milk composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', '3.4g per 100ml');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', '125mg per 100ml');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'c5d6e7f8-a9b0-4c5d-2e3f-5a6b7c8d9e0f', '50IU per 100ml');

-- Spinach composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'b4c5d6e7-f8a9-4b5c-1d2e-4f5a6b7c8d9e', '2.7mg per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', '28mg per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', '99mg per 100g');

-- Almonds composition
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', '21g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', '12.5g per 100g');
INSERT INTO `FoodCompositions` (`FoodId`, `NutrientId`, `Amount`)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', '269mg per 100g');

-- Insert Supplements
INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', 'Whey Protein Powder', 'Protein', 29.99, 150);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('d8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', 'Multivitamin Complex', 'Vitamins', 19.99, 200);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', 'Omega-3 Fish Oil', 'Fatty Acids', 24.99, 100);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', 'Vitamin D3', 'Vitamin', 12.99, 175);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Calcium + Magnesium', 'Minerals', 15.99, 80);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Probiotic Complex', 'Digestive', 22.99, 60);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Iron Supplement', 'Mineral', 9.99, 120);

INSERT INTO `Supplements` (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'BCAA Powder', 'Amino Acids', 27.99, 0);

-- Insert Orders
INSERT INTO `Orders` (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-10-15 14:30:00', 75);

INSERT INTO `Orders` (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', '2025-10-20 09:15:00', 43);

INSERT INTO `Orders` (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-10-25 16:45:00', 53);

INSERT INTO `Orders` (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', '2025-10-28 11:00:00', 89);

INSERT INTO `Orders` (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', '2025-11-01 13:20:00', 35);

-- Insert OrderSupplementDetails
-- Order 1: Adrian's first order
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', '2');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', '1');

-- Order 2: Sarah's order
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'd8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', '1');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', '10');

-- Order 3: Adrian's second order
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', '2');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'd8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', '1');

-- Order 4: Emma's order
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', '1');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', '2');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', '1');

-- Order 5: David's order
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', '2');
INSERT INTO `OrderSupplementDetails` (`OrderId`, `SupplementId`, `Amount`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', '1');