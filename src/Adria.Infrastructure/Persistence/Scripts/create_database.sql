/* 
ADD ALL NECESSARY TABLES, SEED DATA and POC DATA 
HINT: start with dropping tables if they exist
See example server for reference
*/

SET
FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS `pushSubscriptions`;
DROP TABLE IF EXISTS `healthAnalyse`;
DROP TABLE IF EXISTS `analyse`;
DROP TABLE IF EXISTS `bodyStats`;
DROP TABLE IF EXISTS `orderSupplementDetails`;
DROP TABLE IF EXISTS `healthAnalyses`;
DROP TABLE IF EXISTS `foodCompositions`;
DROP TABLE IF EXISTS `scans`;
DROP TABLE IF EXISTS `orders`;
DROP TABLE IF EXISTS `supplements`;
DROP TABLE IF EXISTS `foods`;
DROP TABLE IF EXISTS `nutrients`;
DROP TABLE IF EXISTS `users`;
DROP TABLE IF EXISTS `subscriptions`;

SET
FOREIGN_KEY_CHECKS = 1;

CREATE TABLE `subscriptions`
(
    `id`         VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`       VARCHAR(100)            NOT NULL,
    `PricePerMonth` DOUBLE NOT NULL,
    `Advantages` TEXT,
    `StartDate`  DATETIME                NOT NULL,
    `EndDate`    DATETIME                NOT NULL
);

CREATE TABLE `users`
(
    `AdrianId`       VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`           VARCHAR(255)            NOT NULL,
    `Job`            VARCHAR(255),
    `SubscriptionId` VARCHAR(36)             NOT NULL,
    CONSTRAINT `fk_users_subscription`
        FOREIGN KEY (`SubscriptionId`) REFERENCES `subscriptions` (`id`)
);
CREATE TABLE `pushSubscriptions`
(
    `SubscriptionId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `UserId`         VARCHAR(36)             NOT NULL,
    `Endpoint`       TEXT                    NOT NULL,
    `P256dh`         VARCHAR(255)            NOT NULL,
    `Auth`           VARCHAR(255)            NOT NULL,
    FOREIGN KEY (`UserId`) REFERENCES `users` (`AdrianId`)
);
CREATE TABLE `foods`
(
    `FoodId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`   VARCHAR(255)            NOT NULL,
    `Type`   VARCHAR(100)            NOT NULL,
    `Edible` BOOLEAN                 NOT NULL DEFAULT TRUE
);

CREATE TABLE `supplements`
(
    `SupplementId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`         VARCHAR(255)            NOT NULL,
    `Type`         VARCHAR(100)            NOT NULL,
    `Price` DOUBLE NOT NULL,
    `Stock`        INT                     NOT NULL DEFAULT 0
);

CREATE TABLE `nutrients`
(
    `NutrientId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`       VARCHAR(100)            NOT NULL,
    `Unit` DOUBLE NOT NULL,
    `Minimum`    VARCHAR(36)             NOT NULL,
    `Maximum`    VARCHAR(36)             NOT NULL
);

CREATE TABLE `orders`
(
    `OrderId`    VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`   VARCHAR(36)             NOT NULL,
    `Date`       DATETIME                NOT NULL,
    `TotalPrice` INT                     NOT NULL,
    CONSTRAINT `fk_orders_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `scans`
(
    `ScanId`   VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId` VARCHAR(36)             NOT NULL,
    `DateTime` DATETIME                NOT NULL,
    `Result`   TEXT,
    `FoodId`   VARCHAR(36)             NOT NULL,
    CONSTRAINT `fk_scans_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`),
    CONSTRAINT `fk_scans_food`
        FOREIGN KEY (`FoodId`) REFERENCES `foods` (`FoodId`)
);

CREATE TABLE `foodCompositions`
(
    `FoodId`     VARCHAR(36) NOT NULL,
    `NutrientId` VARCHAR(36) NOT NULL,
    `Amount`     DOUBLE NOT NULL,
    PRIMARY KEY (`FoodId`, `NutrientId`),
    CONSTRAINT `fk_foodcomp_food`
        FOREIGN KEY (`FoodId`) REFERENCES `foods` (`FoodId`),
    CONSTRAINT `fk_foodcomp_nutrient`
        FOREIGN KEY (`NutrientId`) REFERENCES `nutrients` (`NutrientId`)
);

CREATE TABLE `healthAnalyses`
(
    `AnalyseId`      VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`       VARCHAR(36)             NOT NULL,
    `DateTime`       DATETIME                NOT NULL,
    `Status`         VARCHAR(100)            NOT NULL,
    `Recommendation` TEXT,
    CONSTRAINT `fk_healthanalyses_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `orderSupplementDetails`
(
    `OrderId`      VARCHAR(36) NOT NULL,
    `SupplementId` VARCHAR(36) NOT NULL,
    `Amount`       VARCHAR(50) NOT NULL,
    PRIMARY KEY (`OrderId`, `SupplementId`),
    CONSTRAINT `fk_ordersupp_order`
        FOREIGN KEY (`OrderId`) REFERENCES `orders` (`OrderId`),
    CONSTRAINT `fk_ordersupp_supplement`
        FOREIGN KEY (`SupplementId`) REFERENCES `supplements` (`SupplementId`)
);


CREATE TABLE `bodyStats`
(
    `BodyStatId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Label`      VARCHAR(100)            NOT NULL,
    `Unit`       VARCHAR(20),
    `Goal` DOUBLE
);

CREATE TABLE `analyse`
(
    `AnalyseId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`  VARCHAR(36)             NOT NULL,
    `DateTime`  DATETIME                NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `healthAnalyse`
(
    `AnalyseId`  VARCHAR(36) NOT NULL,
    `BodyStatId` VARCHAR(36) NOT NULL,
    `Current` DOUBLE NOT NULL,
    PRIMARY KEY (`AnalyseId`, `BodyStatId`),
    FOREIGN KEY (`AnalyseId`) REFERENCES `analyse` (`AnalyseId`),
    FOREIGN KEY (`BodyStatId`) REFERENCES `bodyStats` (`BodyStatId`)
);

/* ========== SEED DATA ========== */

-- Insert Subscriptions
INSERT INTO subscriptions (id, `Type`, `PricePerMonth`, `Advantages`, `StartDate`, `EndDate`)
VALUES ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Basic', 9.99, 'Access to food database, Basic scanning features',
        '2025-01-01 00:00:00', '2026-01-01 00:00:00');

INSERT INTO subscriptions (id, `Type`, `PricePerMonth`, `Advantages`, `StartDate`, `EndDate`)
VALUES ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Premium', 19.99,
        'Unlimited scans, Health analysis, Personalized recommendations, Priority support',
        '2025-01-01 00:00:00', '2026-01-01 00:00:00');

INSERT INTO subscriptions (id, `Type`, `PricePerMonth`, `Advantages`, `StartDate`, `EndDate`)
VALUES ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Free', 0.00, 'Limited food database access, 5 scans per month',
        '2025-01-01 00:00:00', '2026-01-01 00:00:00');
-- Insert Users
INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'Adrian Martinez', 'Software Engineer',
        'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'Sarah Johnson', 'Fitness Trainer',
        'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'Michael Chen', 'Student', 'c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f');

INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'Emma Wilson', 'Nutritionist', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e');

INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'David Brown', 'Chef', 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d');

-- Insert Nutrients
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-prot-0001', 'Protein', 50, '45', '60');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-carb-0002', 'Carbohydrates', 300, '200', '350');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-fats-0003', 'Fats', 25, '20', '35');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-watr-0004', 'Water', 90, '70', '120');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-cali-0005', 'Calories', 1000, '2000', '3000');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-calc-0001', 'Calcium', 1000, '800', '1500');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-iron-0002', 'Iron', 18, '8', '27');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-magn-0003', 'Magnesium', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-phos-0004', 'Phosphorus', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-pota-0005', 'Potassium', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-sodi-0006', 'Sodium', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-zinc-0007', 'Zinc', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-copp-0008', 'Copper', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-mang-0009', 'Manganese', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-sele-0010', 'Selenium', 600, '400', '800');
INSERT INTO nutrients (`NutrientId`, `Type`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd2-iodi-0011', 'Iodine', 600, '400', '800');
-- Insert Foods
INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'Apple', 'Fruit', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'Banana', 'Fruit', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'Chicken Breast', 'Protein', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'Brown Rice', 'Grain', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'Broccoli', 'Vegetable', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'Salmon', 'Protein', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'Milk', 'Dairy', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'Spinach', 'Vegetable', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'Almonds', 'Nuts', TRUE);

INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('b6c7d8e9-f0a1-4b5c-3d4e-6f7a8b9c0d1e', 'Plastic Wrapper', 'Packaging', FALSE);

-- Insert FoodCompositions
-- Apple composition
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'bd1-carb-0002', 25);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'bd1-cali-0005', 52);

-- Banana
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'bd1-carb-0002', 27);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'bd1-cali-0005', 89);

-- Chicken Breast
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'bd1-prot-0001', 31);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'bd2-iron-0002', 1);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'bd1-cali-0005', 165);

-- Brown Rice
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'bd1-carb-0002', 77);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'bd2-iron-0002', 1.5);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'bd1-cali-0005', 123);

-- Broccoli
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'bd2-calc-0001', 47);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'bd1-cali-0005', 34);

-- Salmon
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'bd1-prot-0001', 20);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'bd1-cali-0005', 208);

-- Milk
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'bd1-prot-0001', 3.4);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'bd2-calc-0001', 125);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'bd1-cali-0005', 42);

-- Spinach
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'bd2-iron-0002', 2.7);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'bd2-calc-0001', 99);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'bd1-cali-0005', 23);

-- Almonds
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'bd1-prot-0001', 21);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'bd2-calc-0001', 269);
INSERT INTO foodCompositions (FoodId, NutrientId, Amount)
VALUES ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'bd1-cali-0005', 579);


-- Insert Supplements
INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', 'Whey Protein Powder', 'Protein', 29.99, 150);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('d8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', 'Multivitamin Complex', 'Vitamins', 19.99, 200);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', 'Omega-3 Fish Oil', 'Fatty Acids', 24.99, 100);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', 'Vitamin D3', 'Vitamin', 12.99, 175);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Calcium + Magnesium', 'Minerals', 15.99, 80);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Probiotic Complex', 'Digestive', 22.99, 60);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Iron Supplement', 'Mineral', 9.99, 120);

INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'BCAA Powder', 'Amino Acids', 27.99, 0);

-- Insert Orders
INSERT INTO orders (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-10-15 14:30:00', 75);

INSERT INTO orders (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', '2025-10-20 09:15:00', 43);

INSERT INTO orders (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-10-25 16:45:00', 53);

INSERT INTO orders (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', '2025-10-28 11:00:00', 89);

INSERT INTO orders (`OrderId`, `AdrianId`, `Date`, `TotalPrice`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', '2025-11-01 13:20:00', 35);

-- Insert OrderSupplementDetails
-- Order 1: Adrian's first order
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', '2');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', '1');

-- Order 2: Sarah's order
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'd8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', '1');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', '10');

-- Order 3: Adrian's second order
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', '2');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'd8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', '1');

-- Order 4: Emma's order
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', '1');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', '2');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', '1');

-- Order 5: David's order
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', '2');
INSERT INTO orderSupplementDetails (`OrderId`, `SupplementId`, `Amount`)
VALUES ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', '1');


-- 1. Daily Goals
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd1-prot-0001', 'Protein', 'g', 150),
       ('bd1-carb-0002', 'Carbohydrates', 'g', 250),
       ('bd1-fats-0003', 'Fats', 'g', 65),
       ('bd1-watr-0004', 'Water', 'ml', 2500);

-- 2. Minerals
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd2-calc-0001', 'Calcium', 'mg', 1000),
       ('bd2-iron-0002', 'Iron', 'mg', 18),
       ('bd2-magn-0003', 'Magnesium', 'mg', 400),
       ('bd2-phos-0004', 'Phosphorus', 'mg', 700),
       ('bd2-pota-0005', 'Potassium', 'mg', 3500),
       ('bd2-sodi-0006', 'Sodium', 'mg', 2300),
       ('bd2-zinc-0007', 'Zinc', 'mg', 11),
       ('bd2-copp-0008', 'Copper', 'mg', 0.9),
       ('bd2-mang-0009', 'Manganese', 'mg', 2.3),
       ('bd2-sele-0010', 'Selenium', 'μg', 55),
       ('bd2-iodi-0011', 'Iodine', 'μg', 150);

-- 3. Cholesterol
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd3-totl-0001', 'Cholesterol Total', 'mg/dL', 200),
       ('bd3-hdlc-0002', 'HDL Cholesterol', 'mg/dL', 40),
       ('bd3-ldlc-0003', 'LDL Cholesterol', 'mg/dL', 100),
       ('bd3-trig-0004', 'Triglycerides', 'mg/dL', 150);

-- 4. Basic Stats 
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd4-fatp-0001', 'Body Fat', '%', 25),
       ('bd4-weig-0002', 'Weight', 'kg', 75.0),
       ('bd4-hydr-0003', 'Hydration', '%', 100.0),
       ('bd4-hear-0004', 'Resting Heart Rate', 'bpm', 100.0),
       ('bd4-bmi-0005', 'BMI', '', 25.0),
       ('bd4-blood-0006', 'Blood Pressure', 'mmHg', 120.0),
       ('bd4-glucose-0007', 'Fasting Blood Glucose', 'mg/dL', 100.0);



INSERT INTO `analyse` (`AnalyseId`, `AdrianId`, `DateTime`)
VALUES ('an1-aaaa-bbbb-cccc-dddddddddddd', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-11-18 08:30:00');

INSERT INTO `healthAnalyse` (`AnalyseId`, `BodyStatId`, `Current`)
VALUES
-- 1. Daily Goals
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-prot-0001', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-carb-0002', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-fats-0003', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-watr-0004', 1),
-- 2. Minerals
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-calc-0001', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-iron-0002', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-magn-0003', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-phos-0004', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-pota-0005', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-sodi-0006', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-zinc-0007', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-copp-0008', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-mang-0009', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-sele-0010', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-iodi-0011', 1),
-- 3. Cholesterol
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-totl-0001', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-hdlc-0002', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-ldlc-0003', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-trig-0004', 1),
-- 4. Basic Stats
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-fatp-0001', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-weig-0002', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-hydr-0003', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-hear-0004', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-bmi-0005', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-blood-0006', 1),
('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-glucose-0007', 1);